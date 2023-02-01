using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners
{
    public class AssetEventListener : IMessageListener
    {
        private readonly ICacheRepository _cacheRepo;

        public AssetEventListener(ICacheRepository cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }

        /// <summary>
        /// Listen for messages from Salesforce Platform Events for Assets
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            // fetch the JSON
            var convertedJson = message.Json;
            // deserialize JSON into C# model
            var obj = JsonConvert.DeserializeObject<AssetEventRoot>(convertedJson);
            if (obj == null)
            {
                return;
            }
            _cacheRepo.SetSalesforceEventReplayId("AssetReplayId", obj.Data.Event.ReplayId.ToString());
            // write to console for demonstration purposes
            Console.WriteLine(convertedJson);
            Console.WriteLine($"Message received ({obj?.Data.Payload.CreatedDate}) - Name: {obj?.Data.Payload.Name}");
        }
    }
}
