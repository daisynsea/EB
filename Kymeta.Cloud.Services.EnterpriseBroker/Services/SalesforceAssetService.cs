using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;
using Microsoft.Azure.ServiceBus;
using System.Reflection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface ISalesforceAssetService
{
    Task<(bool isSuccess, string? error)> ProcessAssetUpdateEvent(SalesforceAssetUpdatedEventPayload payload);
}

public class SalesforceAssetService : ISalesforceAssetService
{
    public readonly ILogger<SalesforceAssetService> _logger;
    public readonly ITerminalsClient _terminalsClient;

    public SalesforceAssetService(ILogger<SalesforceAssetService> logger, ITerminalsClient terminalsClient)
    {
        _logger = logger;
        _terminalsClient = terminalsClient;
    }

    public async Task<(bool isSuccess, string? error)> ProcessAssetUpdateEvent(SalesforceAssetUpdatedEventPayload payload)
    {
        // isolate the product family
        var productFamilyLower = payload.ProductFamily.ToLower();
        // use ProductFamily to determine if Terminal or Component
        if (productFamilyLower == "terminal") // asset is a Terminal
        {
            // fetch Terminal by SalesforceId (AssetId)
            var terminalResult = await _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.AssetId });
            if (!string.IsNullOrEmpty(terminalResult.error))
            {
                _logger.LogCritical($"Encountered an error while attempting to fetch Terminal with Salesforce Id '{payload.AssetId}': {terminalResult.error}");
                return new ValueTuple<bool, string?>(false, terminalResult.error);
            }
            // isolate the result
            var terminal = terminalResult.res?.Items?.FirstOrDefault();
            if (terminal == null)
            {
                // TODO: create new Terminal?
                _logger.LogCritical($"Terminal not found.");
                return new ValueTuple<bool, string?>(false, "Terminal not found.");
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
                    return new ValueTuple<bool, string?>(false, updatedTerminal.error);
                }
            }
        }
        else if (productFamilyLower == "components") // asset is a hardware component
        {
            // fetch Product Types from OSS data store (via Terminals)
            var productInfoResult = _terminalsClient.GetProductTypes().Result;
            // isolate a match based on the payload of the event message
            var productType = productInfoResult.productTypes?.FirstOrDefault(pt => pt.ProductCode == payload.ProductCode);
            // validate the product type data has all the segements we need to propertly set the hardware value on the Terminal
            if (productType == null || string.IsNullOrEmpty(productType.BaseProperty) || string.IsNullOrEmpty(productType.TargetProperty))
            {
                var message = $"Product not recognized: '{payload.ProductCode}'";
                _logger.LogCritical(message);
                return new ValueTuple<bool, string?>(false, message);
            }
            // declare the fully qualified property name of the related hardware component
            var terminalHardwareProperty = $"Hardware.{productType.BaseProperty}.{productType.TargetProperty}";
            var isTargetUpdated = false;

            // check to see if the Parent of the component was modified
            if (payload.PriorParentId != payload.ParentId)
            {
                // fetch Original and Target Terminals by Salesforce Ids
                var parentTerminals = _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.PriorParentId, payload.ParentId }).Result;
                // handle error if unable to fetch Terminals
                if (string.IsNullOrEmpty(parentTerminals.error))
                {
                    var msg = $"Unable to fetch OSS Terminals due to error: {parentTerminals.error}";
                    _logger.LogCritical(msg);
                    return new ValueTuple<bool, string?>(false, msg);
                }

                // remove the component from the original Terminal
                var isOriginalUpdated = UpdateTerminalHardware(parentTerminals, payload.PriorParentId, null, terminalHardwareProperty, payload.AssetId);

                // isolate the 'Target' Terminal for which to set the hardware component
                isTargetUpdated = UpdateTerminalHardware(parentTerminals, payload.ParentId, payload.Serial, terminalHardwareProperty, payload.AssetId);
            }

            // check to see if Serial was modified
            // if target Terminal was already updated above, bypass this snippet
            if (payload.Serial != payload.PriorSerial && !isTargetUpdated)
            {
                // fetch Target Terminals by Salesforce Id
                var parentTerminals = _terminalsClient.GetTerminalsBySalesforceIds(new string[] { payload.ParentId }).Result;
                // handle error if unable to fetch Terminals
                if (string.IsNullOrEmpty(parentTerminals.error))
                {
                    var msg = $"Unable to fetch OSS Terminals due to error: {parentTerminals.error}";
                    _logger.LogCritical(msg);
                    return new ValueTuple<bool, string?>(false, msg);
                }
                var isUpdated = UpdateTerminalHardware(parentTerminals, payload.ParentId, payload.Serial, terminalHardwareProperty, payload.AssetId);
            }
        }
        else
        {
            // TODO: throw error? do nothing?
            var message = $"Product Family not recognized.";
            _logger.LogCritical(message);
            return new ValueTuple<bool, string?>(false, message);
        }

        return new ValueTuple<bool, string?>(true, null);
    }

    private bool UpdateTerminalHardware((TerminalsResponse res, string error) parentTerminals, string parentId, string componentValue, string terminalHardwareProperty, string assetId)
    {
        // isolate the Terminal for which to set the hardware component
        var terminal = parentTerminals.res?.Items?.FirstOrDefault(i => i.SalesforceAssetId == parentId);
        if (terminal == null)
        {
            // TODO: Terminal not found - critical error?
            _logger.LogCritical($"Terminal not found. Salesforce Id '{parentId}'");
            return false;
        }

        // compount assignment to ensure a history has been initilaized on the Terminal
        terminal.ComponentHistory ??= new List<ComponentHistoryRecord>();

        // find the matching history entry for the given component
        var historyMatch = terminal.ComponentHistory
            .Where(ch => ch.AssetId == assetId && !ch.RemovedOn.HasValue)
            .FirstOrDefault();
        if (historyMatch == null)
        {
            // TODO: match not found... critical error... who to alert?
            _logger.LogCritical($"Component History entry not found for asset '{assetId}'");
        }
        else
        {
            // set the RemovedOn value
            historyMatch.RemovedOn = DateTime.UtcNow;
        }

        // using reflection, update relevant hardware component value
        SetProperty(terminalHardwareProperty, terminal, componentValue);
        // only add history entry if a component value is present
        if (!string.IsNullOrEmpty(componentValue))
        {
            // update ComponentHistory to add new entry
            terminal.ComponentHistory.Add(new ComponentHistoryRecord()
            {
                AddedOn = DateTime.UtcNow,
                AssetId = assetId,
                Serial = componentValue,
                Type = terminalHardwareProperty
            });
        }
        // update the Terminal object
        var updatedResult = _terminalsClient.UpdateTerminal(terminal).Result;
        if (!string.IsNullOrEmpty(updatedResult.error))
        {
            _logger.LogCritical($"Encountered an error while attempting to update Terminal: {updatedResult.error}");
            return false;
        }

        // successful update
        return true;
    }

    /// <summary>
    /// Using reflection to set a property via the string equivalent of the property name.
    /// </summary>
    /// <param name="compoundProperty">The full reference to the intended property to set.</param>
    /// <param name="target">The target object (Terminal in this case) to update.</param>
    /// <param name="value">The value to set for the given property.</param>
    private void SetProperty(string compoundProperty, object target, object value)
    {
        // isolate the segments of the property reference
        string[] bits = compoundProperty.Split('.');

        // iterate the segements to dive into the requisite property for proper context
        for (int i = 0; i < bits.Length - 1; i++)
        {
            PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
            // if a nested property, keep updating the target until final reference is found
            target = propertyToGet.GetValue(target, null);
        }

        // isolate the property to set using the target object type
        PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
        // set the value of the property
        propertyToSet.SetValue(target, value, null);
    }

    /// <summary>
    /// Using reflection to get a property via the string equivalent of the property name.
    /// </summary>
    /// <param name="compoundProperty">The full reference to the intended property to get.</param>
    /// <param name="target">The target object (Terminal in this case).</param>
    private string GetProperty(string compoundProperty, object target)
    {
        // isolate the segments of the property reference
        string[] bits = compoundProperty.Split('.');

        // iterate the segements to dive into the requisite property for proper context
        for (int i = 0; i < bits.Length - 1; i++)
        {
            PropertyInfo propertySegmentToGet = target.GetType().GetProperty(bits[i]);
            // if a nested property, keep updating the target until final reference is found
            target = propertySegmentToGet.GetValue(target, null);
        }

        // isolate the property to set using the target object type
        PropertyInfo propertyToGet = target.GetType().GetProperty(bits.Last());
        // get the value of the property
        var propertyValue = propertyToGet.GetValue(target).ToString();
        return propertyValue;
    }
}
