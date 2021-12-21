﻿using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    // Account Endpoints
    Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model);
    Task<Tuple<OracleOrganization, string>> UpdateOrganization(string organizationId, SalesforceAccountModel model);
    Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(long organizationPartyId, List<OraclePartySite> partySites);
    Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(long organizationPartyId, SalesforceAccountModel model, List<OraclePartySite> partySites);
    Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(string customerAccountId, uint customerAccountNumber);
    Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction); // TODO: Not sure what's necessary in this signature
    Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId);
    Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId);
    Task<Tuple<bool, OracleCustomerAccountProfile, string>> GetCustomerProfileByAccountNumber(string customerAccountNumber);
    // Address Endpoints
    Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address);
    Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction);
    // Contact Endpoints
    Task<Tuple<string, string>> CreatePerson(SalesforceContactModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdatePerson(SalesforceContactModel model, SalesforceContactModel transaction);
}

public class OracleService : IOracleService
{
    private readonly IOracleClient _oracleClient;
    private readonly IConfiguration _config;

    public OracleService(IOracleClient oracleClient, IConfiguration config)
    {
        _oracleClient = oracleClient;
        _config = config;
    }

    #region Account / Organization / Customer Account / Customer Profile
    public async Task<Tuple<OracleOrganization, string>> CreateOrganization(SalesforceAccountModel model)
    {
        var organization = await _oracleClient.CreateOrganization(model);
        if (!string.IsNullOrEmpty(organization.Item2)) return new Tuple<OracleOrganization, string>(null, $"There was an error adding the account to Oracle: {organization.Item2}");
        return new Tuple<OracleOrganization, string>(organization.Item1, String.Empty);
    }

    public async Task<Tuple<OracleLocationModel, string>> CreateLocation(SalesforceAddressModel address)
    {
        // populate the template
        var createLocationEnvelope = OracleSoapTemplates.CreateLocation(address);
        // create the Location via SOAP service
        var locationServiceUrl = $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Location"]}";
        var locationResponse = await _oracleClient.SendSoapRequest(createLocationEnvelope, locationServiceUrl);
        if (!string.IsNullOrEmpty(locationResponse.Item2)) return new Tuple<OracleLocationModel, string>(null, $"There was an error creating the Location in Oracle: {locationResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateLocationEnvelope));
        var oracleLocation = (CreateLocationEnvelope)serializer.Deserialize(locationResponse.Item1.CreateReader());

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

        // TODO: Which Id are we returning to store in SF?
        return new Tuple<OracleLocationModel, string>(location, string.Empty);
    }

    public async Task<Tuple<List<OraclePartySite>, string>> CreateOrganizationPartySites(long organizationPartyId, List<OraclePartySite> partySites)
    {
        // populate the template
        var orgPartySiteEnvelope = OracleSoapTemplates.CreateOrganizationPartySites(organizationPartyId, partySites);
        // create the Party Site via SOAP service
        var partySiteResponse = await _oracleClient.SendSoapRequest(orgPartySiteEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Organization"]}");
        if (!string.IsNullOrEmpty(partySiteResponse.Item2)) return new Tuple<List<OraclePartySite>, string>(null, $"There was an error creating the Party Site for Organization '{organizationPartyId}' in Oracle: {partySiteResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(PartySiteEnvelope));
        var oraclePartySites = (PartySiteEnvelope)serializer.Deserialize(partySiteResponse.Item1.CreateReader());
        // map to a list of our simplified OraclePartySite model
        var partySitesResults = oraclePartySites?.Body?.mergeOrganizationResponse?.result?.Value?.PartySite
            .Select(ps => new OraclePartySite { 
                PartySiteId = ps.PartySiteId,
                LocationId = ps.LocationId,
                OrigSystemReference = ps.OrigSystemReference,
                SiteUseType = ps.PartySiteUse.SiteUseType
            }).ToList();

        // TODO: Which Id are we returning to store in SF?
        return new Tuple<List<OraclePartySite>, string>(partySitesResults, string.Empty);
    }
    public async Task<Tuple<OracleCustomerAccount, string>> CreateCustomerAccount(long organizationPartyId, SalesforceAccountModel model, List<OraclePartySite> partySites) 
    {
        // map the model values to the expected Customer Account payload
        var customerAccount = RemapOrganizationToOracleCustomerAccount(model);
        // populate the template
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(customerAccount, organizationPartyId, partySites);
        // create the Customer Account via SOAP service
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2)) return new Tuple<OracleCustomerAccount, string>(null, $"There was an error creating the Customer Account in Oracle: {customerAccountResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerAccountResponseEnvelope));
        var oracleCustomerAccount = (CreateCustomerAccountResponseEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        // TODO: map to simple object
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

        // TODO: Which Id are we returning to store in SF?
        return new Tuple<OracleCustomerAccount, string>(customerAccountResult, string.Empty);
    }
    public async Task<Tuple<OracleCustomerAccountProfile, string>> CreateCustomerAccountProfile(string customerAccountId, uint customerAccountNumber)
    {
        // populate the template
        var accountProfileEnvelope = OracleSoapTemplates.CreateCustomerProfile(customerAccountId, customerAccountNumber);
        // create the Customer Account via SOAP service
        var accountProfileResponse = await _oracleClient.SendSoapRequest(accountProfileEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerProfile"]}");
        if (!string.IsNullOrEmpty(accountProfileResponse.Item2)) return new Tuple<OracleCustomerAccountProfile, string>(null, $"There was an error creating the Customer Account Profile in Oracle: {accountProfileResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerProfileEnvelope));
        var oracleCustomerAccountProfile = (CreateCustomerProfileEnvelope)serializer.Deserialize(accountProfileResponse.Item1.CreateReader());

        // TODO: map to simple object
        var customerProfileResult = new OracleCustomerAccountProfile
        {
            PartyId = oracleCustomerAccountProfile?.Body?.createCustomerProfileResponse?.result?.Value?.PartyId
        };

        // TODO: Which Id are we returning to store in SF?
        return new Tuple<OracleCustomerAccountProfile, string>(customerProfileResult, string.Empty);
    }
    public async Task<Tuple<OracleOrganization, string>> UpdateOrganization(string organizationId, SalesforceAccountModel model)
    {
        var organization = RemapSalesforceAccountToOracleOrganization(model.Name, model.TaxId);
        if (string.IsNullOrEmpty(organizationId)) return new Tuple<OracleOrganization, string>(null, $"oracleCustomerAccountId must be present to update the Oracle Account record.");
        var added = await _oracleClient.UpdateOrganization(organization, organizationId);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<OracleOrganization, string>(null, $"There was an error updating the account in Oracle: {added.Item2}");
        return new Tuple<OracleOrganization, string>(added.Item1, string.Empty);
    }
    public Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }
    public async Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId)
    {
        // populate the template
        var findOrganizationEnvelope = OracleSoapTemplates.FindOrganization(organizationName, salesforceAccountId);
        // Find the Organization via SOAP service
        var findOrgResponse = await _oracleClient.SendSoapRequest(findOrganizationEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:Organization"]}");
        if (!string.IsNullOrEmpty(findOrgResponse.Item2)) return new Tuple<bool, OracleOrganization, string>(false, null, $"There was an error finding the Organization in Oracle: {findOrgResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindOrganizationEnvelope));
        var oracleCustomerAccount = (FindOrganizationEnvelope)serializer.Deserialize(findOrgResponse.Item1.CreateReader());
        if (oracleCustomerAccount.Body?.findOrganizationResponse?.result?.Value == null) return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");
        // map the response model into our simplified C# model
        var organization = new OracleOrganization
        {
            OrganizationName = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyName,
            PartyId = (long)oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyId,
            PartyNumber = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartyNumber.ToString(),
            OrigSystemReference = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.OriginalSystemReference?.OrigSystemReference,
            // map the response object to our C# model for party sites
            PartySites = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.PartySite
                .Select(ps => new OraclePartySite { 
                    PartySiteId = ps.PartySiteId, 
                    OrigSystemReference = ps.OrigSystemReference, 
                    LocationId = ps.LocationId })
                .ToList()
        };
        if (string.IsNullOrEmpty(organization.OrigSystemReference)) return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");
        return new Tuple<bool, OracleOrganization, string>(true, organization, null);
    }
    public async Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId)
    {
        var findAccountEnvelope = OracleSoapTemplates.FindCustomerAccount(salesforceAccountId);
        // Find the Organization via SOAP service
        var findAccountResponse = await _oracleClient.SendSoapRequest(findAccountEnvelope, $"{_config["Oracle:Endpoint"]}/{_config["Oracle:Services:CustomerAccount"]}");
        if (!string.IsNullOrEmpty(findAccountResponse.Item2)) return new Tuple<bool, OracleCustomerAccount, string>(false, null, $"There was an error finding the Customer Account in Oracle: {findAccountResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindCustomerAccountEnvelope));
        var oracleCustomerAccount = (FindCustomerAccountEnvelope)serializer.Deserialize(findAccountResponse.Item1.CreateReader());
        if (oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        // map the response model into our simplified C# model
        var account = new OracleCustomerAccount
        { 
            PartyId = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.PartyId.ToString(),
            AccountNumber = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.AccountNumber,
            OrigSystemReference = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.OrigSystemReference.ToString()
        };
        if (string.IsNullOrEmpty(account.OrigSystemReference)) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        return new Tuple<bool, OracleCustomerAccount, string>(true, account, null);
    }
    public async Task<Tuple<bool, OracleCustomerAccountProfile, string>> GetCustomerProfileByAccountNumber(string customerAccountNumber)
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
    #endregion

    #region Address/Location
    public async Task<Tuple<string, string>> CreateLocation(SalesforceAddressModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Tuple<string, string>> UpdateLocation(SalesforceContactModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Contact/Person
    public async Task<Tuple<string, string>> CreatePerson(SalesforceContactModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Tuple<string, string>> UpdatePerson(SalesforceContactModel model, SalesforceContactModel transaction)
    {
        throw new NotImplementedException();
    }
    #endregion

    private OracleOrganization RemapSalesforceAccountToOracleOrganization(string name, string taxId)
    {
        var organization = new OracleOrganization
        {
            OrganizationName = name,
            TaxpayerIdentificationNumber = taxId
        };
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
}
