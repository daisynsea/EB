using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    // Account Endpoints
    Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<OracleOrganization, string>> UpdateOrganization(OracleOrganization existingOracleOrganization, SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(long organizationPartyId, List<OraclePartySite> partySites, SalesforceActionTransaction transaction);
    Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(long organizationPartyId, SalesforceAccountModel model, List<OraclePartySite> partySites, List<OraclePersonObject> persons, SalesforceActionTransaction transaction);
    Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(string customerAccountId, uint customerAccountNumber, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction); // TODO: Not sure what's necessary in this signature
    Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId, SalesforceActionTransaction transaction);
    Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId, SalesforceActionTransaction transaction);
    Task<Tuple<bool, OracleCustomerAccountProfile, string>> GetCustomerProfileByAccountNumber(string customerAccountNumber, SalesforceActionTransaction transaction);
    // Address Endpoints
    Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction);
    // Contact Endpoints
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
        if (oracleCustomerAccount.Body?.findOrganizationResponse?.result?.Value == null) return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");
        if (oracleCustomerAccount.Body?.findOrganizationResponse?.result?.Value?.OriginalSystemReference?.OrigSystemReference == null) return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");

        // map the response model into our simplified C# model
        var organization = new OracleOrganization
        {
            OrganizationName = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyName,
            PartyId = (long)oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyId,
            PartyNumber = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyNumber.ToString(),
            OrigSystemReference = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.OriginalSystemReference?.OrigSystemReference,
            // map the response object to our C# model for party sites
            PartySites = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartySite?
                .Select(ps => new OraclePartySite
                {
                    PartySiteId = ps.PartySiteId,
                    OrigSystemReference = ps.OrigSystemReference,
                    LocationId = ps.LocationId
                }).ToList(),
            Contacts = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.Relationship?
                .Select(r => new OracleOrganizationContact
                {
                    ContactPartyId = r.OrganizationContact.ContactPartyId,
                    OrigSystemReference = r.OrganizationContact.OrigSystemReference,
                    PersonFirstName = r.OrganizationContact.PersonFirstName,
                    PersonLastName = r.OrganizationContact.PersonLastName
                }).ToList()
        };

        // return the Organization that was found
        return new Tuple<bool, OracleOrganization, string>(true, organization, null);
    }

    public async Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Started);

        // map model to simplified object
        var orgToCreate = RemapSalesforceAccountToCreateOracleOrganization(model);
        
        // create the organization in Oracle
        var organization = await _oracleClient.CreateOrganization(orgToCreate);
        
        // validate the the org was created
        if (!string.IsNullOrEmpty(organization.Item2))
        {
            await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Error, model.ObjectId, organization.Item2);
            return new Tuple<OracleOrganization, string>(null, $"There was an error adding the account to Oracle: {organization.Item2}");
        }

        // TODO: map the response object to our simplified model

        await LogAction(transaction, SalesforceTransactionAction.CreateOrganizationInOracle, ActionObjectType.Account, StatusType.Successful, organization.Item1.PartyId.ToString());

        // return the Oracle Organization
        return new Tuple<OracleOrganization, string>(organization.Item1, String.Empty);
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
    public async Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId, SalesforceActionTransaction transaction)
    {
        var findAccountEnvelope = OracleSoapTemplates.FindCustomerAccount(salesforceAccountId);
        // Find the Organization via SOAP service
        var findAccountResponse = await _oracleClient.SendSoapRequest(findAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(findAccountResponse.Item2)) return new Tuple<bool, OracleCustomerAccount, string>(false, null, $"There was an error finding the Customer Account in Oracle: {findAccountResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindCustomerAccountEnvelope));
        var oracleCustomerAccount = (FindCustomerAccountEnvelope)serializer.Deserialize(findAccountResponse.Item1.CreateReader());
        if (oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        if (oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.OrigSystemReference == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");

        // map the response model into our simplified C# model
        var account = new OracleCustomerAccount
        {
            PartyId = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.PartyId.ToString(),
            AccountName = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.AccountName.ToString(),
            AccountNumber = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.AccountNumber,
            OrigSystemReference = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.OrigSystemReference.ToString(),
            Contacts = oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value?.CustomerAccountContacts
                .Select(cac => new OracleCustomerAccountContact
                {
                    ContactPersonId = cac.ContactPersonId,
                    OrigSystemReference = cac.OrigSystemReference,
                    IsPrimary = cac.PrimaryFlag,
                    RelationshipId = cac.RelationshipId.ToString()
                }).ToList()
        };

        // return the Customer Account that was found
        return new Tuple<bool, OracleCustomerAccount, string>(true, account, null);
    }

    public async Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(long organizationPartyId, SalesforceAccountModel model, List<OraclePartySite> partySites, List<OraclePersonObject> persons, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Started, model.ObjectId);

        // map the model values to the expected Customer Account payload
        var customerAccount = RemapOrganizationToOracleCustomerAccount(model);

        // populate the template
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(customerAccount, organizationPartyId, partySites, persons);

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
        var customerAccountResult = new OracleCustomerAccount
        {
            PartyId = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.PartyId.ToString(),
            CustomerAccountId = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.CustomerAccountId.ToString(),
            AccountNumber = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.AccountNumber,
            AccountName = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.AccountName?.ToString(),
            AccountType = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.CustomerType?.ToString(),
            AccountSubType = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.CustomerClassCode?.ToString(),
            OrigSystemReference = oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.OriginalSystemReference?.OrigSystemReference.ToString(),
        };

        await LogAction(transaction, SalesforceTransactionAction.CreateCustomerAccountInOracle, ActionObjectType.Account, StatusType.Successful, customerAccountResult.PartyId);

        // return the Customer Account
        return new Tuple<OracleCustomerAccount, string>(customerAccountResult, string.Empty);
    }

    public Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
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

    public async Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(string customerAccountId, uint customerAccountNumber, SalesforceActionTransaction transaction)
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
    public async Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateLocationInOracle, ActionObjectType.Address, StatusType.Started, address.ObjectId);

        // populate the template
        var createLocationEnvelope = OracleSoapTemplates.CreateLocation(address);
        
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
        var location = new OracleLocationModel
        {
            LocationId = oracleLocation?.Body?.createLocationResponse?.result?.Value?.LocationId,
            OrigSystemReference = oracleLocation?.Body?.createLocationResponse?.result?.Value?.OrigSystemReference?.ToString(),
            Address1 = oracleLocation?.Body?.createLocationResponse?.result?.Value?.Address1?.ToString(),
            Address2 = oracleLocation?.Body?.createLocationResponse?.result?.Value?.Address2.ToString(),
            City = oracleLocation?.Body?.createLocationResponse?.result?.Value?.City?.ToString(),
            State = oracleLocation?.Body?.createLocationResponse?.result?.Value?.State?.ToString(),
            PostalCode = oracleLocation?.Body?.createLocationResponse?.result?.Value?.PostalCode?.ToString(),
            Country = oracleLocation?.Body?.createLocationResponse?.result?.Value?.Country?.ToString()
        };

        await LogAction(transaction, SalesforceTransactionAction.CreateLocationInOracle, ActionObjectType.Address, StatusType.Started, location.LocationId.ToString());

        // return the simplified Location object
        return new Tuple<OracleLocationModel, string>(location, string.Empty);
    }

    public Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(long organizationPartyId, List<OraclePartySite> partySites, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreatePartySiteInOracle, ActionObjectType.Account, StatusType.Started);

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
                SiteUseType = ps.PartySiteUse.SiteUseType
            }).ToList();

        await LogAction(transaction, SalesforceTransactionAction.CreatePartySiteInOracle, ActionObjectType.Account, StatusType.Successful, organizationPartyId.ToString());

        // return the Oracle PartySites
        return new Tuple<List<OraclePartySite>, string>(partySitesResults, string.Empty);
    }
    #endregion

    #region Contact/Person
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
        var oraclePerson = new OraclePersonObject
        {
            PartyId = res?.Body?.createPersonResponse?.result?.Value?.PartyId,
            OrigSystemReference = res?.Body?.createPersonResponse?.result?.Value?.OrigSystemReference,
            RelationshipId = res?.Body?.createPersonResponse?.result?.Value?.Relationship.RelationshipId,
            EmailAddress = res?.Body?.createPersonResponse?.result?.Value?.EmailAddress,
            FirstName = res?.Body?.createPersonResponse?.result?.Value?.PersonFirstName,
            LastName = res?.Body?.createPersonResponse?.result?.Value?.PersonLastName,
            // TODO: validate the format of this data
            //PhoneAreaCode = res?.Body?.createPersonResponse?.result?.Value?.Phone.PhoneAreaCode,
            //PhoneNumber = res?.Body?.createPersonResponse?.result?.Value?.Phone.PhoneNumber,
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

        // populate the 
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

    private OracleCustomerAccount RemapOrganizationToOracleCustomerAccount(SalesforceAccountModel model)
    {
        var account = new OracleCustomerAccount
        {
            SalesforceId = model.ObjectId,
            AccountName = model.Name,
            TaxId = model.TaxId
        };
        // check for accountType
        var accountType = model.AccountType;
        if (!string.IsNullOrEmpty(accountType)) account.AccountType = OracleSoapTemplates.CustomerTypeMap.GetValue(accountType);
        // check for account sub-type
        var accountSubType = model.SubType;
        if (!string.IsNullOrEmpty(accountSubType)) account.AccountSubType = OracleSoapTemplates.CustomerClassMap.GetValue(accountSubType);
        return account;
    }

    private OraclePersonObject RemapSalesforceContactToOraclePerson(SalesforceContactModel model)
    {
        // TODO: separate the phone number into segments
        var phoneSegements = model.Phone.Split('-');

        // map the model
        var person = new OraclePersonObject
        {
            OrigSystemReference = model.ObjectId,
            EmailAddress = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneCountryCode = "1", // TODO: extract from Salesforce provided values when present
            PhoneAreaCode = phoneSegements[0],
            PhoneNumber = $"{phoneSegements[1]}-{phoneSegements[2]}"
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
