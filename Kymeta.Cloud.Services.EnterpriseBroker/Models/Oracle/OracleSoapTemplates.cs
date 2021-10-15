using System.Text;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public static class OracleSoapTemplates
{
    /// <summary>
    ///  A template for creating a Customer Account object in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating a Customer Account in Oracle</returns>
    public static string CreateCustomerAccount()
    {
        // TODO: verify XML and required fields via guidance from RSM or Oracle guru (OrigSystem, CreatedByModule, OriginalSystemReference, OwnerTableName, etc...)
        // TODO: must identify template values to be populated from previous Oracle requests (OrgId etc...)
        var customerAccountEnvelope = "<soap:Envelope xmlns: soap = \"http://schemas.xmlsoap.org/soap/envelope/\">" +
           "<soap:Body xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
              "<ns1:createCustomerAccount>" +
                 "<ns1:customerAccount xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\">" +
                    "<ns2:PartyId>300000090083080</ns2:PartyId>" +
                    "<!--Organization_Party_id-->" +
                    "<ns2:AccountNumber>22092020</ns2:AccountNumber>" +
                    "<ns2:AccountName>Alpha Kymeta Customer Account</ns2:AccountName>" +
                    "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                    "<ns2:CustomerAccountContact>" +
                       "<ns2:RoleType>CONTACT</ns2:RoleType>" +
                       "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                       "<ns2:RelationshipId>300000090083090</ns2:RelationshipId>" +
                       "<ns2:OriginalSystemReference xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                          "<ns3:OrigSystem>ONE_TIME_PAYMENTS</ns3:OrigSystem>" +
                          "<ns3:OrigSystemReference>LEG1_CFS_CUST_ACCOUNT_ROLE</ns3:OrigSystemReference>" +
                          "<ns3:OwnerTableName>HZ_CUST_ACCOUNT_ROLES</ns3:OwnerTableName>" +
                          "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                       "</ns2:OriginalSystemReference>" +
                    "</ns2:CustomerAccountContact>" +
                    "<ns2:CustomerAccountSite>" +
                       "<ns2:PartySiteId>300000090083081</ns2:PartySiteId>" +
                       "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                       "<ns2:SetId>300000001127004</ns2:SetId>" +
                       "<ns2:CustomerAccountSiteUse>" +
                          "<ns2:SiteUseCode>BILL_TO</ns2:SiteUseCode>" +
                          "<ns2:Location>London (BILL TO)</ns2:Location>" +
                          "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                          "<ns2:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                             "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                             "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_BILL_TO</ns4:OrigSystemReference>" +
                             "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                             "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                          "</ns2:OriginalSystemReference>" +
                       "</ns2:CustomerAccountSiteUse>" +
                       "<ns2:CustomerAccountSiteUse>" +
                          "<ns2:SiteUseCode>SHIP_TO</ns2:SiteUseCode>" +
                          "<ns2:Location>London (SHIP TO)</ns2:Location>" +
                          "<ns2:CreatedByModule>HZ_WS</ns2:CreatedByModule>" +
                          "<ns2:OriginalSystemReference xmlns:ns4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                             "<ns4:OrigSystem>ONE_TIME_PAYMENTS</ns4:OrigSystem>" +
                             "<ns4:OrigSystemReference>LEG1_CFS_CUST_SITE_USE_SHIP_TO</ns4:OrigSystemReference>" +
                             "<ns4:OwnerTableName>HZ_CUST_SITE_USES_ALL</ns4:OwnerTableName>" +
                             "<ns4:CreatedByModule>HZ_WS</ns4:CreatedByModule>" +
                          "</ns2:OriginalSystemReference>" +
                       "</ns2:CustomerAccountSiteUse>" +
                    "</ns2:CustomerAccountSite>" +
                    "<ns2:OriginalSystemReference xmlns:ns7=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                       "<ns7:OrigSystem>ONE_TIME_PAYMENTS</ns7:OrigSystem>" +
                       "<ns7:OrigSystemReference>LEG1_CFS_CUST_ACCOUNT</ns7:OrigSystemReference>" +
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
