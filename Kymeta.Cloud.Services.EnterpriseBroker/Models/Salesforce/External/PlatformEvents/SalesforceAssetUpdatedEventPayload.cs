using CometD.NetCore.Salesforce.Messaging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents
{
    public class SalesforceAssetUpdatedEventPayload : MessagePayload
    {
        [JsonProperty("AssetID__c")]
        public string AssetId { get; set; } = null!;
        [JsonProperty("Asset_Name__c")]
        public string? Name { get; set; } = null!;
        [JsonProperty("Parent_Id__c")]
        public string? ParentId { get; set; } = null!;
        [JsonProperty("Prior_Parent_Id__c")]
        public string? PriorParentId { get; set; } = null!;
        [JsonProperty("Parent_Serial_Key__c")]
        public string? ParentSerial { get; set; } = null!;
        [JsonProperty("Account__c")]
        public string? AccountId { get; set; } = null!;
        [JsonProperty("Product__c")]
        public string? ProductId { get; set; } = null!;
        [JsonProperty("Product_Code__c")]
        public string? ProductCode { get; set; } = null!;
        [JsonProperty("ProductFamily__c")]
        public string? ProductFamily { get; set; } = null!;
        [JsonProperty("Serial_Number__c")]
        public string? Serial { get; set; } = null!;
        [JsonProperty("Prior_Serial_Number__c")]
        public string? PriorSerial { get; set; } = null!;
        [JsonProperty("End_Customer__c")]
        public string? EndCustomer { get; set; } = null!;
        [JsonProperty("Subscription_Id__c")]
        public string? SubscriptionId { get; set; } = null!;
        [JsonProperty("Parent_Product_Code__c")]
        public string? ParentProductCode { get; set; } = null!;
        [JsonProperty("Prior_Parent_Product_Code__c")]
        public string? PriorParentProductCode { get; set; } = null!;
    }
}
