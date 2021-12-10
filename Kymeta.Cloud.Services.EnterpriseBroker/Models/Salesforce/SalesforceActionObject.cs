using System.ComponentModel.DataAnnotations;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceActionObject
{
    [Required]
    public ActionObjectType? ObjectType { get; set; }
    [Required]
    public ActionType? ActionType { get; set; }
    public string? UserName { get; set; }
    public string? SalesforceOriginUri { get; set; }
    [Required]
    public string ObjectId { get; set; }
    [Required]
    public Dictionary<string, object> ObjectValues { get; set; }
}
