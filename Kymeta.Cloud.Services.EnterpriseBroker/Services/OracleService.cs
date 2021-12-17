using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
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
    Task<Tuple<OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string salesforceAccountId);
    Task<Tuple<OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId);
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
        // TODO: This needs to be updated to reflect the actual organization creation
        var added = await _oracleClient.CreateOrganization(model.Name);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error adding the account to Oracle: {added.Item2}");
        // TODO: The first string in this tuple should be the "organizationId" -- whatever that is
        return new Tuple<string, string>(added.Item1.PartyNumber, String.Empty);
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
        var account = RemapSalesforceAccountToOracleOrganization(model.Name, model.TaxId);
        if (string.IsNullOrEmpty(organizationId)) return new Tuple<string, string>(null, $"oracleCustomerAccountId must be present to update the Oracle Account record.");
        var added = await _oracleClient.UpdateAccount(account, organizationId);
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
    public async Task<Tuple<OracleOrganization, string>> GetOrganizationBySalesforceAccountId(string salesforceAccountId)
    {
        throw new NotImplementedException();
    }
    public async Task<Tuple<OracleCustomerAccount, string>> GetCustomerAccountBySalesforceAccountId(string salesforceAccountId)
    {
        throw new NotImplementedException();
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
