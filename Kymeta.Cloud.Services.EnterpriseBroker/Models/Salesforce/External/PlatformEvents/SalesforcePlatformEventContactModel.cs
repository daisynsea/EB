using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents
{
    public class ContactEventRoot
    {
        [JsonProperty("data")]
        public SalesforcePlatformEventData<ContactEventPayload> Data { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public class ContactEventPayload
    {
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string KCS_Contact_Id__c { get; set; }
        public string KCS_Contact_FirstName__c { get; set; }
        public string KCS_Contact_LastName__c { get; set; }
        public string KCS_Contact_Email__c { get; set; }
        public string KCS_Contact_Account_Name__c { get; set; }
    }
}
