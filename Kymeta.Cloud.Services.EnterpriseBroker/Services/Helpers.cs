using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public static class Helpers
{
    // fetch a value from a Dictionary based on the key
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
    {
        return dict.TryGetValue(key, out TV value) ? value : defaultValue;
    }

    public static List<OraclePartySiteUse> RemapAddressTypeToOracleSiteUse(SalesforceAddressModel address)
    {
        // if no Address, then return empty list
        if (address == null) return new List<OraclePartySiteUse>();

        // calculate SiteUseTypes (there can be multiple purposes (billing & shipping)
        var decodedType = OracleSoapTemplates.DecodeEncodedNonAsciiCharacters(address.Type);
        var siteUseTypes = new List<OraclePartySiteUse>();
        switch (decodedType.ToLower())
        {
            case "billing & shipping":
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.BILL_TO.ToString() });
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.SHIP_TO.ToString() });
                break;
            case "shipping":
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.SHIP_TO.ToString() });
                break;
            default:
                break;
        }
        return siteUseTypes;
    }
}