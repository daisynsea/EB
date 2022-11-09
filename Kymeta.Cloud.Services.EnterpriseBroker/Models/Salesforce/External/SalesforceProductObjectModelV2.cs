using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

public class SalesforceProductObjectModelV2
{
    public string Id { get; set; }
    public string? SalesforceId { get; set; }
    public string? Name { get; set; }
    public int? WholesalePrice { get; set; }
    public int? MsrpPrice { get; set; }
    public int? DiscountTier2Price { get; set; }
    public int? DiscountTier3Price { get; set; }
    public int? DiscountTier4Price { get; set; }
    public int? DiscountTier5Price { get; set; }
    public bool? Mil { get; set; }
    public bool? Comm { get; set; }
    public string? Description { get; set; }
    public bool? Unavailable { get; set; } = false;
    public string? ProductType { get; set; }
    public string? ProductSubType { get; set; }
    public string? ProductFamily { get; set; }
    public int? Score { get; set; } = 0; // Connectivity Only
    //public string? Kit { get; set; } // Accessory Only
    public IEnumerable<string>? Assets { get; set; } // Terminal, Accessory Only
}
