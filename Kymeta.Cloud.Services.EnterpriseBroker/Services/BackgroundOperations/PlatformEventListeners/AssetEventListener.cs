using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;
using CometD.NetCore.Salesforce.Messaging;
using System.Reflection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners;

// inherit from IMessageListener
public interface IAssetEventListener : IMessageListener { }

public class AssetEventListener : IAssetEventListener
{
    private readonly IConfiguration _config;
    private readonly ILogger<AssetEventListener> _logger;
    private readonly ICacheRepository _cacheRepo;
    private readonly ITerminalsClient _terminalsClient;
    private readonly ISalesforceAssetService _sfAssetService;

    public AssetEventListener(IConfiguration config, ILogger<AssetEventListener> logger, ICacheRepository cacheRepo, ITerminalsClient terminalsClient, ISalesforceAssetService sfAssetService)
    {
        _config = config;
        _logger = logger;
        _cacheRepo = cacheRepo;
        _terminalsClient = terminalsClient;
        _sfAssetService = sfAssetService;
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
            var assetEvent = JsonConvert.DeserializeObject<MessageEnvelope<SalesforceAssetUpdatedEventPayload>>(eventContent);
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

            // isolate the payload
            var payload = assetEvent.Data.Payload;
            _logger.LogInformation($"Message received ({payload.CreatedDate}) - Name: {payload.Name}");

            // call to SF asset service to process the payload
            var result = _sfAssetService.ProcessAssetUpdateEvent(payload).Result;
            if (!result.isSuccess)
            {
                _logger.LogWarning(result.error);
                return;
            }
        }
        catch (Exception ex)
        {
            // TODO: log critical?
            _logger.LogCritical($"An unexpected error occurred while attempting to process Salesforce Platform Event message: {ex.Message}", message);
            throw;
        }
    }
}