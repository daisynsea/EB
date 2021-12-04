namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class CreateAddressModel : SalesforceActionObject
    {
        public string? ParentAccountOracleId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
    }
}
