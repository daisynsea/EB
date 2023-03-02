using CometD.NetCore.Salesforce.Messaging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;

public class SalesforceSubscriptionEventPayload : MessagePayload
{
    [JsonProperty("Subscription_Id__c")]
    public string SubscriptionId { get; set; } = null!;
}
