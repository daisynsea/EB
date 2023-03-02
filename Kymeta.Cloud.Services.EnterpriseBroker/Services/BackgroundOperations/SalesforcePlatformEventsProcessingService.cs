using CometD.NetCore.Client.Transport;
using CometD.NetCore.Client;
using CometD.NetCore.Bayeux.Client;
using System.Collections.Specialized;
using System.Net;
using Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners;
using CometD.NetCore.Client.Extension;
using CometD.NetCore.Salesforce;
using Cronos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    internal interface ISalesforcePlatformEventsProcessingService
    {
        Task PlatformEventsListen(CancellationToken stoppingToken);
    }

    public class SalesforcePlatformEventsProcessingService : ISalesforcePlatformEventsProcessingService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly ISalesforceClient _salesforceClient;
        private readonly IAssetEventListener _assetUpdateEventListener;
        private readonly ISubscriptionEventListener _subscriptionEventListener;
        private readonly ICacheRepository _cacheRepo;

        public SalesforcePlatformEventsProcessingService(IConfiguration config, ILogger<SalesforcePlatformEventsProcessingService> logger, ISalesforceClient salesforceClient, ICacheRepository cacheRepo, IAssetEventListener assetUpdateEventListener, ISubscriptionEventListener subscriptionEventListener)
        {
            _config = config;
            _logger = logger;
            _salesforceClient = salesforceClient;
            _cacheRepo = cacheRepo;
            _assetUpdateEventListener = assetUpdateEventListener;
            _subscriptionEventListener = subscriptionEventListener;
        }

        public async Task PlatformEventsListen(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"Authenticate with Salesforce to fetch access_token and instance_url.");
                // fetch authorization to establish connection to Platform Events
                var authResult = await _salesforceClient.GetTokenAndUrl();
                if (authResult == null)
                {
                    var msg = $"Unable to authenticate with Salesforce.";
                    _logger.LogCritical(msg);
                    throw new Exception(msg);
                }
                // configure options
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Dictionary<string, object> options = new() {{ ClientTransport.TIMEOUT_OPTION, 120000 } };
                NameValueCollection authCollection = new() {{ HttpRequestHeader.Authorization.ToString(), $"OAuth {authResult.Item1}" }};

                // define the transport
                var transport = new LongPollingTransport(options, new NameValueCollection { authCollection });
                var serverUri = new Uri(authResult.Item2);
                var streamingEndpoint = string.Format($"{serverUri.Scheme}://{serverUri.Host}/cometd/43.0");
                var bayeuxClient = new BayeuxClient(streamingEndpoint, new[] { transport });

                _logger.LogInformation($"Connecting Bayeux Client to Salesforce Platform Events CometD streaming endpoint.");
                // add replay extension to be able to re-process potential missed events in case of service interruption
                bayeuxClient.AddExtension(new ReplayExtension());
                bayeuxClient.Handshake();
                bayeuxClient.WaitFor(1000, new List<BayeuxClient.State>() { BayeuxClient.State.CONNECTED });

                // verify the connection is truly established
                if (!bayeuxClient.Connected)
                {
                    var msg = $"Bayeux Client failed to connect to Streaming API.";
                    _logger.LogCritical(msg);
                    throw new Exception(msg);
                }

                _logger.LogInformation($"Activating subscribers for Salesforce Platform Events.");

                #region Meta Connect Events
                // listen for meta connect messages in case of any errors published by Salesforce
                IClientSessionChannel metaEventChannel = bayeuxClient.GetChannel("/meta/connect", -1);
                metaEventChannel.Subscribe(new MetaEventListener());
                _logger.LogInformation($"Listening for events from Salesforce on the '{metaEventChannel}' channel...");
                #endregion

                #region Asset Events
                // fetch replay id from redis to fetch all messages from most recent message processed
                var assetUpdateReplayId = _cacheRepo.GetSalesforceEventReplayId(_config["Salesforce:PlatformEvents:Channels:Asset"]);

                // connect to the event channel and add the event listner
                IClientSessionChannel assetUpdateEventChannel = bayeuxClient.GetChannel($"/event/{_config["Salesforce:PlatformEvents:Channels:Asset"]}", assetUpdateReplayId);
                assetUpdateEventChannel.Subscribe(_assetUpdateEventListener);
                _logger.LogInformation($"Listening for events from Salesforce on the '{assetUpdateEventChannel}' channel...");
                #endregion

                #region Subscription Events
                // fetch replay id from redis to fetch all messages from most recent message processed
                var subscriptionEventReplayId = _cacheRepo.GetSalesforceEventReplayId(_config["Salesforce:PlatformEvents:Channels:Subscription"]);

                // connect to the event channel and add the event listner
                IClientSessionChannel subscriptionEventChannel = bayeuxClient.GetChannel($"/event/{_config["Salesforce:PlatformEvents:Channels:Subscription"]}", subscriptionEventReplayId);
                assetUpdateEventChannel.Subscribe(_subscriptionEventListener);
                _logger.LogInformation($"Listening for events from Salesforce on the '{subscriptionEventChannel}' channel...");
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
