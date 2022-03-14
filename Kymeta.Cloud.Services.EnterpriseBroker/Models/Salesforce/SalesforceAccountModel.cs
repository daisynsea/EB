namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceAccountModel : SalesforceActionObject
    {
        public long? OraclePartyId { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? Pricebook { get; set; } // CPB, MPB or both
        public string? VolumeTier { get; set; } // Discount tier for configurator
        public string? SubType { get; set; } // Used in SOAP request
        public string? TaxId { get; set; } // Used in SOAP request
        public string? AccountType { get; set; } // Used in SOAP request
        public string? BusinessUnit { get; set; } // Used in SOAP request
        public string? OssId { get; set; }
        public string? OraclePartyId { get; set; } // used by Legacy objects
        public List<SalesforceAddressModel>? Addresses { get; set; }
        public List<SalesforceContactModel>? Contacts { get; set; }
    }
}
