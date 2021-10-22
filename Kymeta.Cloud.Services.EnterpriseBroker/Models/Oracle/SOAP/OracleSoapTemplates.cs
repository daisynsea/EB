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
        // TODO: verify XML and required fields via guidance from RSM or Oracle guru (OrigSystem, CreatedByModule, OriginalSystemReference, OwnerTableName, etc...)
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
    public static string CreateCustomerAccount(string organizationPartyId, string accountNumber, string accountName)
    {
        // validate the inputs
        if (string.IsNullOrEmpty(organizationPartyId))
        {
            throw new ArgumentException($"'{nameof(organizationPartyId)}' cannot be null or empty.", nameof(organizationPartyId));
        }
        if (string.IsNullOrEmpty(accountNumber))
        {
            throw new ArgumentException($"'{nameof(accountNumber)}' cannot be null or empty.", nameof(accountNumber));
        }

        // TODO: verify XML and required fields via guidance from RSM or Oracle guru (OrigSystem, CreatedByModule, OriginalSystemReference, OwnerTableName, etc...)
        // TODO: must identify template values to be populated from previous Oracle requests (OrgId etc...)
        var customerAccountEnvelope = 
            "<soap:Envelope xmlns: soap = \"http://schemas.xmlsoap.org/soap/envelope/\">" +
               "<soap:Body xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                  "<ns1:createCustomerAccount>" +
                     "<ns1:customerAccount xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\">" +
                        $"<ns2:PartyId>{organizationPartyId}</ns2:PartyId>" + // acquired from the create organization request (via REST)
                        $"<ns2:AccountNumber>{accountNumber}</ns2:AccountNumber>" + // 22092020
                        $"<ns2:AccountName>{accountName}</ns2:AccountName>" + // name for the Customer Account (can/should be the same as Organization name with "Acc" suffix on the end...?)
                        "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
        #region Extra metadata for a later story
                        //"<ns2:CustomerAccountContact>" +
                        //   "<ns2:RoleType>CONTACT</ns2:RoleType>" +
                        //   "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                        //   "<ns2:RelationshipId>300000090083090</ns2:RelationshipId>" +
                        //   "<ns2:OriginalSystemReference xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //      "<ns3:OrigSystem>ONE_TIME_PAYMENTS</ns3:OrigSystem>" +
                        //      "<ns3:OrigSystemReference>LEG1_CFS_CUST_ACCOUNT_ROLE</ns3:OrigSystemReference>" +
                        //      "<ns3:OwnerTableName>HZ_CUST_ACCOUNT_ROLES</ns3:OwnerTableName>" +
                        //      "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                        //   "</ns2:OriginalSystemReference>" +
                        //"</ns2:CustomerAccountContact>" +
                        //"<ns2:CustomerAccountSite>" +
                        //   "<ns2:PartySiteId>300000090083081</ns2:PartySiteId>" +
                        //   "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                        //   "<ns2:SetId>300000001127004</ns2:SetId>" +
                        //   "<ns2:CustomerAccountSiteUse>" +
                        //      "<ns2:SiteUseCode>BILL_TO</ns2:SiteUseCode>" +
                        //      "<ns2:Location>London (BILL TO)</ns2:Location>" +
                        //      "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                        //      "<ns2:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //         "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                        //         "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_BILL_TO</ns4:OrigSystemReference>" +
                        //         "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                        //         "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                        //      "</ns2:OriginalSystemReference>" +
                        //   "</ns2:CustomerAccountSiteUse>" +
                        //   "<ns2:CustomerAccountSiteUse>" +
                        //      "<ns2:SiteUseCode>SHIP_TO</ns2:SiteUseCode>" +
                        //      "<ns2:Location>London (SHIP TO)</ns2:Location>" +
                        //      "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                        //      "<ns2:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                        //         "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                        //         "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_SHIP_TO</ns4:OrigSystemReference>" +
                        //         "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                        //         "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                        //      "</ns2:OriginalSystemReference>" +
                        //   "</ns2:CustomerAccountSiteUse>" +
                        //"</ns2:CustomerAccountSite>" +
        #endregion
                        "<ns2:OriginalSystemReference xmlns:ns7=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                           "<ns7:OrigSystem>ONE_TIME_PAYMENTS</ns7:OrigSystem>" +
                           $"<ns7:OrigSystemReference>{Guid.NewGuid()}</ns7:OrigSystemReference>" + // TODO: do we need to store/save this value to our DB?
                           "<ns7:OwnerTableName>HZ_CUST_ACCOUNTS</ns7:OwnerTableName>" +
                           "<ns7:CreatedByModule>HZ_WS</ns7:CreatedByModule>" +
                        "</ns2:OriginalSystemReference>" +
                     "</ns1:customerAccount>" +
                  "</ns1:createCustomerAccount>" +
               "</soap:Body>" +
            "</soap:Envelope>";
        return customerAccountEnvelope;
    }
}
