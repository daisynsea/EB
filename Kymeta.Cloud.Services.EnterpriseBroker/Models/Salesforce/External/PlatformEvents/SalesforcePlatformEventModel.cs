using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents
{

    public class SalesforcePlatformEventData<T>
    {
        [JsonProperty("schema")]
        public string Schema { get; set; }
        [JsonProperty("event")]
        public PlatformEvent Event { get; set; }
        [JsonProperty("payload")]
        public T Payload { get; set; }
    }

    public class PlatformEvent
    {
        [JsonProperty("replayId")]
        public int ReplayId { get; set; }
    }
}
