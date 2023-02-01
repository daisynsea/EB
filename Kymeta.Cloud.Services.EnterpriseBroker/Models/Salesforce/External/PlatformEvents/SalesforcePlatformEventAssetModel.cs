using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents
{
    public class AssetEventRoot
    {
        [JsonProperty("data")]
        public SalesforcePlatformEventData<AssetEventPayload> Data { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public class AssetEventPayload : PlatformEventPayload
    {
        [JsonProperty("Name__c")]
        public string Name { get; set; }
    }
}
