namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceContactModel : SalesforceActionObject
    {
        public long? OraclePartyId { get; set; }
        public string? ParentAccountId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public bool? IsPrimary { get; set; }
        public string? Role { get; set; }
        public string? Title { get; set; }
    }
}
