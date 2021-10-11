namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class Account
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? SalesforceAccountId { get; set; }
    public string? OracleAccountId { get; set; }
}