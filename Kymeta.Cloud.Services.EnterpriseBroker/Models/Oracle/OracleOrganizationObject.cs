namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleOrganization
{
    public long PartyId { get; set; }
    public string PartyNumber { get; set; }
    public string OrganizationName { get; set; }
    public string Type { get; set; }
    public string SourceSystem { get; set; }
    public string SourceSystemReferenceValue { get; set; }
    public string OrigSystemReference { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
    public List<OraclePartySite>? PartySites { get; set; }
    public List<OracleOrganizationContact>? Contacts { get; set; }
}

public class OraclePartySite
{
    public ulong? LocationId { get; set; }
    public ulong? PartySiteId { get; set; }
    public string OrigSystemReference { get; set; }
    public List<OraclePartySiteUse>? SiteUses { get; set; }
}

public class OraclePartySiteUse
{
    public ulong? PartySiteUseId { get; set; }
    public string? SiteUseType { get; set; }
}

public class OracleOrganizationContact
{
    public string OrigSystemReference { get; set; }
    public ulong ContactPartyId { get; set; }
    public string PersonFirstName { get; set; }
    public string PersonLastName { get; set; }
}
