namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
public class OraclePersonObject
{
    public ulong? PartyId { get; set; }
    public ulong? RelationshipId { get; set; }
    public string OrigSystemReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneCountryCode { get; set; }
    public string PhoneAreaCode { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public bool IsPrimary { get; set; }

}
