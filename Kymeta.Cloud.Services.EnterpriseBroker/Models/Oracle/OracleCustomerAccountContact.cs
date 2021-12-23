using static Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.OracleSoapTemplates;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleCustomerAccountContact
{
    public ulong? ContactPersonId { get; set; }
    public string OrigSystemReference { get; set; }
    public string RelationshipId { get; set; }
    public AddressType ResponsibilityType { get; set; }
    public bool IsPrimary { get; set; }
}
