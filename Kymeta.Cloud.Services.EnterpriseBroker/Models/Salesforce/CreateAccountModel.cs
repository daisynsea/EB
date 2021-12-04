namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class CreateAccountModel : SalesforceActionObject
    {
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? SubType { get; set; } // Used in SOAP request
        public string? TaxId { get; set; } // Used in SOAP request
        public string? AccountType { get; set; } // Used in SOAP request
        public List<CreateAddressModel>? Addresses { get; set; }
        public List<CreateContactModel>? Contacts { get; set; }
    }
}
