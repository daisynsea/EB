namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleOrganization
{
    public long PartyId { get; set; }
    public string PartyNumber { get; set; }
    public string OrganizationName { get; set; }
    public string Type { get; set; }
    public string OrigSystemReference { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
    public List<OraclePartySite> PartySites { get; set; }
}

public class OraclePartySite
{
    public ulong? PartySiteId { get; set; }
}
