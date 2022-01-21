namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceAccountModel : SalesforceActionObject
    {
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? SubType { get; set; } // Used in SOAP request
        public string? TaxId { get; set; } // Used in SOAP request
        public string? AccountType { get; set; } // Used in SOAP request
        public string? OssId { get; set; }
        public List<SalesforceAddressModel>? Addresses { get; set; }
        public List<SalesforceContactModel>? Contacts { get; set; }
    }
}
