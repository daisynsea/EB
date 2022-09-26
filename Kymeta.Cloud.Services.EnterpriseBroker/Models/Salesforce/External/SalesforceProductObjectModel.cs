using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

// TODO: Actually convert this to a real response from SF
// Then pass it to Partner API and convert it into this model there
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
    public ProductType ProductType { get; set; }
    public ProductSubtype ProductSubtype { get; set; }
    public int? Score { get; set; } // Connectivity Only
    public IEnumerable<string> Assets { get; set; } // Terminal, Accessory Only
    public IEnumerable<string> Kit { get; set; } // Accessory Only
}

public enum ProductType
{
    connectivity,
    terminal,
    warranty,
    odu,
    accessory
}

public enum ProductSubtype
{
    accessory,
    component,
    cable
}
