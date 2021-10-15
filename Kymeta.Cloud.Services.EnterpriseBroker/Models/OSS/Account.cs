using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class Account
{
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid? ModifiedById { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }

    public string SalesforceAccountId { get; set; }
    public string OracleAccountId { get; set; }
    public string PrimaryContactName { get; set; }
    public string PrimaryContactEmail { get; set; }
    public string PrimaryContactPhone { get; set; }
    public string CustomerRelationshipType { get; set; }
    // Comma delimited list of sub types
    public string CustomerRelationshipSubTypes { get; set; }
    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingAddressLine3 { get; set; }
    public string BillingAddressLine4 { get; set; }
    public string BillingCity { get; set; }
    public string BillingState { get; set; }
    public string BillingPostalCode { get; set; }
    public string BillingCountryCode { get; set; }
    public CreatedOriginEnum? Origin { get; set; }
    public bool? Enabled { get; set; }

    // Optional
    public string ParentName { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreatedOriginEnum
{
    SF,
    OSS
}