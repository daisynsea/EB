using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

public class SalesforceProductObjectModel
{
    public string Id { get; set; }
    public string? ProductCode { get; set; }
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
    [JsonConverter(typeof(StringEnumConverter))]
    public ProductType? ProductType { get; set; }
    public ProductSubtype? ProductFamily { get; set; }
    public int? Score { get; set; } // Connectivity Only
    public IEnumerable<string> Assets { get; set; } // Terminal, Accessory Only
    public IEnumerable<string> Kit { get; set; } // Accessory Only
}

public enum ProductType
{
    Connectivity,
    Terminal,
    Warranty,
    Odu,
    Accessory
}

public enum ProductSubtype
{
    accessory,
    fru,
    cable
}
