using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

public static class OracleSoapTemplates
{
    #region Location
    /// <summary>
    ///  A template for creating a Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateLocation(SalesforceAddressModel address)
    {
        var locationEnvelope =
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
                    <typ:createLocation>
                        <typ:location xmlns:loc=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/"">
                            <loc:CreatedByModule>HZ_WS</loc:CreatedByModule>
                            <loc:OrigSystem>SFDC</loc:OrigSystem>
                            <loc:OrigSystemReference>{address.ObjectId}</loc:OrigSystemReference>
                            <loc:Address1>{address.Address1}</loc:Address1>
                            <loc:Address2>{address.Address2}</loc:Address2>
                            <loc:City>{address.City}</loc:City>
                            <loc:PostalCode>{address.PostalCode}</loc:PostalCode>
                            <loc:Country>{address.Country}</loc:Country>
                        </typ:location>
                    </typ:createLocation>
                </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }

    /// <summary>
    ///  A template for updating an existing Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string UpdateLocation(OracleLocationModel model)
    {
        var locationEnvelope =
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
	            <soap:Body xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
		            <typ:updateLocation>
			            <typ:location xmlns:loc=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/"">
				            <loc:LocationId>{model.LocationId}</loc:LocationId>
				            <loc:Address1>{model.Address1}</loc:Address1>
				            <loc:Address2>{model.Address2}</loc:Address2>
				            <loc:City>{model.City}</loc:City>
				            <loc:PostalCode>{model.PostalCode}</loc:PostalCode>
				            <loc:Country>{model.Country}</loc:Country>
			            </typ:location>
		            </typ:updateLocation>
	            </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }
    #endregion

    #region Organization & PartySite
    /// <summary>
    ///  A template for creating an Organization Party Site object in Oracle.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindOrganization(string organizationName, string originSystemReference)
    {
        var locationEnvelope = 
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                <typ:findOrganization xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/"">
                    <typ:findCriteria xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
                    <find:fetchStart>0</find:fetchStart>
                    <find:fetchSize>-1</find:fetchSize>
                    <find:filter>
                        <find:conjunction/>
                        <find:group>
                        <find:conjunction/>
                        <find:upperCaseCompare>false</find:upperCaseCompare>
                        <find:item>
                            <find:conjunction/>
                            <find:upperCaseCompare>false</find:upperCaseCompare>
                            <find:attribute>PartyName</find:attribute>
                            <find:operator>=</find:operator>
                            <find:value>{organizationName}</find:value>
                        </find:item>
                        </find:group>
                    </find:filter>
                    <find:findAttribute>PartyId</find:findAttribute>
                    <find:findAttribute>PartyName</find:findAttribute>
                    <find:findAttribute>PartyNumber</find:findAttribute>
                    <find:findAttribute>OriginalSystemReference</find:findAttribute>
				    <find:findAttribute>PartySite</find:findAttribute>
				    <find:findAttribute>Relationship</find:findAttribute>
                    <find:excludeAttribute>false</find:excludeAttribute>
                    <find:childFindCriteria>
                        <find:fetchStart>0</find:fetchStart>
                        <find:fetchSize>-1</find:fetchSize>
                        <find:filter>
                        <find:conjunction>And</find:conjunction>
                        <find:group>
                            <find:conjunction>And</find:conjunction>
                            <find:upperCaseCompare>false</find:upperCaseCompare>
                            <find:item>
                                <find:conjunction>And</find:conjunction>
                                <find:upperCaseCompare>false</find:upperCaseCompare>
                                <find:attribute>OrigSystemReference</find:attribute>
                                <find:operator>=</find:operator>
                                <find:value>{originSystemReference}</find:value>
                            </find:item>
                            <find:item>
                                <find:conjunction>And</find:conjunction>
                                <find:upperCaseCompare>false</find:upperCaseCompare>
                                <find:attribute>OrigSystem</find:attribute>
                                <find:operator>=</find:operator>
                                <find:value>SFDC</find:value>
                            </find:item>
                        </find:group>
                        </find:filter>
                        <find:excludeAttribute>false</find:excludeAttribute>
					    <find:findAttribute>OrigSystemReference</find:findAttribute>					
                        <find:childAttrName>OriginalSystemReference</find:childAttrName>
                    </find:childFindCriteria>
				    <find:childFindCriteria>
                        <find:fetchStart>0</find:fetchStart>
                        <find:fetchSize>-1</find:fetchSize>
                        <find:childAttrName>PartySite</find:childAttrName>
					    <find:findAttribute>PartySiteId</find:findAttribute>
					    <find:findAttribute>OrigSystemReference</find:findAttribute>
					    <find:findAttribute>LocationId</find:findAttribute>
                    </find:childFindCriteria>				
				     <find:childFindCriteria>
                        <find:fetchStart>0</find:fetchStart>
                        <find:fetchSize>-1</find:fetchSize>
                        <find:childAttrName>Relationship</find:childAttrName>					
					    <find:findAttribute>OrganizationContact</find:findAttribute>
					    <find:childFindCriteria>
						    <find:fetchStart>0</find:fetchStart>
						    <find:fetchSize>-1</find:fetchSize>
						    <find:childAttrName>OrganizationContact</find:childAttrName>
						    <find:findAttribute>ContactPartyId</find:findAttribute>
						    <find:findAttribute>PersonFirstName</find:findAttribute>
						    <find:findAttribute>PersonLastName</find:findAttribute>
						    <find:findAttribute>OrigSystemReference</find:findAttribute>	
					    </find:childFindCriteria>
                    </find:childFindCriteria>
				
                    </typ:findCriteria>
                    <typ:findControl xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
                    <find:retrieveAllTranslations>false</find:retrieveAllTranslations>
                    </typ:findControl>
                </typ:findOrganization>
                </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }

    /// <summary>
    ///  A template for creating an Organization Party Site object in Oracle.
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateOrganizationPartySites(long organizationPartyId, List<OraclePartySite> partySites)
    {
        var locationEnvelope =
            $"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<soap:Body>" +
                    "<ns1:mergeOrganization xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/\">" +
                        "<ns1:organizationParty xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/\">" +
                            $"<ns2:PartyId>{organizationPartyId}</ns2:PartyId>";
        // include all the PartySite additions
        foreach (var ps in partySites)
        {
            locationEnvelope +=
                            "<ns2:PartySite xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                                $"<ns3:LocationId>{ps.LocationId}</ns3:LocationId>" +
                                $"<ns3:OrigSystemReference>{ps.OrigSystemReference}</ns3:OrigSystemReference>" +
                                "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                                "<ns3:PartySiteUse>" +
                                    $"<ns3:SiteUseType>{ps.SiteUseType}</ns3:SiteUseType>" +
                                    "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                                "</ns3:PartySiteUse>" +
                            "</ns2:PartySite>";
        }

        locationEnvelope +=
                        "</ns1:organizationParty>" +
	                "</ns1:mergeOrganization>" +
                "</soap:Body>" +
                "</soap:Envelope>";
        return locationEnvelope;
    }
    #endregion

    #region Person
    /// <summary>
    ///  A template for creating a Person object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreatePerson(OraclePersonObject person, string organizationPartyId)
    {
        var currentDate = $"{DateTime.UtcNow:yyyy-MM-dd}";
        var personEnvelope =
            $@"<soapenv:Envelope
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
		        xmlns:con=""http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/""
  	            xmlns:con1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/contactPoint/""
                xmlns:org=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/orgContact/""
                xmlns:par=""http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/""
                xmlns:par1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/""
                xmlns:per=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/""
                xmlns:per1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/person/""
  	            xmlns:rel=""http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/""
		        xmlns:rel1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/relationship/""
 		        xmlns:sour=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/""
		        xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/"">
                <soapenv:Header />
                <soapenv:Body>
                    <typ:createPerson>
                        <typ:personParty>
                            <per:CreatedByModule>HZ_WS</per:CreatedByModule>
                            <per:SourceSystem>SFDC</per:SourceSystem>
                            <per:SourceSystemReferenceValue>{person.OrigSystemReference}</per:SourceSystemReferenceValue>
                            <per:PersonProfile>
                                <per:PersonFirstName>{person.FirstName}</per:PersonFirstName>
                                <per:PersonLastName>{person.LastName}</per:PersonLastName>
                                <per:CreatedByModule>HZ_WS</per:CreatedByModule>
                                <per:OrigSystemReference>{person.OrigSystemReference}</per:OrigSystemReference>
                            </per:PersonProfile>
                            <per:Relationship>
                                <rel:SubjectType>PERSON</rel:SubjectType>
                                <rel:SubjectTableName>HZ_PARTIES</rel:SubjectTableName>
                                <rel:ObjectId>{organizationPartyId}</rel:ObjectId>
                                <rel:ObjectType>ORGANIZATION</rel:ObjectType>
                                <rel:ObjectTableName>HZ_PARTIES</rel:ObjectTableName>
                                <rel:RelationshipCode>CONTACT_OF</rel:RelationshipCode>
                                <rel:RelationshipType>CONTACT</rel:RelationshipType>
                                <rel:StartDate>{currentDate}</rel:StartDate>
                                <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                <rel:OrganizationContact>
                                    <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                    <rel:OrganizationContactRole>
                                        <rel:RoleType>CONTACT</rel:RoleType>
                                        <rel:PrimaryFlag>{person.IsPrimary}</rel:PrimaryFlag>
                                        <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                    </rel:OrganizationContactRole>
                                </rel:OrganizationContact>
                                <rel:Phone>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
									<con:PhoneCountryCode>{person.PhoneCountryCode}</con:PhoneCountryCode>
                                    <con:PhoneAreaCode>{person.PhoneAreaCode}</con:PhoneAreaCode>
                                    <con:PhoneNumber>{person.PhoneNumber}</con:PhoneNumber>
                                    <!-- <con:PhoneLineType>MOBILE</con:PhoneLineType> -->
                                </rel:Phone>
                                <rel:Email>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:PrimaryFlag>true</con:PrimaryFlag>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:ContactPointPurpose>BUSINESS</con:ContactPointPurpose>
                                    <con:StartDate>{currentDate}</con:StartDate>
                                    <con:EmailAddress>{person.EmailAddress}</con:EmailAddress>
                                </rel:Email>
                            </per:Relationship>
                        </typ:personParty>
                    </typ:createPerson>
                </soapenv:Body>
            </soapenv:Envelope>";
        return personEnvelope;
    }
    #endregion

    #region Customer Account
    /// <summary>
    /// Find an Oracle Customer Account by searching with the origin system reference (enterprise Id)
    /// </summary>
    /// <param name="enterpriseId">The Id of the originating object from Salesforce</param>
    /// <returns>A SOAP envelope XML payload to send as a request body.</returns>
    public static string FindCustomerAccount(string enterpriseId)
    {
        var findEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/""
	            xmlns:typ1=""http://xmlns.oracle.com/adf/svc/types/"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:findCustomerAccount>
			            <typ:findCriteria>
				            <typ1:fetchStart>0</typ1:fetchStart>
				            <typ1:fetchSize>1</typ1:fetchSize>
				            <typ1:filter>
					            <typ1:conjunction/>
					            <typ1:group>
						            <typ1:conjunction/>
						            <typ1:upperCaseCompare>false</typ1:upperCaseCompare>
						            <typ1:item>
							            <typ1:conjunction/>
							            <typ1:upperCaseCompare>false</typ1:upperCaseCompare>
							            <typ1:attribute>OrigSystemReference</typ1:attribute>
							            <typ1:operator>=</typ1:operator>
							            <typ1:value>{enterpriseId}</typ1:value>
						            </typ1:item>
					            </typ1:group>
					            <typ1:nested/>
				            </typ1:filter>
			            </typ:findCriteria>
			            <typ:findControl>
				            <typ1:retrieveAllTranslations>false</typ1:retrieveAllTranslations>
			            </typ:findControl>
		            </typ:findCustomerAccount>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return findEnvelope;
    }


    /// <summary>
    ///  A template for creating a Customer Account object in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating a Customer Account in Oracle</returns>
    public static string CreateCustomerAccount(OracleCustomerAccount model, long organizationPartyId, List<OraclePartySite> partySites, List<OraclePersonObject> persons)
    {
        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                $"xmlns:info=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                $"xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                "<soap:Body xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                  "<typ:createCustomerAccount>" +
                     "<typ:customerAccount xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\">" +
                        $"<cus:PartyId>{organizationPartyId}</cus:PartyId>" + // acquired from the create Organization response (via REST)
                        $"<cus:AccountName>{model.AccountName} Acc</cus:AccountName>" + // description for the Customer Account
                        $"<cus:CustomerType>{model.AccountType}</cus:CustomerType>" +
                        $"<cus:CustomerClassCode>{model.AccountSubType}</cus:CustomerClassCode>" +
                        $"<cus:OrigSystemReference>{model.SalesforceId}</cus:OrigSystemReference>" +
                        "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        "<cus:CustAcctInformation>" +
                            $"<info:salesforceId>{model.SalesforceId}</info:salesforceId>" +
                            $"<info:ksnId>{model.OssId}</info:ksnId>" +
                        "</cus:CustAcctInformation>" +
                        "<cus:OriginalSystemReference xmlns:ns7=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                           "<ns7:OrigSystem>SFDC</ns7:OrigSystem>" +
                           $"<ns7:OrigSystemReference>{model.SalesforceId}</ns7:OrigSystemReference>" +
                           "<ns7:OwnerTableName>HZ_CUST_ACCOUNTS</ns7:OwnerTableName>" +
                           "<ns7:CreatedByModule>HZ_WS</ns7:CreatedByModule>" +
                        "</cus:OriginalSystemReference>";

        // check for the existence of any Sites
        if (partySites != null && partySites.Count > 0)
        {
            // include each Site
            foreach (var site in partySites)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountSite>" +
                            $"<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            "<cus:SetId>300000001127004</cus:SetId>" +
                            "<cus:CustomerAccountSiteUse>" +
                                "<cus:SiteUseCode>BILL_TO</cus:SiteUseCode>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                "<cus:OriginalSystemReference>" +
                                    "<par:OrigSystem>SFDC</par:OrigSystem>" +
                                    $"<par:OrigSystemReference>{site.OrigSystemReference}</par:OrigSystemReference>" +
                                    "<par:OwnerTableName>HZ_CUST_SITE_USES_ALL</par:OwnerTableName>" +
                                    "<par:CreatedByModule>HZ_WS</par:CreatedByModule>" +
                                "</cus:OriginalSystemReference>" +
                            "</cus:CustomerAccountSiteUse>" +
                        "</cus:CustomerAccountSite>";
            }
        }

        // check for the existence of any Persons
        if (persons != null && persons.Count > 0)
        {
            // include each Person
            foreach (var person in persons)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountContact>" +
                            "<cus:RoleType>CONTACT</cus:RoleType>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            $"<cus:RelationshipId>{person.RelationshipId}</cus:RelationshipId>" +
                            $"<cus:OrigSystemReference>{person.OrigSystemReference}</cus:OrigSystemReference>" +
                            "<cus:OriginalSystemReference>" +
                                "<par:OrigSystem>SFDC</par:OrigSystem>" +
                                $"<par:OrigSystemReference>{person.OrigSystemReference}</par:OrigSystemReference>" +
                                "<par:OwnerTableName>HZ_CUST_ACCOUNT_ROLES</par:OwnerTableName>" +
                                "<par:CreatedByModule>HZ_WS</par:CreatedByModule>" +
                            "</cus:OriginalSystemReference>" +
                        "</cus:CustomerAccountContact>";
            }
        }

        // close the envelope
        customerAccountEnvelope +=
                     "</typ:customerAccount>" +
                  "</typ:createCustomerAccount>" +
               "</soap:Body>" +
            "</soap:Envelope>";
        return customerAccountEnvelope;
    }

    /// <summary>
    ///  A template for adding a Contact to a Customer Account in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating updating a Customer Account in Oracle with a new Contact</returns>
    public static string UpsertCustomerAccount(UpdateOracleCustomerAccountViewModel account, OracleCustomerAccountContact contact)
    {
        // validate the inputs
        if (string.IsNullOrEmpty(account.CustomerAccountId))
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountId)}' cannot be null or empty.", nameof(account.CustomerAccountId));
        }
        if (string.IsNullOrEmpty(account.CustomerAccountPartyId))
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountPartyId)}' cannot be null or empty.", nameof(account.CustomerAccountPartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soapenv:Envelope" +
                "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                "xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\"" +
                "xmlns:cus1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/\"" +
                "xmlns:cus2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/\"" +
                "xmlns:cus3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountRel/\"" +
                "xmlns:cus4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/\"" +
                "xmlns:cus5=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/\"" +
                "xmlns:cus6=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\"" +
                "xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\"" +
                "xmlns:sour=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/\"" +
                "xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                "<soapenv:Header />" +
                "<soapenv:Body>" +
                    "<typ:mergeCustomerAccount>" +
                        "<typ:customerAccount>" +
                            "<cus:OrigSystem>SFDC</cus:OrigSystem>" +
                            $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                            $"<cus:PartyId>{account.CustomerAccountPartyId}</cus:PartyId>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>";
        if (contact != null)
        {
            if (!string.IsNullOrEmpty(contact.RelationshipId))
            {
                customerAccountEnvelope +=
                            "<cus:CustomerAccountContact>" +
                                $"<cus:PrimaryFlag>{contact.IsPrimary}</cus:PrimaryFlag>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                $"<cus:RelationshipId>{contact.RelationshipId}</cus:RelationshipId>" + // RelationshipId from the Person response
                                "<cus:RoleType>CONTACT</cus:RoleType>" +
                                "<cus:CustomerAccountContactRole>" +
                                    $"<cus:ResponsibilityType>{contact.ResponsibilityType}</cus:ResponsibilityType>" +
                                    $"<cus:PrimaryFlag>{contact.IsPrimary}</cus:PrimaryFlag>" +
                                "</cus:CustomerAccountContactRole>" +
                            "</cus:CustomerAccountContact>";
            }
        }
        customerAccountEnvelope +=
                        "</typ:customerAccount>" +
                    "</typ:mergeCustomerAccount>" +
                "</soapenv:Body>" +
            "</soapenv:Envelope>";
        return customerAccountEnvelope;
    }
    #endregion

    #region Customer Profile
    /// <summary>
    ///  A template for creating a Customer Account Profile object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateCustomerProfile(string customerAccountPartyId, uint customerAccountNumber)
    {
        var locationEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/""
	            xmlns:cus=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/""
	            xmlns:cus1=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/""
	            xmlns:cus2=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/""
	            xmlns:xsi=""xsi"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:createCustomerProfile>
			            <typ:customerProfile>
				            <cus:PartyId>{customerAccountPartyId}</cus:PartyId> <!-- You will get Customer PartyId in response of Customer creation -->
				            <cus:AccountNumber>{customerAccountNumber}</cus:AccountNumber>
				            <cus:ProfileClassName>DEFAULT</cus:ProfileClassName>
				            <cus:EffectiveStartDate>{DateTime.UtcNow:yyyy-MM-dd}</cus:EffectiveStartDate>
			            </typ:customerProfile>
		            </typ:createCustomerProfile>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return locationEnvelope;
    }

    public static string GetActiveCustomerProfile(string customerAccountNumber)
    {
        var customerProfileEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/""
	            xmlns:cus=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/""
	            xmlns:cus1=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/""
	            xmlns:cus2=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/""
	            xmlns:xsi=""xsi"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:getActiveCustomerProfile>
			            <typ:customerProfile>
				            <cus:AccountNumber>{customerAccountNumber}</cus:AccountNumber>
			            </typ:customerProfile>
		            </typ:getActiveCustomerProfile>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return customerProfileEnvelope;
    }
    #endregion

    #region Helpers
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AddressType
    {
        BILL_TO,
        SHIP_TO
    }

    public static readonly Dictionary<string, string> AddressSetIds = new()
    {
        { "kymeta", "300000001127004" },
        { "kgs", "300000001127004" }
    };

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
    #endregion
}
