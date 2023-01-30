using CometD.NetCore.Client.Transport;
using CometD.NetCore.Client;
using CometD.NetCore.Bayeux.Client;
using System.Collections.Specialized;
using System.Net;
using Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners;
using CometD.NetCore.Client.Extension;

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

        public SalesforcePlatformEventsProcessingService(IConfiguration config, ILogger<SalesforcePlatformEventsProcessingService> logger, ISalesforceClient salesforceClient)
        {
            _config = config;
            _logger = logger;
            _salesforceClient = salesforceClient;
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
                bayeuxClient.AddExtension(new ReplayExtension());
                bayeuxClient.Handshake();
                bayeuxClient.WaitFor(1000, new List<BayeuxClient.State>() { BayeuxClient.State.CONNECTED });

                _logger.LogInformation($"Activating listeners for Salesforce Platform Events.");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Subscription to EB_Contact_Event__e
                IClientSessionChannel contactEventChannel = bayeuxClient.GetChannel("/event/EB_Contact_Event__e");
                contactEventChannel.Subscribe(new ContactEventListener());
                _logger.LogInformation($"Listening for events from Salesforce on the '{contactEventChannel}' channel...");

                //// Subscription to Kymeta_Event__e
                //IClientSessionChannel kymetaEventChannel = bayeuxClient.GetChannel("/event/Kymeta_Event__e");
                //kymetaEventChannel.Subscribe(new Listener());
                //Console.WriteLine($"Listening for events from Salesforce on the '{kymetaEventChannel}' channel...");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
