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

    public static string RemapBusinessUnitToOracleSiteAddressSet(string businessUnit)
    {
        // if no BusinessUnit, then return
        if (string.IsNullOrEmpty(businessUnit)) return null;

        // decode any Ascii characters
        var decodedBusinessUnit = OracleSoapTemplates.DecodeEncodedNonAsciiCharacters(businessUnit);

        // calculate AddressSet from Business Unit
        var businessUnitLower = decodedBusinessUnit.ToLower();
        if (businessUnitLower.Contains("commercial;u.s. government"))
        {
            return OracleSoapTemplates.AddressSetIds.GetValue(OracleSoapTemplates.BusinessUnit.KYMETAKGS.ToString().ToLower());
        }
        else if (businessUnitLower.Contains("commercial"))
        {
            return OracleSoapTemplates.AddressSetIds.GetValue(OracleSoapTemplates.BusinessUnit.KYMETA.ToString().ToLower());
        }
        else if (businessUnitLower.Contains("u.s. government"))
        {
            return OracleSoapTemplates.AddressSetIds.GetValue(OracleSoapTemplates.BusinessUnit.KGS.ToString().ToLower());
        }
        else
        {
            // selection not recognized
            return null;
        }
    }
}