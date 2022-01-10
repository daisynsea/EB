using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

public static class OracleSoapTemplates
{
    #region Location
    /// <summary>
    ///  A template for finding Locations in Oracle based on Enterprise Id.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindLocations(List<string> addressIds)
    {
        if (addressIds == null || addressIds.Count == 0) return null;

        var findLocationsEnvelope =
            $@"<soap:Envelope
	            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
	            <soap:Body>
		            <typ:getLocationByOriginalSystemReference>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>-1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>";
        foreach (var addressId in addressIds)
        {
            findLocationsEnvelope +=
                                    @$"<find:item>
							            <find:conjunction>Or</find:conjunction>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>OrigSystemReference</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{addressId}</find:value>
						            </find:item>";
        }

        findLocationsEnvelope +=
					            @$"</find:group>
				            </find:filter>
				            <find:findAttribute>LocationId</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>Address1</find:findAttribute>
				            <find:findAttribute>Address2</find:findAttribute>
				            <find:findAttribute>City</find:findAttribute>
				            <find:findAttribute>State</find:findAttribute>
				            <find:findAttribute>PostalCode</find:findAttribute>
				            <find:findAttribute>Country</find:findAttribute>
				            <find:excludeAttribute>false</find:excludeAttribute>
			            </typ:findCriteria>
		            </typ:getLocationByOriginalSystemReference>
	            </soap:Body>
            </soap:Envelope>";
        return findLocationsEnvelope;
    }

    /// <summary>
    ///  A template for creating a Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateLocation(OracleLocationModel location)
    {
        var locationEnvelope =
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
                    <typ:createLocation>
                        <typ:location xmlns:loc=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/"">
                            <loc:CreatedByModule>HZ_WS</loc:CreatedByModule>
                            <loc:OrigSystem>SFDC</loc:OrigSystem>
                            <loc:OrigSystemReference>{location.OrigSystemReference}</loc:OrigSystemReference>
                            <loc:Address1>{location.Address1}</loc:Address1>
                            <loc:Address2>{location.Address2}</loc:Address2>
                            <loc:City>{location.City}</loc:City>
                            <loc:PostalCode>{location.PostalCode}</loc:PostalCode>
                            <loc:Country>{location.Country}</loc:Country>
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
    public static string CreateOrganizationPartySites(ulong organizationPartyId, List<OraclePartySite> partySites)
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
                                "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>";
            if (ps.SiteUses != null)
            {
                foreach (var siteUse in ps.SiteUses)
                {
                    locationEnvelope +=
                                    "<ns3:PartySiteUse>" +
                                        $"<ns3:SiteUseType>{siteUse.SiteUseType}</ns3:SiteUseType>" +
                                        "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                                    "</ns3:PartySiteUse>";
                }
            }
            locationEnvelope +=
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
    ///  A template for finding Persons in Oracle based on Enterprise Id.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindPersons(List<string> contactIds)
    {
        if (contactIds == null || contactIds.Count == 0) return null;

        var findPersonsEnvelope =
            $@"<soap:Envelope
	            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/"">
	            <soap:Body>
		            <typ:findPerson>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>-1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>";
        foreach (var contactId in contactIds)
        {
            findPersonsEnvelope +=
                                    @$"<find:item>
							            <find:conjunction>Or</find:conjunction>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>OrigSystemReference</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{contactId}</find:value>
						            </find:item>";
        }

        findPersonsEnvelope +=
                                @$"</find:group>
				            </find:filter>
				            <find:findAttribute>PartyId</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>PartyName</find:findAttribute>
				            <find:findAttribute>PersonFirstName</find:findAttribute>
				            <find:findAttribute>PersonLastName</find:findAttribute>
				            <find:findAttribute>EmailAddress</find:findAttribute>
 				            <find:findAttribute>Relationship</find:findAttribute>
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>Relationship</find:childAttrName>
					            <find:findAttribute>RelationshipId</find:findAttribute>
					            <find:findAttribute>ObjectId</find:findAttribute>
					            <find:findAttribute>ObjectType</find:findAttribute>
				            </find:childFindCriteria>
				            <find:excludeAttribute>false</find:excludeAttribute>
			            </typ:findCriteria>
		            </typ:findPerson>
	            </soap:Body>
            </soap:Envelope>";
        return findPersonsEnvelope;
    }

    /// <summary>
    ///  A template for creating a Person object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreatePerson(OraclePersonObject person, ulong organizationPartyId)
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
                                        <rel:PrimaryFlag>{person.IsPrimary ?? false}</rel:PrimaryFlag>
                                        <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                    </rel:OrganizationContactRole>
                                </rel:OrganizationContact>";

        // verify we have Phone metadata
        if (!string.IsNullOrEmpty(person.PhoneNumber))
        {
            personEnvelope +=
                                $@"<rel:Phone>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:PhoneNumber>{person.PhoneNumber}</con:PhoneNumber>
                                    <con:PhoneLineType>MOBILE</con:PhoneLineType> 
                                </rel:Phone>";
        }

        personEnvelope +=
                                    $@"<rel:Email>
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

    /// <summary>
    ///  A template for updating a Person object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string UpdatePerson(OraclePersonObject person)
    {
        var currentDate = $"{DateTime.UtcNow:yyyy-MM-dd}";
        // TODO: may need ContactPointId for Phone and Email
        // <con:ContactPointId>300000099945549</con:ContactPointId>
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
		            <typ:updatePerson>
			            <typ:personParty>
				            <per:PartyId>{person.PartyId}</per:PartyId>
				            <per:PersonProfile>
					            <per:PersonFirstName>{person.FirstName}</per:PersonFirstName>
					            <per:PersonLastName>{person.LastName}</per:PersonLastName>
				            </per:PersonProfile>
				            <per:Phone>
					            <con:PhoneNumber>{person.PhoneNumber}</con:PhoneNumber>
				            </per:Phone>
				            <per:Email>
					            <con:EmailAddress>{person.EmailAddress}</con:EmailAddress>
				            </per:Email>
			            </typ:personParty>
		            </typ:updatePerson>
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
        var findCustomerAccountEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:findCustomerAccount>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>
						            <find:item>
							            <find:conjunction/>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>OrigSystemReference</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{enterpriseId}</find:value>
						            </find:item>
					            </find:group>
					            <find:nested/>
				            </find:filter>
				            <find:findAttribute>PartyId</find:findAttribute>
				            <find:findAttribute>CustomerAccountId</find:findAttribute>
				            <find:findAttribute>AccountNumber</find:findAttribute>
				            <find:findAttribute>AccountName</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>CustomerType</find:findAttribute>
				            <find:findAttribute>CustomerClassCode</find:findAttribute>
				            <find:findAttribute>CustomerAccountSite</find:findAttribute>
				            <find:findAttribute>CustomerAccountContact</find:findAttribute>
				            <find:excludeAttribute>false</find:excludeAttribute>
				            <!-- Customer Account Sites -->
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>CustomerAccountSite</find:childAttrName>
					            <find:findAttribute>OrigSystemReference</find:findAttribute>
					            <find:findAttribute>CustomerAccountSiteId</find:findAttribute>
					            <find:findAttribute>CustomerAccountId</find:findAttribute>
					            <find:findAttribute>PartySiteId</find:findAttribute>
					            <find:findAttribute>CustomerAccountSiteUse</find:findAttribute>
                                <!-- Original System Reference -->
					            <find:childFindCriteria>
						            <find:fetchStart>0</find:fetchStart>
						            <find:fetchSize>-1</find:fetchSize>
						            <find:childAttrName>CustomerAccountSiteUse</find:childAttrName>
						            <find:findAttribute>OriginalSystemReference</find:findAttribute>						
						            <find:childFindCriteria>
							            <find:fetchStart>0</find:fetchStart>
							            <find:fetchSize>-1</find:fetchSize>
							            <find:childAttrName>OriginalSystemReference</find:childAttrName>
							            <find:findAttribute>OrigSystemReference</find:findAttribute>
						            </find:childFindCriteria>
					            </find:childFindCriteria>
				            </find:childFindCriteria>
				            <!-- Customer Account Contacts -->
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>CustomerAccountContact</find:childAttrName>
					            <find:findAttribute>OrigSystemReference</find:findAttribute>
					            <find:findAttribute>CustomerAccountId</find:findAttribute>
					            <find:findAttribute>PrimaryFlag</find:findAttribute>
					            <find:findAttribute>RelationshipId</find:findAttribute>
					            <find:findAttribute>ContactPersonId</find:findAttribute>
				            </find:childFindCriteria>
			            </typ:findCriteria>
			            <typ:findControl>
				            <find:retrieveAllTranslations>false</find:retrieveAllTranslations>
			            </typ:findControl>
		            </typ:findCustomerAccount>
	            </soapenv:Body>
            </soapenv:Envelope>
            ";
        return findCustomerAccountEnvelope;
    }

    /// <summary>
    ///  A template for creating a Customer Account object in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating a Customer Account in Oracle</returns>
    public static string CreateCustomerAccount(OracleCustomerAccount model, ulong organizationPartyId, List<OracleCustomerAccountSite> customerSites, List<OracleCustomerAccountContact> contacts)
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
                        "</cus:CustAcctInformation>";

        // check for the existence of any Sites
        if (customerSites != null && customerSites.Count > 0)
        {
            // include each Site
            foreach (var site in customerSites)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountSite>" +
                            $"<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            "<cus:SetId>300000001127004</cus:SetId>" +
                            "<cus:OrigSystem>SFDC</cus:OrigSystem>" +
                            $"<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>" +
                            "<cus:CustomerAccountSiteUse>" +
                                "<cus:SiteUseCode>BILL_TO</cus:SiteUseCode>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            "</cus:CustomerAccountSiteUse>" +
                        "</cus:CustomerAccountSite>";
            }
        }

        // check for the existence of any Persons
        if (contacts != null && contacts.Count > 0)
        {
            // include each Person
            foreach (var contact in contacts)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountContact>" +
                            "<cus:RoleType>CONTACT</cus:RoleType>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            $"<cus:RelationshipId>{contact.RelationshipId}</cus:RelationshipId>" +
                            "<cus:OrigSystem>SFDC</cus:OrigSystem>" +
                            $"<cus:OrigSystemReference>{contact.OrigSystemReference}</cus:OrigSystemReference>";

                if (contact.ResponsibilityType != null)
                {
                    //customerAccountEnvelope +=
                            //"<cus:CustomerAccountContactRole>" +
                            //    $"<cus:ResponsibilityType>{contact.ResponsibilityType}</cus:ResponsibilityType>" +
                            //"</cus:CustomerAccountContactRole>";
                }
                customerAccountEnvelope +=
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
    public static string UpsertCustomerAccount(OracleCustomerAccount account, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts)
    {
        // validate the inputs
        if (account.CustomerAccountId == null)
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountId)}' cannot be null.", nameof(account.CustomerAccountId));
        }
        if (account.PartyId == null)
        {
            throw new ArgumentException($"'{nameof(account.PartyId)}' cannot be null.", nameof(account.PartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soapenv:Envelope " +
                "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                "xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\" " +
                "xmlns:cus1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/\" " +
                "xmlns:cus2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/\" " +
                "xmlns:cus3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountRel/\" " +
                "xmlns:cus4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/\" " +
                "xmlns:cus5=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/\" " +
                "xmlns:cus6=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                "xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\" " +
                "xmlns:sour=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/\" " +
                "xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                "<soapenv:Header />" +
                "<soapenv:Body>" +
                    "<typ:mergeCustomerAccount>" +
                        "<typ:customerAccount>" +
                            $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                            $"<cus:PartyId>{account.PartyId}</cus:PartyId>" +
                            $"<cus:CustomerType>{account.AccountType}</cus:CustomerType>" +
                            "<cus:CustAcctInformation>" +
                                $"<cus6:salesforceId>{account.SalesforceId}</cus6:salesforceId>" +
                                $"<cus6:ksnId>{account.OssId}</cus6:ksnId>" +
                            "</cus:CustAcctInformation>";

        // update Customer Account Contacts
        if (accountContacts != null && accountContacts.Count > 0)
        {
            foreach (var person in accountContacts)
            {
                if (person.RelationshipId != null)
                {
                    customerAccountEnvelope +=
                            "<cus:CustomerAccountContact>" +
                                $"<cus:PrimaryFlag>{person.IsPrimary}</cus:PrimaryFlag>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                $"<cus:RelationshipId>{person.RelationshipId}</cus:RelationshipId>" + // RelationshipId from the Person response
                                "<cus:RoleType>CONTACT</cus:RoleType>" +
                                //"<cus:CustomerAccountContactRole>" +
                                //    $"<cus:ResponsibilityType>{person.ResponsibilityType}</cus:ResponsibilityType>" +
                                //    $"<cus:PrimaryFlag>{person.IsPrimary}</cus:PrimaryFlag>" +
                                //"</cus:CustomerAccountContactRole>" +
                            "</cus:CustomerAccountContact>";
                }
            }
        }

        // update Customer Account Sites
        if (accountSites != null && accountSites.Count > 0)
        {
            foreach (var site in accountSites)
            {
                // TODO: invesitgate SetId and how it should flow to here... there are two options AFAIK (Kymeta, KGS)
                // TODO: see how critical it is to differentiate
                customerAccountEnvelope +=
                            $@"<cus:CustomerAccountSite>
								<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>
								<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>
								<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								<cus:SetId>300000001127004</cus:SetId>
								<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>";

                if (site.SiteUses != null)
                {
                    foreach (var siteUse in site.SiteUses)
                    {
                        customerAccountEnvelope +=
                                        @$"<cus:CustomerAccountSiteUse>
									        <cus:SiteUseCode>{siteUse.SiteUseCode}</cus:SiteUseCode>
									        <cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								        </cus:CustomerAccountSiteUse>";
                    }
                }
                customerAccountEnvelope +=
                            "</cus:CustomerAccountSite>";
            }
        }
        customerAccountEnvelope +=
                        "</typ:customerAccount>" +
                    "</typ:mergeCustomerAccount>" +
                "</soapenv:Body>" +
            "</soapenv:Envelope>";
        return customerAccountEnvelope;
    }

    /// <summary>
    ///  A template for adding a Contact to a Customer Account in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating updating a Customer Account in Oracle with a new Contact</returns>
    public static string UpdateCustomerAccountChildren(OracleCustomerAccount account, List<OracleCustomerAccountSite>? accountSites = null, List<OracleCustomerAccountContact>? accountContacts = null)
    {
        // validate the inputs
        if (account.CustomerAccountId == null)
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountId)}' cannot be null.", nameof(account.CustomerAccountId));
        }
        if (account.PartyId == null)
        {
            throw new ArgumentException($"'{nameof(account.PartyId)}' cannot be null.", nameof(account.PartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soapenv:Envelope " +
                "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                "xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\" " +
                "xmlns:cus1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/\" " +
                "xmlns:cus2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/\" " +
                "xmlns:cus3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountRel/\" " +
                "xmlns:cus4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/\" " +
                "xmlns:cus5=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/\" " +
                "xmlns:cus6=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                "xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\" " +
                "xmlns:sour=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/\" " +
                "xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                "<soapenv:Header />" +
                "<soapenv:Body>" +
                    "<typ:mergeCustomerAccount>" +
                        "<typ:customerAccount>" +
                            $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                            $"<cus:PartyId>{account.PartyId}</cus:PartyId>";

        // update Customer Account Contacts
        if (accountContacts != null && accountContacts.Count > 0)
        {
            foreach (var person in accountContacts)
            {
                if (person.RelationshipId != null)
                {
                    customerAccountEnvelope +=
                            "<cus:CustomerAccountContact>" +
                                $"<cus:PrimaryFlag>{person.IsPrimary}</cus:PrimaryFlag>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                $"<cus:RelationshipId>{person.RelationshipId}</cus:RelationshipId>" + // RelationshipId from the Person response
                                "<cus:RoleType>CONTACT</cus:RoleType>" +
                                //"<cus:CustomerAccountContactRole>" +
                                //    $"<cus:ResponsibilityType>{person.ResponsibilityType}</cus:ResponsibilityType>" +
                                //    $"<cus:PrimaryFlag>{person.IsPrimary}</cus:PrimaryFlag>" +
                                //"</cus:CustomerAccountContactRole>" +
                            "</cus:CustomerAccountContact>";
                }
            }
        }

        // update Customer Account Sites
        if (accountSites != null && accountSites.Count > 0)
        {
            foreach (var site in accountSites)
            {
                // TODO: invesitgate SetId and how it should flow to here... there are two options AFAIK (Kymeta, KGS)
                // TODO: see how critical it is to differentiate
                customerAccountEnvelope +=
                            $@"<cus:CustomerAccountSite>
								<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>
								<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>
								<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								<cus:SetId>300000001127004</cus:SetId>
								<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>";

                // account for multiple site uses (purposes)
                if (site.SiteUses != null)
                {
                    foreach (var siteUse in site.SiteUses)
                    {
                        customerAccountEnvelope +=
                                        @$"<cus:CustomerAccountSiteUse>
									        <cus:SiteUseCode>{siteUse.SiteUseCode}</cus:SiteUseCode>
									        <cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								        </cus:CustomerAccountSiteUse>";
                    }
                }
                customerAccountEnvelope +=
                            "</cus:CustomerAccountSite>";
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
    public static string CreateCustomerProfile(ulong? customerAccountPartyId, uint customerAccountNumber)
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

    public static readonly Dictionary<string, string> SiteUseTypes = new()
    {
        { "Billing & Shipping", "300000001127004" },
        { "Billing", "300000001127004" },
        { "Shipping", "300000001127004" }
    };

    public static readonly Dictionary<string, string> AddressSetIds = new()
    {
        { "kymeta", "300000001127004" },
        { "kgs", "300000001127004" }
    };

    // TODO: must create a full dictionary to match values from Salesforce
    public static readonly Dictionary<string, string> CountryShortcodes = new()
    {
        { "United States", "US" }
    };

    // acceptable value map for Account Type
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

    // TODO: must create a full dictionary to match values from Salesforce
    public static readonly Dictionary<string, string> ResponsibilityTypeMap = new()
    {
        { "Bill To Contact", "Bill to" },
        { "Ship To Contact", "Ship to" },
        { "Primary", "Primary" },
    };

    public static string DecodeEncodedNonAsciiCharacters(string value)
    {
        return Regex.Replace(
            value,
            @"\\u(?<Value>[a-zA-Z0-9]{4})",
            m => {
                return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
            });
    }
    #endregion
}
