using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;
using CometD.NetCore.Salesforce.Messaging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners;

// inherit from IMessageListener
public interface ISubscriptionEventListener : IMessageListener { }

public class SubscriptionEventListener : ISubscriptionEventListener
{
    private readonly IConfiguration _config;
    private readonly ILogger<SubscriptionEventListener> _logger;
    private readonly ICacheRepository _cacheRepo;
    private readonly ITerminalsClient _terminalsClient;

    public SubscriptionEventListener(IConfiguration config, ILogger<SubscriptionEventListener> logger, ICacheRepository cacheRepo, ITerminalsClient terminalsClient)
    {
        _config = config;
        _logger = logger;
        _cacheRepo = cacheRepo;
        _terminalsClient = terminalsClient;
    }

    /// <summary>
    /// Listen for messages from Salesforce Platform Events for Subscriptions
    /// </summary>
    /// <param name="channel">Platform Event channel Salesforce is publishing to.</param>
    /// <param name="message">Event payload containing the metadata for the Subscription event.</param>
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
            var subscriptionEvent = JsonConvert.DeserializeObject<MessageEnvelope<SalesforceSubscriptionEventPayload>>(eventContent);
            if (subscriptionEvent == null || subscriptionEvent.Data?.Event == null)
            {
                _logger.LogCritical($"[PLATFORM_EVENTS] Unable to deserialize message payload: Subscription not recognized.");
                return;
            }
            // assign replayId to redis cache to establish replay starting point in event of service failure
            _cacheRepo.SetSalesforceEventReplayId(_config["Salesforce:PlatformEvents:Channels:Subscription"], subscriptionEvent.Data.Event.ReplayId.ToString());
            // ensure we have a payload from the event message
            if (subscriptionEvent == null || subscriptionEvent.Data?.Payload == null)
            {
                _logger.LogCritical($"[PLATFORM_EVENTS] Subscription payload not found.");
                return;
            }

            var payload = subscriptionEvent.Data.Payload;
            _logger.LogInformation($"Message received ({payload.CreatedDate}) - Id: {payload.SubscriptionId}");

        }
        catch (Exception ex)
        {
            // TODO: log critical?
            _logger.LogCritical($"An unexpected error occurred while attempting to process Salesforce Platform Event message: {ex.Message}", message);
            throw;
        }
    }
}