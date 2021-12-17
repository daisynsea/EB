namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle
{
    public class OracleCustomerAccount
    {
        // TODO: Fill this in
        public string? PartyNumber { get; set; }
        public string? PartyId { get; set; }
        public List<OracleCustomerAccountContact> Persons { get; set; }
        public List<OracleCustomerAccountSite> Locations { get; set; }
    }
}
