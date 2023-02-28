using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;
using CometD.NetCore.Salesforce.Messaging;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners
{
    public interface IAssetSerialUpdateEventListener : IMessageListener
    {
    }

    public class AssetSerialUpdateEventListener : IAssetSerialUpdateEventListener
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AssetEventListener> _logger;
        private readonly ICacheRepository _cacheRepo;
        private readonly ITerminalsClient _terminalsClient;

        public AssetSerialUpdateEventListener(IConfiguration config, ILogger<AssetEventListener> logger, ICacheRepository cacheRepo, ITerminalsClient terminalsClient)
        {
            _config = config;
            _logger = logger;
            _cacheRepo = cacheRepo;
            _terminalsClient = terminalsClient;
        }

        /// <summary>
        /// Listen for messages from Salesforce Platform Events for Assets
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            try
            {
                // fetch the JSON
                var eventContent = message.Json;
                if (eventContent == null)
                {
                    _logger.LogCritical($"[PLATFORM_EVENTS] Message content not available.");
                    return;
                }
                // deserialize JSON into C# model
                var assetEvent = JsonConvert.DeserializeObject<MessageEnvelope<SalesforceAssetSerialUpdatedEventPayload>>(eventContent);
                if (assetEvent == null || assetEvent.Data?.Event == null)
                {
                    _logger.LogCritical($"[PLATFORM_EVENTS] Unable to deserialize message payload: Asset event not recognized.");
                    return;
                }
                // assign replayId to redis cache to establish replay starting point in event of service failure
                _cacheRepo.SetSalesforceEventReplayId(_config["Salesforce:PlatformEvents:Channels:Asset"], assetEvent.Data.Event.ReplayId.ToString());
                // ensure we have a payload from the event message
                if (assetEvent == null || assetEvent.Data?.Payload == null)
                {
                    _logger.LogCritical($"[PLATFORM_EVENTS] Asset payload not found.");
                    return;
                }

                var payload = assetEvent.Data.Payload;
                _logger.LogInformation($"Message received ({payload.CreatedDate}) - Name: {payload.Name}");

                // isolate the product family
                var productFamilyLower = payload.ProductFamily.ToLower();
                // use ProductFamily to determine if Terminal or Component
                if (productFamilyLower == "terminal") // asset is a Terminal
                {
                    // fetch Terminal by SalesforceId (AssetId)
                    var terminalResult = _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.AssetId }).Result;
                    if (!string.IsNullOrEmpty(terminalResult.error))
                    {
                        _logger.LogCritical($"Encountered an error while attempting to fetch Terminal with Salesforce Id '{payload.AssetId}': {terminalResult.error}");
                        return;
                    }
                    // isolate the result
                    var terminal = terminalResult.res?.Items?.FirstOrDefault();
                    if (terminal == null)
                    {
                        // TODO: create new Terminal?
                        _logger.LogCritical($"Terminal not found");
                        return;
                    }
                    // check if Serial differs from OSS record
                    if (terminal.Serial != payload.Serial)
                    {
                        // update OSS Terminal Serial with Asset.Serial
                        terminal.Serial = payload.Serial;
                        var updatedTerminal = _terminalsClient.UpdateTerminal(terminal).Result;
                        // check for error from update command
                        if (!string.IsNullOrEmpty(updatedTerminal.error))
                        {
                            _logger.LogCritical($"Encountered an error while attempting to update Terminal.");
                            return;
                        }
                    }
                }
                else if (productFamilyLower == "component") // asset is a hardware component
                {
                    // fetch Product Types from OSS data store (via Terminals)
                    var productInfoResult = _terminalsClient.GetProductTypes().Result;
                    // isolate a match based on the payload of the event message
                    var productType = productInfoResult.productTypes?.FirstOrDefault(pt => pt.ProductCode == payload.ProductCode);
                    if (productType == null)
                    {
                        _logger.LogCritical($"Product not recognized: '{payload.ProductCode}'");
                        return;
                    }
                    // declare the fully qualified property name of the related hardware component
                    var terminalHardwareProperty = $"Hardware.{productType.HardwareField}";
                    var isTargetUpdated = false;

                    // check to see if the Parent of the component was modified
                    if (payload.PriorParentId != payload.ParentId)
                    {
                        // fetch Original and Target Terminals by Salesforce Ids
                        var parentTerminals = _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.PriorParentId, payload.ParentId }).Result;
                        //var isOriginalUpdated = UpdateTerminal(parentTerminals, payload.PriorParentId, null, terminalHardwareProperty);
                        // isolate the original
                        var originalTerminal = parentTerminals.res?.Items?.FirstOrDefault(i => i.SalesforceAssetId == payload.PriorParentId);
                        if (originalTerminal == null)
                        {
                            // TODO: original terminal not found - critical error? create new Terminal?
                            _logger.LogCritical($"Terminal not found. Salesforce Id '{payload.PriorParentId}'");
                            return;
                        }
                        else
                        {
                            // update relevant hardware component serial value with null to remove the reference from the original
                            SetProperty(terminalHardwareProperty, originalTerminal, null);
                        }

                        // isolate the 'Target' Terminal for which to set the hardware component
                        var targetTerminal = parentTerminals.res?.Items?.FirstOrDefault(i => i.SalesforceAssetId == payload.ParentId);
                        if (targetTerminal == null)
                        {
                            // TODO: target Terminal not found - critical error?
                            _logger.LogCritical($"Terminal not found. Salesforce Id '{payload.ParentId}'");
                            return;
                        }
                        else
                        {
                            // update relevant hardware component serial value
                            SetProperty(terminalHardwareProperty, targetTerminal, payload.Serial);
                            var updatedTargetResult = _terminalsClient.UpdateTerminal(targetTerminal).Result;
                            if (!string.IsNullOrEmpty(updatedTargetResult.error))
                            {
                                _logger.LogCritical($"Encountered an error while attempting to update Target Terminal: {updatedTargetResult.error}");
                            }
                            isTargetUpdated = true;
                        }
                    }

                    // check to see if Serial was modified
                    // if target Terminal was already updated above, bypass this snippet
                    if (payload.Serial != payload.PriorSerial && !isTargetUpdated)
                    {
                        // fetch Target Terminals by Salesforce Id
                        var parentTerminals = _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.ParentId }).Result;
                        var isUpdated = UpdateTerminal(parentTerminals, payload.ParentId, payload.Serial, terminalHardwareProperty);
                    }
                }
                else
                {
                    // TODO: throw error? do nothing?
                    _logger.LogCritical($"Product Family not recognized.");
                }
            }
            catch (Exception ex)
            {
                // TODO: log critical?
                throw;
            }
        }

        private bool UpdateTerminal((TerminalsResponse res, string error) parentTerminals, string parentId, string serial, string terminalHardwareProperty)
        {
            // isolate the Terminal for which to set the hardware component
            var terminal = parentTerminals.res?.Items?.FirstOrDefault(i => i.SalesforceAssetId == parentId);
            if (terminal == null)
            {
                // TODO: Terminal not found - critical error?
                _logger.LogCritical($"Terminal not found. Salesforce Id '{parentId}'");
                return false;
            }
            else
            {
                // update relevant hardware component serial value
                SetProperty(terminalHardwareProperty, terminal, serial);
                var updatedResult = _terminalsClient.UpdateTerminal(terminal).Result;
                if (!string.IsNullOrEmpty(updatedResult.error))
                {
                    _logger.LogCritical($"Encountered an error while attempting to update Terminal: {updatedResult.error}");
                    return false;
                }
            }
            return true;
        }

        private void SetProperty(string compoundProperty, object target, object value)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                target = propertyToGet.GetValue(target, null);
            }
            PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
            propertyToSet.SetValue(target, value, null);
        }
    }
}
