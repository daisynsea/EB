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
        private readonly IMessageListener _assetEventListener;

        public SalesforcePlatformEventsProcessingService(IConfiguration config, ILogger<SalesforcePlatformEventsProcessingService> logger, ISalesforceClient salesforceClient, IMessageListener assetEventListener)
        {
            _config = config;
            _logger = logger;
            _salesforceClient = salesforceClient;
            _assetEventListener = assetEventListener;
        }

        public async Task PlatformEventsListen(CancellationToken stoppingToken)
        {
            try
            {
                var authResult = await _salesforceClient.GetTokenAndUrl();
                if (authResult == null)
                {
                    var msg = $"Unable to authenticate with Salesforce.";
                    _logger.LogCritical(msg);
                    throw new Exception(msg);
                }
                // configure timeout
                int readTimeOut = 120000;
                Dictionary<string, object> options = new() {
                    { ClientTransport.TIMEOUT_OPTION, readTimeOut }
                };
                // declare auth
                NameValueCollection authCollection = new() {
                    { HttpRequestHeader.Authorization.ToString(), $"OAuth {authResult.Item1}" }
                };

                // define the transport
                var transport = new LongPollingTransport(options, new NameValueCollection { authCollection });
                var serverUri = new Uri(authResult.Item2);
                string endpoint = string.Format($"{serverUri.Scheme}://{serverUri.Host}/cometd/43.0");
                var bayeuxClient = new BayeuxClient(endpoint, new[] { transport });
                // add replay extension to be able to re-process potential missed events in case of service interruption
                bayeuxClient.AddExtension(new ReplayExtension());
                bayeuxClient.Handshake();
                bayeuxClient.WaitFor(1000, new List<BayeuxClient.State>() { BayeuxClient.State.CONNECTED });
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (!bayeuxClient.Connected)
                {
                    _logger.LogCritical($"Bayeux Client failed to connect to Streaming API.");
                    // TODO: throw exception?
                }

                _logger.LogInformation($"Activating listeners for Salesforce Platform Events.");

                // listen for meta connect messages to ascertain when/if the Streaming API needs a new auth token
                IClientSessionChannel metaEventChannel = bayeuxClient.GetChannel("/meta/connect", -1);
                // subscribe to meta events 
                metaEventChannel.Subscribe(new MetaEventListener());
                _logger.LogInformation($"Listening for events from Salesforce on the '{metaEventChannel}' channel...");

                // define the channel connection
                // replayId -1: (Default if no replay option is specified.) Subscriber receives new events that are broadcast after the client subscribes.
                // replayId -2: Subscriber receives all events, including past events that are within the retention window and new events.
                IClientSessionChannel assetEventChannel = bayeuxClient.GetChannel("/event/EB_Asset_Event__e", -1);
                // subscribe to events on the channel
                assetEventChannel.Subscribe(_assetEventListener);
                _logger.LogInformation($"Listening for events from Salesforce on the '{assetEventChannel}' channel...");


                //// prevent execution if background service is stopping
                //while (!stoppingToken.IsCancellationRequested)
                //{
                //    _logger.LogInformation($"[{DateTimeOffset.Now}] Synchronize Sales Orders is working.");
                    
                    
                //    // TODO: fetch latest auth token from Redis



                //    // fetch synchronize interval from config
                //    var synchronizeSalesOrdersInterval = _config["Intervals:SalesOrders"];
                //    // error if no config value is present
                //    if (string.IsNullOrEmpty(synchronizeSalesOrdersInterval)) throw new Exception("Missing 'Intervals:SalesOrders' configuration value.");
                //    // schedule the operation to run on the schedule the cron expression dictates (from Grapevine config)
                //    await WaitForNextSchedule(synchronizeSalesOrdersInterval, stoppingToken);
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task WaitForNextSchedule(string cronExpression, CancellationToken stoppingToken)
        {
            // parse the CRON expression
            var parsedExp = CronExpression.Parse(cronExpression);
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            // calculate the next occurence 
            var occurenceTime = parsedExp.GetNextOccurrence(currentUtcTime).GetValueOrDefault();
            // calculate the delay
            var delay = occurenceTime - currentUtcTime;
            _logger.LogInformation($"The Sales Order synchronization worker is delayed for {delay}. Current time: {DateTimeOffset.Now}");
            await Task.Delay(delay, stoppingToken);
        }
    }
}
