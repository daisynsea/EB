namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class SalesforceProcessResponse
{
    public string? ObjectId { get; set; }
    public DateTime CompletedOn { get; set; }
    public StatusType OSSStatus { get; set; }
    public StatusType OracleStatus { get; set; }
    public string? OSSErrorMessage { get; set; }
    public string? OracleErrorMessage { get; set; }
}

