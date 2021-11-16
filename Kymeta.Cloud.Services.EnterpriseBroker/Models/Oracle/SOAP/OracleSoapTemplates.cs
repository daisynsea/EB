using System.Text;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

public static class OracleSoapTemplates
{
    /// <summary>
    ///  A template for creating a Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateLocation()
    {
        // create the SOAP envelope with a beefy string
        // TODO: must identify template values to be populated from previous Oracle requests (OrgId etc...)
        var customerAccountEnvelope = 
            "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<soap:Body xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/\">" +
                    "<ns1:createLocation>" +
                        "<ns1:location xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/\">" +
                            "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                            "<ns2:OrigSystem>SFDC</ns2:OrigSystem>" +
                            "<ns2:OrigSystemReference>ALPHA_ORIGIN_1</ns2:OrigSystemReference>" +
                            "<ns2:Country>US</ns2:Country>" +
                            "<ns2:Address1>1337 Dune Street</ns2:Address1>" +
                            "<ns2:City>Redmond</ns2:City>" +
                            "<ns2:PostalCode>98052</ns2:PostalCode>" +
                        "</ns1:location>" +
                    "</ns1:createLocation>" +
                "</soap:Body>" +
            "</soap:Envelope>";
        return customerAccountEnvelope;
    }

    /// <summary>
    ///  A template for creating a Customer Account object in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating a Customer Account in Oracle</returns>
    public static string CreateCustomerAccount(CreateOracleCustomerAccountViewModel model)
    {
        // validate the inputs
        if (string.IsNullOrEmpty(model.PartyId))
        {
            throw new ArgumentException($"'{nameof(model.PartyId)}' cannot be null or empty.", nameof(model.PartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:info=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\">" +
                "<soap:Body xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                  "<typ:createCustomerAccount>" +
                     "<typ:customerAccount xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\">" +
                        $"<cus:PartyId>{model.PartyId}</cus:PartyId>" + // acquired from the create Organization response (via REST)
                        $"<cus:AccountName>{model.OrganizationName} Acc</cus:AccountName>" + // description for the Customer Account
                        $"<cus:CustomerType>{model.AccountType}</cus:CustomerType>" +
                        $"<cus:CustomerClassCode>{model.AccountSubType}</cus:CustomerClassCode>" +
                        "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        "<cus:CustAcctInformation>" +
                            $"<info:salesforceId>{model.SalesforceId}</info:salesforceId>" +
                            $"<info:ksnId>{model.OssId}</info:ksnId>" +
                        "</cus:CustAcctInformation>" +
        #region Extra metadata for a later story
                        //"<cus:CustomerAccountContact>" +
                        //   "<cus:RoleType>CONTACT</cus:RoleType>" +
                        //   "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        //   "<cus:RelationshipId>300000090083090</cus:RelationshipId>" +
                        //   "<cus:OriginalSystemReference xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //      "<ns3:OrigSystem>ONE_TIME_PAYMENTS</ns3:OrigSystem>" +
                        //      "<ns3:OrigSystemReference>LEG1_CFS_CUST_ACCOUNT_ROLE</ns3:OrigSystemReference>" +
                        //      "<ns3:OwnerTableName>HZ_CUST_ACCOUNT_ROLES</ns3:OwnerTableName>" +
                        //      "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                        //   "</cus:OriginalSystemReference>" +
                        //"</cus:CustomerAccountContact>" +
                        //"<cus:CustomerAccountSite>" +
                        //   "<cus:PartySiteId>300000090083081</cus:PartySiteId>" +
                        //   "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        //   "<cus:SetId>300000001127004</cus:SetId>" +
                        //   "<cus:CustomerAccountSiteUse>" +
                        //      "<cus:SiteUseCode>BILL_TO</cus:SiteUseCode>" +
                        //      "<cus:Location>London (BILL TO)</cus:Location>" +
                        //      "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        //      "<cus:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //         "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                        //         "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_BILL_TO</ns4:OrigSystemReference>" +
                        //         "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                        //         "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                        //      "</cus:OriginalSystemReference>" +
                        //   "</cus:CustomerAccountSiteUse>" +
                        //   "<cus:CustomerAccountSiteUse>" +
                        //      "<cus:SiteUseCode>SHIP_TO</cus:SiteUseCode>" +
                        //      "<cus:Location>London (SHIP TO)</cus:Location>" +
                        //      "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        //      "<cus:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //         "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                        //         "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_SHIP_TO</ns4:OrigSystemReference>" +
                        //         "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                        //         "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                        //      "</cus:OriginalSystemReference>" +
                        //   "</cus:CustomerAccountSiteUse>" +
                        //"</cus:CustomerAccountSite>" +
        #endregion
                        "<cus:OriginalSystemReference xmlns:ns7=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                           "<ns7:OrigSystem>SFDC</ns7:OrigSystem>" +
                           $"<ns7:OrigSystemReference>{model.SalesforceId}</ns7:OrigSystemReference>" +
                           "<ns7:OwnerTableName>HZ_CUST_ACCOUNTS</ns7:OwnerTableName>" +
                           "<ns7:CreatedByModule>HZ_WS</ns7:CreatedByModule>" +
                        "</cus:OriginalSystemReference>" +
                     "</typ:customerAccount>" +
                  "</typ:createCustomerAccount>" +
               "</soap:Body>" +
            "</soap:Envelope>";
        return customerAccountEnvelope;
    }


    public static readonly Dictionary<string, string> CustomerTypeMap = new()
    {
        { "Consultant", "CONSULTANT" },
        { "End Customer", "ENDCUSTOMER" },
        { "External", "R" },
        { "Internal", "I" },
        { "Internal Dept", "INTERNALDEPT" },
        { "Partner", "PARTNER" },
        { "Partner Affiliate", "PARTNERAFFILIATE" },
        { "Prospect", "PROSPECT" },
        { "Regulatory Organization", "REGULATORY" },
        { "SubDistributor", "SUBDISTRIBUTOR" },
        { "Supplier", "SUPPLIER" },
        { "Other", "OTHER" }
    };

    public static readonly Dictionary<string, string> CustomerClassMap = new()
    {
        { "End User", "END USER" },
        { "VAR/Distributor", "VAR/ DISTRIBUTOR" },
        { "Integrator", "INTEGRATOR" },
        { "Eqpmt Mfr", "EQPMT MFR" },
        { "Component Mfr", "COMPONENT MFR" },
        { "OEM", "OEM" },
        { "SSO", "SSO" },
        { "SSP", "SSP" },
        { "Technology Partner", "TECHNOLOGY PARTNER" },
        { "Technology Provider", "TECHNOLOGY PROVIDER" },
        { "Consulting Svcs", "CONSULTING SVCS" },
        { "Financial Svcs", "FINANCIAL SVCS" },
        { "Legal Svcs", "LEGAL SVCS" },
        { "Other", "OTHER" }
    };
}
