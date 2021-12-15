using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    // Account Endpoints
    Task<Tuple<string, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> CreateCustomerAccount(string organizationId, SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> CreateCustomerAccountProfile(string customerAccountId, SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateOrganization(string organizationId, SalesforceAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction); // TODO: Not sure what's necessary in this signature
    Task<Tuple<string, string>> UpdateCustomerAccountProfile(SalesforceAccountModel model, SalesforceActionTransaction transaction); // TODO: Same as above
    Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId);
    Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId);
    Task<Tuple<OracleCustomerAccountProfile, string>> GetCustomerAccountProfileBySalesforceAccountId(string salesforceAccountId);
    // Address Endpoints
    Task<Tuple<string, string>> CreateLocation(SalesforceAddressModel model, SalesforceActionTransaction transaction);
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
    public async Task<Tuple<string, string>> CreateOrganization(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        var organization = await _oracleClient.CreateOrganization(model);
        if (!string.IsNullOrEmpty(organization.Item2)) return new Tuple<string, string>(null, $"There was an error adding the account to Oracle: {organization.Item2}");
        // TODO: The first string in this tuple should be the "organizationId" -- whatever that is
        return new Tuple<string, string>(organization.Item1.PartyNumber, String.Empty);
    }
    public async Task<Tuple<string, string>> CreateCustomerAccount(string organizationId, SalesforceAccountModel model, SalesforceActionTransaction transaction) 
    {
        // map the model values to the expected Customer Account payload
        var customerAccount = RemapOrganizationToOracleCustomerAccount(model);
        // set the PartyId acquired from the successful creation of the Organization above
        customerAccount.OrganizationPartyId = organizationId;
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(customerAccount);
        // create the Customer Account via SOAP service
        var customerAccountServiceUrl = $"{_config["Oracle:Endpoint"]}/crmService/CustomerAccountService";
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, customerAccountServiceUrl);
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2)) return new Tuple<string, string>(null, $"There was an error creating the Customer Account in Oracle: {customerAccountResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerAccountResponseEnvelope));
        var oracleCustomerAccount = (CreateCustomerAccountResponseEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        // TODO: Which Id are we returning to store in SF?
        return new Tuple<string, string>(oracleCustomerAccount?.Body?.createCustomerAccountResponse?.result?.Value?.CustomerAccountId.ToString(), string.Empty);
    }
    public async Task<Tuple<string, string>> CreateCustomerAccountProfile(string customerAccountId, SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }
    public async Task<Tuple<string, string>> UpdateOrganization(string organizationId, SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        var organization = RemapSalesforceAccountToOracleOrganization(model.Name, model.TaxId);
        if (string.IsNullOrEmpty(organizationId)) return new Tuple<string, string>(null, $"oracleCustomerAccountId must be present to update the Oracle Account record.");
        var added = await _oracleClient.UpdateAccount(organization, organizationId);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error updating the account in Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }
    public Task<Tuple<string, string>> UpdateCustomerAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }
    public Task<Tuple<string, string>> UpdateCustomerAccountProfile(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        throw new NotImplementedException();
    }
    public async Task<Tuple<bool, OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string organizationName, string salesforceAccountId)
    {
        var findOrganizationEnvelope = OracleSoapTemplates.FindOrganization(organizationName, salesforceAccountId);
        // Find the Organization via SOAP service
        var organizationServiceUrl = $"{_config["Oracle:Endpoint"]}/crmService/FoundationPartiesOrganizationService";
        var findOrgResponse = await _oracleClient.SendSoapRequest(findOrganizationEnvelope, organizationServiceUrl);
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
            OrigSystemReference = oracleCustomerAccount.Body.findOrganizationResponse.result.Value.OriginalSystemReference?.OrigSystemReference
        };
        if (string.IsNullOrEmpty(organization.OrigSystemReference)) return new Tuple<bool, OracleOrganization, string>(true, null, $"Organization not found.");
        return new Tuple<bool, OracleOrganization, string>(true, organization, null);
    }
    public async Task<Tuple<bool, OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId)
    {
        var findAccountEnvelope = OracleSoapTemplates.FindCustomerAccount(salesforceAccountId);
        // Find the Organization via SOAP service
        var accountServiceUrl = $"{_config["Oracle:Endpoint"]}/crmService/CustomerAccountService";
        var findAccountResponse = await _oracleClient.SendSoapRequest(findAccountEnvelope, accountServiceUrl);
        if (!string.IsNullOrEmpty(findAccountResponse.Item2)) return new Tuple<bool, OracleCustomerAccount, string>(false, null, $"There was an error finding the Customer Account in Oracle: {findAccountResponse.Item2}.");
        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(FindCustomerAccountEnvelope));
        var oracleCustomerAccount = (FindCustomerAccountEnvelope)serializer.Deserialize(findAccountResponse.Item1.CreateReader());
        if (oracleCustomerAccount.Body?.findCustomerAccountResponse?.result?.Value == null) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        // map the response model into our simplified C# model
        var account = new OracleCustomerAccount
        { 
            PartyId = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.PartyId.ToString(),
            PartyNumber = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.AccountNumber.ToString(),
            OrigSystemReference = oracleCustomerAccount.Body.findCustomerAccountResponse.result.Value.OrigSystemReference.ToString()
        };
        if (string.IsNullOrEmpty(account.OrigSystemReference)) return new Tuple<bool, OracleCustomerAccount, string>(true, null, $"Customer Account not found.");
        return new Tuple<bool, OracleCustomerAccount, string>(true, account, null);
    }
    public Task<Tuple<OracleCustomerAccountProfile, string>> GetCustomerAccountProfileBySalesforceAccountId(string salesforceAccountId)
    {
        throw new NotImplementedException();
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

    private CreateOracleAccountViewModel RemapSalesforceAccountToOracleOrganization(string name, string taxId)
    {
        var account = new CreateOracleAccountViewModel
        {
            OrganizationName = name,
            TaxpayerIdentificationNumber = taxId
        };
        return account;
    }

    private CreateOracleCustomerAccountViewModel RemapOrganizationToOracleCustomerAccount(SalesforceAccountModel model)
    {
        var account = new CreateOracleCustomerAccountViewModel
        {
            SalesforceId = model.ObjectId,
            OrganizationName = model.Name,
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
