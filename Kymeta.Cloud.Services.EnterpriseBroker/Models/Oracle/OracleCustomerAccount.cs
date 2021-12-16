namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle
{
    public class OracleCustomerAccount
    {
        // TODO: Fill this in
        public string? AccountName { get; set; }
        public string? PartyNumber { get; set; }
        public string? PartyId { get; set; }
        public string? OrigSystemReference { get; set; }
        public string? SalesforceId { get; set; }
        public string? OssId { get; set; }
        public string? TaxId { get; set; }
        public string? AccountType { get; set; }
        public string? AccountSubType { get; set; }
        public List<OracleCustomerAccountContact> Persons { get; set; }
        public List<OracleCustomerAccountSite> Locations { get; set; }
    }
}
