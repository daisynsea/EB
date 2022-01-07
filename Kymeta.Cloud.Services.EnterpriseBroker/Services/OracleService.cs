using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    // Account Endpoints
    Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<OracleOrganization, string>> UpdateOrganization(OracleOrganization existingOracleOrganization, SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(ulong organizationPartyId, List<OraclePartySite> partySites, SalesforceActionTransaction transaction);
    Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(ulong organizationPartyId, SalesforceAccountModel model, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts, SalesforceActionTransaction transaction);
    Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(ulong? customerAccountId, uint customerAccountNumber, SalesforceActionTransaction transaction);
    Task<Tuple<OracleCustomerAccount, string>> UpdateCustomerAccount(OracleCustomerAccount existingCustomerAccount, SalesforceAccountModel model, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts, SalesforceActionTransaction transaction);
    Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId, SalesforceActionTransaction transaction);
    Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId);
    Task<Tuple<bool, OracleCustomerAccountProfile, string>> GetCustomerProfileByAccountNumber(string customerAccountNumber, SalesforceActionTransaction transaction);
    // Address Endpoints
    Task<Tuple<bool, IEnumerable<OracleLocationModel>, string>> GetLocationsBySalesforceAddressId(List<string> addressIds);
    Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction);
    // Contact Endpoints
    Task<Tuple<bool, IEnumerable<OraclePersonObject>, string>> GetPersonsBySalesforceContactId(List<string> contactIds);
    Task<Tuple<OraclePersonObject, string>> CreatePerson(SalesforceContactModel model, string organizationPartyId, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdatePerson(SalesforceContactModel model, SalesforceContactModel transaction);
}

public class OracleService : IOracleService
{
    private readonly IOracleClient _oracleClient;
    private readonly IConfiguration _config;
    private readonly IActionsRepository _actionsRepository;

    public OracleService(IOracleClient oracleClient, IConfiguration config, IActionsRepository actionsRepository)
    {
        _oracleClient = oracleClient;
        _config = config;
        _actionsRepository = actionsRepository;
    }

    #region Account / Organization / Customer Account / Customer Profile
    #region Organization
    public async Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.GetOrganizationInOracleBySFID, ActionObjectType.Account, StatusType.Started);

        // populate the template
        var findOrganizationEnvelope = OracleSoapTemplates.FindOrganization(organizationName, salesforceAccountId);

        // Find the Organization via SOAP service
        var findOrgResponse = await _oracleClient.SendSoapRequest(findOrganizationEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Organization"]}");
        if (!string.IsNullOrEmpty(findOrgResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.GetOrganizationInOracleBySFID, ActionObjectType.Account, StatusType.Error, salesforceAccountId, findOrgResponse.Item2);
            return new Tuple<bool, OracleOrganization, string>(false, null, $"There was an error finding the Organization in Oracle: {findOrgResponse.Item2}.");
        }

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindOrganizationEnvelope));
        var oracleCustomerAccount = (FindOrganizationEnvelope)serializer.Deserialize(findOrgResponse.Item1.CreateReader());

        var oracleResult = oracleCustomerAccount.Body?.findOrganizationResponse?.result?.Value;
        if (oracleResult == null || oracleResult?.OriginalSystemReference?.OrigSystemReference == null)
        {
            await LogAction(transaction, SalesforceTransactionAction.GetOrganizationInOracleBySFID, ActionObjectType.Account, StatusType.Successful);
            return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");
        }

        // map the response model into our simplified C# model
        var organization = new OracleOrganization
        {
            OrganizationName = oracleResult.PartyName,
            PartyId = oracleResult.PartyId,
            PartyNumber = oracleResult.PartyNumber.ToString(),
            OrigSystemReference = oracleResult.OriginalSystemReference?.OrigSystemReference,
            // map the response object to our C# model for party sites
            PartySites = oracleResult.PartySite?
                .Select(ps => new OraclePartySite
                {
                    PartySiteId = ps.PartySiteId,
                    OrigSystemReference = ps.OrigSystemReference,
                    LocationId = ps.LocationId
                }).ToList(),
            Contacts = oracleResult.Relationship?
                .Select(r => new OracleOrganizationContact
                {
                    ContactPartyId = r.OrganizationContact.ContactPartyId,
                    OrigSystemReference = r.OrganizationContact.OrigSystemReference,
                    PersonFirstName = r.OrganizationContact.PersonFirstName,
                    PersonLastName = r.OrganizationContact.PersonLastName
                }).ToList()
        };

        await LogAction(transaction, SalesforceTransactionAction.GetOrganizationInOracleBySFID, ActionObjectType.Account, StatusType.Successful);

        // return the Organization that was found
        return new Tuple<bool, OracleOrganization, string>(true, organization, null);
    }

    public async Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Started);

        // map model to simplified object
        var orgToCreate = RemapSalesforceAccountToCreateOracleOrganization(model);
        
        // create the organization in Oracle
        var organizationResult = await _oracleClient.CreateOrganization(orgToCreate);
        
        // validate the the org was created
        if (!string.IsNullOrEmpty(organizationResult.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Error, model.ObjectId, organizationResult.Item2);
            return new Tuple<OracleOrganization, string>(null, $"There was an error adding the account to Oracle: {organizationResult.Item2}");
        }

        await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Successful, organizationResult.Item1.PartyId.ToString());

        // return the Oracle Organization
        return new Tuple<OracleOrganization, string>(organizationResult.Item1, String.Empty);
    }

    public async Task<Tuple<OracleOrganization, string>> UpdateOrganization(OracleOrganization existingOracleOrganization, SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        var organization = RemapSalesforceAccountToUpdateOracleOrganization(model, existingOracleOrganization);
        if (string.IsNullOrEmpty(existingOracleOrganization.PartyNumber)) return new Tuple<OracleOrganization, string>(null, $"oracleCustomerAccountId must be present to update the Oracle Account record.");
        var updated = await _oracleClient.UpdateOrganization(organization, existingOracleOrganization.PartyNumber);
        if (!string.IsNullOrEmpty(updated.Item2)) return new Tuple<OracleOrganization, string>(null, $"There was an error updating the account in Oracle: {updated.Item2}");

        // map response model to our simplified OracleOrganization
        var updatedOrganization = new OracleOrganization
        {
            OrganizationName = updated.Item1.OrganizationName,
            PartyId = updated.Item1.PartyId,
            PartyNumber = updated.Item1.PartyNumber,
            TaxpayerIdentificationNumber = updated.Item1.TaxpayerIdentificationNumber,
            OrigSystemReference = updated.Item1.SourceSystemReferenceValue,
            Type = updated.Item1.Type,
            SourceSystem = updated.Item1.SourceSystem,
            SourceSystemReferenceValue = updated.Item1.SourceSystemReferenceValue,

            // retain existing PartySites and Contacts
            PartySites = existingOracleOrganization.PartySites,
            Contacts = existingOracleOrganization.Contacts,
        };

        return new Tuple<OracleOrganization, string>(updatedOrganization, string.Empty);
    }
    #endregion

    #region Customer Account
    public async Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId)
    {
        var findAccountEnvelope = OracleSoapTemplates.FindCustomerAccount(salesforceAccountId);
        // Find the Organization via SOAP service
        var findAccountResponse = await _oracleClient.SendSoapRequest(findAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(findAccountResponse.Item2)) return new Tuple<bool, OracleCustomerAccount, string>(false, null, $"There was an error finding the Customer Account in Oracle: {findAccountResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindCustomerAccountEnvelope));
        var oracleCustomerAccount = (FindCustomerAccountEnvelope)serializer.Deserialize(findAccountResponse.Item1.CreateReader());
        
        // verify we have a found object
        var oracleResult = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value;
        if (oracleResult == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        if (oracleResult?.OrigSystemReference == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");

        // map the response model into our simplified C# model
        var account = new OracleCustomerAccount
        {
            PartyId = oracleResult?.PartyId,
            AccountName = oracleResult?.AccountName?.ToString(),
            AccountNumber = oracleResult?.AccountNumber,
            OrigSystemReference = oracleResult?.OrigSystemReference?.ToString(),
            AccountType = oracleResult?.CustomerType?.ToString(),
            AccountSubType = oracleResult?.CustomerClassCode?.ToString(),
            CustomerAccountId = oracleResult?.CustomerAccountId,
            SalesforceId = oracleResult?.CustAcctInformation?.salesforceId,
            OssId = oracleResult?.CustAcctInformation?.ksnId,
            Contacts = oracleResult?.CustomerAccountContacts?
                .Select(cac => new OracleCustomerAccountContact
                {
                    ContactPersonId = cac.ContactPersonId,
                    OrigSystemReference = cac.OrigSystemReference,
                    IsPrimary = cac.PrimaryFlag,
                    RelationshipId = cac.RelationshipId
                }).ToList(),
            Sites = oracleResult?.CustomerAccountSites?
                .Select(cas => new OracleCustomerAccountSite
                {
                    PartySiteId = cas.PartySiteId,
                    OrigSystemReference = cas.OrigSystemReference
                }).ToList(),
        };

        // return the Customer Account that was found
        return new Tuple<bool, OracleCustomerAccount, string>(true, account, null);
    }

    public async Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(ulong organizationPartyId, SalesforceAccountModel model, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Started, model.ObjectId);

        // map the model values to the expected Customer Account payload
        var customerAccount = RemapSalesforceAccountToOracleCustomerAccount(model);

        // populate the template
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(customerAccount, organizationPartyId, accountSites, accountContacts);

        // create the Customer Account via SOAP service
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Error, model.ObjectId, customerAccountResponse.Item2);
            return new Tuple<OracleCustomerAccount, string>(null, $"There was an error creating the Customer Account in Oracle: {customerAccountResponse.Item2}.");
        }

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerAccountResponseEnvelope));
        var oracleCustomerAccount = (CreateCustomerAccountResponseEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        // map Oracle response to our simplified object
        var oracleResult = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value;
        var customerAccountResult = new OracleCustomerAccount
        {
            PartyId = oracleResult?.PartyId,
            CustomerAccountId = oracleResult?.CustomerAccountId,
            AccountNumber = oracleResult?.AccountNumber,
            AccountName = oracleResult?.AccountName?.ToString(),
            AccountType = oracleResult?.CustomerType?.ToString(),
            AccountSubType = oracleResult?.CustomerClassCode?.ToString(),
            OrigSystemReference = oracleResult?.OrigSystemReference.ToString(),
            SalesforceId = oracleResult?.CustAcctInformation.salesforceId,
            OssId = oracleResult?.CustAcctInformation.ksnId
        };

        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Successful, customerAccountResult.PartyId.ToString());

        // return the Customer Account
        return new Tuple<OracleCustomerAccount, string>(customerAccountResult, string.Empty);
    }

    public async Task<Tuple<OracleCustomerAccount, string>> UpdateCustomerAccount(OracleCustomerAccount existingCustomerAccount, SalesforceAccountModel model, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Started, model.ObjectId);

        // map the model values to the expected Customer Account payload
        var customerAccount = RemapSalesforceAccountToOracleCustomerAccount(model, existingCustomerAccount);

        // populate the template
        var customerAccountEnvelope = OracleSoapTemplates.UpsertCustomerAccount(customerAccount, accountSites, accountContacts);

        // update the Customer Account via SOAP service
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.UpdateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Error, model.ObjectId, customerAccountResponse.Item2);
            return new Tuple<OracleCustomerAccount, string>(null, $"There was an error updating the Customer Account in Oracle: {customerAccountResponse.Item2}.");
        }

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(UpdateCustomerAccountEnvelope));
        var oracleCustomerAccount = (UpdateCustomerAccountEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        // map Oracle response to our simplified object
        var updateResponse = oracleCustomerAccount?.Body?.mergeCustomerAccountResponse?.result?.Value;
        var customerAccountResult = new OracleCustomerAccount
        {
            PartyId = updateResponse?.PartyId,
            CustomerAccountId = updateResponse?.CustomerAccountId,
            AccountNumber = updateResponse?.AccountNumber,
            AccountName = updateResponse?.AccountName?.ToString(),
            AccountType = updateResponse?.CustomerType?.ToString(),
            AccountSubType = updateResponse?.CustomerClassCode?.ToString(),
            OrigSystemReference = updateResponse?.OrigSystemReference,
            SalesforceId = updateResponse?.CustAcctInformation.salesforceId,
            OssId = updateResponse?.CustAcctInformation.ksnId,
        };

        await LogAction(transaction, SalesforceTransactionAction.UpdateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Successful, customerAccountResult.PartyId.ToString());

        // return the Customer Account
        return new Tuple<OracleCustomerAccount, string>(customerAccountResult, string.Empty);
    }
    #endregion

    #region Customer Account Profile
    public async Task<Tuple<bool, OracleCustomerAccountProfile, string>> GetCustomerProfileByAccountNumber(string customerAccountNumber, SalesforceActionTransaction transaction)
    {
        // populate the template
        var findCustomerProfileEnvelope = OracleSoapTemplates.GetActiveCustomerProfile(customerAccountNumber);
        // create the Customer Account via SOAP service
        var customerProfileResponse = await _oracleClient.SendSoapRequest(findCustomerProfileEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerProfile"]}");
        if (!string.IsNullOrEmpty(customerProfileResponse.Item2))
        {
            // if there is no fault message we just had a straight up error... so fail out
            if (string.IsNullOrEmpty(customerProfileResponse.Item3)) return new Tuple<bool, OracleCustomerAccountProfile, string>(false, null, $"There was an error finding the Customer Account Profile in Oracle: {customerProfileResponse.Item2}.");
            // check the fault message to see if the error states the profile doesn't exist
            if (customerProfileResponse.Item3.Contains($"doesn't exist")) return new Tuple<bool, OracleCustomerAccountProfile, string>(true, null, $"Customer Profile not found for Account Number '{customerAccountNumber}'.");
        }
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindCustomerProfileEnvelope));
        var oracleCustomerAccountProfile = (FindCustomerProfileEnvelope)serializer.Deserialize(customerProfileResponse.Item1.CreateReader());

        // map to simple object
        var customerProfileResult = new OracleCustomerAccountProfile
        {
            PartyId = oracleCustomerAccountProfile?.Body?.getActiveCustomerProfileResponse?.result?.Value?.CustomerAccountId
        };

        // return the Customer Profile PartyId
        return new Tuple<bool, OracleCustomerAccountProfile, string>(true, customerProfileResult, string.Empty);
    }

    public async Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(ulong? customerAccountId, uint customerAccountNumber, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerProfileInOracle, ActionObjectType.Account, StatusType.Started, customerAccountNumber.ToString());

        // populate the template
        var accountProfileEnvelope = OracleSoapTemplates.CreateCustomerProfile(customerAccountId, customerAccountNumber);

        // create the Customer Account via SOAP service
        var accountProfileResponse = await _oracleClient.SendSoapRequest(accountProfileEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerProfile"]}");
        if (!string.IsNullOrEmpty(accountProfileResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreateCustomerProfileInOracle, ActionObjectType.Account, StatusType.Error, customerAccountNumber.ToString(), accountProfileResponse.Item2);
            return new Tuple<OracleCustomerAccountProfile, string>(null, $"There was an error creating the Customer Account Profile in Oracle: {accountProfileResponse.Item2}.");
        }

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerProfileEnvelope));
        var oracleCustomerAccountProfile = (CreateCustomerProfileEnvelope)serializer.Deserialize(accountProfileResponse.Item1.CreateReader());

        // map the Oracle response to our simplified object
        var customerProfileResult = new OracleCustomerAccountProfile
        {
            PartyId = oracleCustomerAccountProfile?.Body?.createCustomerProfileResponse?.result?.Value?.PartyId
        };

        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerProfileInOracle, ActionObjectType.Account, StatusType.Successful, customerProfileResult.PartyId?.ToString());

        // return the Customer Account Profile
        return new Tuple<OracleCustomerAccountProfile, string>(customerProfileResult, string.Empty);
    }
    #endregion
    #endregion

    #region Address/Location
    public async Task<Tuple<bool, IEnumerable<OracleLocationModel>, string>> GetLocationsBySalesforceAddressId(List<string> addressIds)
    {
        var findLocationsEnvelope = OracleSoapTemplates.FindLocations(addressIds);
        // Find the Organization via SOAP service
        var findLocationsResponse = await _oracleClient.SendSoapRequest(findLocationsEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:LocationFoundation"]}");
        if (!string.IsNullOrEmpty(findLocationsResponse.Item2)) return new Tuple<bool, IEnumerable<OracleLocationModel>, string>(false, null, $"There was an error finding the Locations in Oracle: {findLocationsResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindLocationsEnvelope)); 
        var oracleLocationsResult = (FindLocationsEnvelope)serializer.Deserialize(findLocationsResponse.Item1.CreateReader());

        var result = oracleLocationsResult.Body?.getLocationByOriginalSystemReferenceResponse?.result;
        if (result == null || result.Count() == 0) return new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, null, $"Locations not found.");

        // map the response model into our simplified C# List
        var oracleLocations = new List<OracleLocationModel>();
        foreach (var location in result)
        {
            oracleLocations.Add(new OracleLocationModel
            {
                OrigSystemReference = location.OrigSystemReference,
                LocationId = location.LocationId,
                Address1 = location.Address1,
                Address2 = location.Address2,
                City = location.City,
                Country = location.Country,
                PostalCode = location.PostalCode,
                State = location.State
            });
        }

        // return the Locations that were found
        return new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, oracleLocations, null);
    }

    public async Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateLocationInOracle, ActionObjectType.Address, StatusType.Started, address.ObjectId);

        var location = RemapSalesforceAddressToOracleLocation(address);

        // populate the template
        var createLocationEnvelope = OracleSoapTemplates.CreateLocation(location);
        
        // create the Location via SOAP service
        var locationServiceUrl = $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Location"]}";
        var locationResponse = await _oracleClient.SendSoapRequest(createLocationEnvelope, locationServiceUrl);
        if (!string.IsNullOrEmpty(locationResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreateLocationInOracle, ActionObjectType.Address, StatusType.Error, address.ObjectId, locationResponse.Item2);
            return new Tuple<OracleLocationModel, string>(null, $"There was an error creating the Location in Oracle: {locationResponse.Item2}.");
        }
        
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateLocationEnvelope));
        var oracleLocation = (CreateLocationEnvelope)serializer.Deserialize(locationResponse.Item1.CreateReader());

        // map response model to simplified object
        var oracleResult = oracleLocation?.Body?.createLocationResponse?.result?.Value;
        var locationResult = new OracleLocationModel
        {
            LocationId = oracleResult?.LocationId,
            OrigSystemReference = oracleResult?.OrigSystemReference?.ToString(),
            Address1 = oracleResult?.Address1?.ToString(),
            Address2 = oracleResult?.Address2.ToString(),
            City = oracleResult?.City?.ToString(),
            State = oracleResult?.State?.ToString(),
            PostalCode = oracleResult?.PostalCode?.ToString(),
            Country = oracleResult?.Country?.ToString()
        };

        await LogAction(transaction, SalesforceTransactionAction.CreateLocationInOracle, ActionObjectType.Address, StatusType.Started, locationResult.LocationId.ToString());

        // return the simplified Location object
        return new Tuple<OracleLocationModel, string>(locationResult, string.Empty);
    }

    public Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(ulong organizationPartyId, List<OraclePartySite> partySites, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreatePartySiteInOracle, ActionObjectType.Account, StatusType.Started);

        // TODO: remap partySiteUse to list

        // populate the template
        var orgPartySiteEnvelope = OracleSoapTemplates.CreateOrganizationPartySites(organizationPartyId, partySites);

        // create the Party Site via SOAP service
        var partySiteResponse = await _oracleClient.SendSoapRequest(orgPartySiteEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Organization"]}");
        if (!string.IsNullOrEmpty(partySiteResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreatePartySiteInOracle, ActionObjectType.Account, StatusType.Error, organizationPartyId.ToString(), partySiteResponse.Item2);
            return new Tuple<List<OraclePartySite>, string>(null, $"There was an error creating the Party Site for Organization '{organizationPartyId}' in Oracle: {partySiteResponse.Item2}.");
        }

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(PartySiteEnvelope));
        var oraclePartySites = (PartySiteEnvelope)serializer.Deserialize(partySiteResponse.Item1.CreateReader());

        // map to a list of our simplified OraclePartySite model
        var partySitesResults = oraclePartySites?.Body?.mergeOrganizationResponse?.result?.Value?.PartySite
            .Select(ps => new OraclePartySite
            {
                PartySiteId = ps.PartySiteId,
                LocationId = ps.LocationId,
                OrigSystemReference = ps.OrigSystemReference,
                SiteUses = ps.PartySiteUse?.Select(psu => new OraclePartySiteUse
                {
                    PartySiteUseId = psu.PartySiteUseId,
                    SiteUseType = psu.SiteUseType
                }).ToList()
            }).ToList();

        await LogAction(transaction, SalesforceTransactionAction.CreatePartySiteInOracle, ActionObjectType.Account, StatusType.Successful, organizationPartyId.ToString());

        // return the Oracle PartySites
        return new Tuple<List<OraclePartySite>, string>(partySitesResults, string.Empty);
    }
    #endregion

    #region Contact/Person
    public async Task<Tuple<bool, IEnumerable<OraclePersonObject>, string>> GetPersonsBySalesforceContactId(List<string> contactIds)
    {
        var findPersonsEnvelope = OracleSoapTemplates.FindPersons(contactIds);
        // Find the Organization via SOAP service
        var findPersonsResponse = await _oracleClient.SendSoapRequest(findPersonsEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Person"]}");
        if (!string.IsNullOrEmpty(findPersonsResponse.Item2)) return new Tuple<bool, IEnumerable<OraclePersonObject>, string>(false, null, $"There was an error finding the Persons in Oracle: {findPersonsResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindPersonsEnvelope));
        var oraclePersonsResult = (FindPersonsEnvelope)serializer.Deserialize(findPersonsResponse.Item1.CreateReader());

        var result = oraclePersonsResult.Body?.findPersonResponse?.result;
        if (result == null || result.Count() == 0) return new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, null, $"Persons not found.");

        // map the response model into our simplified C# List
        var oraclePersons = new List<OraclePersonObject>();
        foreach (var person in result)
        {
            oraclePersons.Add(new OraclePersonObject
            {
                OrigSystemReference = person.OrigSystemReference,
                RelationshipId = person.Relationship?.RelationshipId,
                PartyId = person.PartyId,
                EmailAddress = person.EmailAddress,
                FirstName = person.PersonFirstName,
                LastName = person.PersonLastName
            });;
        }

        // return the Persons that were found
        return new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, oraclePersons, null);
    }

    public async Task<Tuple<OraclePersonObject, string>> CreatePerson(SalesforceContactModel model, string organizationPartyId, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreatePersonInOracle, ActionObjectType.Contact, StatusType.Started, model.ObjectId);

        // map SF model to Oracle Person
        var person = RemapSalesforceContactToOraclePerson(model);
        
        // populate the template
        var createPersonEnvelope = OracleSoapTemplates.CreatePerson(person, organizationPartyId);
        
        // create the Person via SOAP service
        var createPersonResponse = await _oracleClient.SendSoapRequest(createPersonEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Person"]}");
        if (!string.IsNullOrEmpty(createPersonResponse.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreatePersonInOracle, ActionObjectType.Contact, StatusType.Error, model.ObjectId, createPersonResponse.Item2);
            return new Tuple<OraclePersonObject, string>(null, $"There was an error creating the Person in Oracle: {createPersonResponse.Item2}.");
        }
        
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreatePersonEnvelope));
        var res = (CreatePersonEnvelope)serializer.Deserialize(createPersonResponse.Item1.CreateReader());

        // map response model to a simplified object
        var oracleResult = res?.Body?.createPersonResponse?.result?.Value;
        var oraclePerson = new OraclePersonObject
        {
            PartyId = oracleResult?.PartyId,
            OrigSystemReference = oracleResult?.OrigSystemReference,
            RelationshipId = oracleResult?.Relationship.RelationshipId,
            EmailAddress = oracleResult?.EmailAddress,
            FirstName = oracleResult?.PersonFirstName,
            LastName = oracleResult?.PersonLastName,
            PhoneNumber = oracleResult?.PrimaryPhoneNumber
            // TODO: validate the format of this data
            //PhoneAreaCode = res?.Body?.createPersonResponse?.result?.Value?.Phone.PhoneAreaCode,
        };

        await LogAction(transaction, SalesforceTransactionAction.CreatePersonInOracle, ActionObjectType.Contact, StatusType.Successful, oraclePerson.PartyId.ToString());

        // return the simplified Person object
        return new Tuple<OraclePersonObject, string>(oraclePerson, string.Empty);
    }

    public Task<Tuple<string, string>> UpdatePerson(SalesforceContactModel model, SalesforceContactModel transaction)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Helpers

    private CreateOracleOrganizationModel RemapSalesforceAccountToCreateOracleOrganization(SalesforceAccountModel model)
    {
        var organization = new CreateOracleOrganizationModel
        {
            OrganizationName = model.Name,
            TaxpayerIdentificationNumber = model.TaxId,
            SourceSystem = "SFDC",
            SourceSystemReferenceValue = model.ObjectId
        };

        return organization;
    }

    private UpdateOracleOrganizationModel RemapSalesforceAccountToUpdateOracleOrganization(SalesforceAccountModel model, OracleOrganization? existingOrganization = null)
    {
        var organization = new UpdateOracleOrganizationModel
        {
            OrganizationName = model.Name,
            TaxpayerIdentificationNumber = model.TaxId,
            SourceSystemReferenceValue = model.ObjectId
        };

        // populate the party metadata
        if (existingOrganization != null)
        {
            organization.PartyId = existingOrganization.PartyId;
            organization.PartyNumber = existingOrganization.PartyNumber;
        }

        return organization;
    }
    private OracleOrganization RemapSalesforceAccountToOracleOrganization(SalesforceAccountModel model, OracleOrganization? existingOrganization = null)
    {
        var organization = new OracleOrganization
        {
            OrganizationName = model.Name,
            OrigSystemReference = model.ObjectId,
            TaxpayerIdentificationNumber = model.TaxId,
            SourceSystem = "SFDC",
            SourceSystemReferenceValue = model.ObjectId
        };

        // populate the 
        if (existingOrganization != null)
        {
            organization.PartyId = existingOrganization.PartyId;
            organization.PartyNumber = existingOrganization.PartyNumber;
        }

        return organization;
    }

    private OracleCustomerAccount RemapOrganizationToCustomerAccount(SalesforceAccountModel model)
    {
        var account = new OracleCustomerAccount
        {
            SalesforceId = model.ObjectId,
            AccountName = model.Name,
        };
        // check for accountType
        var accountType = model.AccountType;
        if (!string.IsNullOrEmpty(accountType)) account.AccountType = OracleSoapTemplates.CustomerTypeMap.GetValue(accountType);
        
        return account;
    }

    private OracleCustomerAccount RemapSalesforceAccountToOracleCustomerAccount(SalesforceAccountModel model, OracleCustomerAccount? existingCustomerAccount = null)
    {
        var customerAccount = new OracleCustomerAccount
        {
            AccountName = model.Name,
            SalesforceId = model.ObjectId,
            OrigSystemReference = model.ObjectId,
            OssId = model.OssId
        };

        // check for accountType
        if (!string.IsNullOrEmpty(model.AccountType)) customerAccount.AccountType = OracleSoapTemplates.CustomerTypeMap.GetValue(model.AccountType);

        // populate the existing metadata
        if (existingCustomerAccount != null)
        {
            customerAccount.CustomerAccountId = existingCustomerAccount.CustomerAccountId;
            customerAccount.PartyId = existingCustomerAccount.PartyId;
            customerAccount.AccountNumber = existingCustomerAccount.AccountNumber;
            customerAccount.OrigSystemReference = existingCustomerAccount.OrigSystemReference;
        }

        return customerAccount;
    }

    private OracleLocationModel RemapSalesforceAddressToOracleLocation(SalesforceAddressModel address)
    {
        var location = new OracleLocationModel
        {
            Address1 = address.Address1,
            Address2 = address.Address2,
            City = address.City,
            OrigSystemReference = address.ObjectId,
            State = address.StateProvince,
            PostalCode = address.PostalCode
        };

        if (!string.IsNullOrEmpty(address.Country)) location.Country = OracleSoapTemplates.CountryShortcodes.GetValue(address.Country);

        return location;
    }

    private OraclePersonObject RemapSalesforceContactToOraclePerson(SalesforceContactModel model)
    {
        // map the model
        var person = new OraclePersonObject
        {
            OrigSystemReference = model.ObjectId,
            EmailAddress = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.Phone // accept full # that has country code and area code together
        };
        return person;
    }

    private async Task LogAction(SalesforceActionTransaction transaction, SalesforceTransactionAction action, ActionObjectType objectType, StatusType status, string? entityId = null, string? errorMessage = null)
    {
        var actionRecord = new SalesforceActionRecord
        {
            ObjectType = objectType,
            Action = action,
            Status = status,
            Timestamp = DateTime.UtcNow,
            ErrorMessage = errorMessage,
            EntityId = entityId
        };
        if (transaction.TransactionLog == null) transaction.TransactionLog = new List<SalesforceActionRecord>();
        transaction.TransactionLog?.Add(actionRecord);
        await _actionsRepository.AddTransactionRecord(transaction.Id, transaction.Object.ToString() ?? "Unknown", actionRecord);
    }
    #endregion
}
