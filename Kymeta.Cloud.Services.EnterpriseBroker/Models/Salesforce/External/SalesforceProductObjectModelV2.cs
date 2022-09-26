using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceProductObjectModelV2
    {
        public string? Id { get; set; }
        [JsonProperty("Name__c")]
        public string? Name { get; set; }
        [JsonProperty("ItemDetails__c")]
        public string? ItemDetails { get; set; }
        public string? Description { get; set; }
        [JsonProperty("Terminal_Category__c")]
        public string? TerminalCategory { get; set; }
        [JsonProperty("ProductType__c")]
        public string? ProductType { get; set; }
        public string? Family { get; set; }
        public bool? IsMilitary => TerminalCategory == "MIL";
    }
}
