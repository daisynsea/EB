using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    /// <summary>
    /// Add an Account to Oracle. This includes the Organization, Location (Site), Customer Account, and Customer Profile objects.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>
    /// Item1 is the partyNumber (unique Id) of the new Organization in Oracle
    /// Item2 is an error message should any problem arise during execution.
    /// </returns>
    Task<Tuple<string, string>> AddAccount(CreateAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateAccount(UpdateAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> AddAddress(CreateAddressModel model, SalesforceActionTransaction transaction);
    Task<Tuple<string, string>> UpdateAddress(UpdateAddressModel model, SalesforceActionTransaction transaction);
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

    public async Task<Tuple<string, string>> AddAccount(CreateAccountModel model, SalesforceActionTransaction transaction)
    {
        var account = RemapSalesforceAccountToOracleAccount(model.Name, model.TaxId);
        // create the Organization via REST endpoint
        var added = await _oracleClient.CreateAccount(account);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error adding the account to Oracle: {added.Item2}");

        // map the model values to the expected Customer Account payload
        var customerAccount = RemapOrganizationToOracleCustomerAccount(model);
        // set the PartyId acquired from the successful creation of the Organization above
        customerAccount.OrganizationPartyId = added.Item1.PartyId.ToString();
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(customerAccount);

        // create the Customer Account via SOAP service
        var customerAccountServiceUrl = $"{_config["Oracle:Endpoint"]}/crmService/CustomerAccountService";
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, customerAccountServiceUrl);
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2)) return new Tuple<string, string>(null, $"There was an error creating the Customer Account in Oracle: {customerAccountResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerAccountResponseEnvelope));
        var oracleCustomerAccount = (CreateCustomerAccountResponseEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> UpdateAccount(UpdateAccountModel model, SalesforceActionTransaction transaction)
    {
        var account = RemapSalesforceAccountToOracleAccount(model.Name, model.TaxId);
        var partyNumber = model.OracleCustomerAccountId;
        if (string.IsNullOrEmpty(partyNumber)) return new Tuple<string, string>(null, $"oracleCustomerAccountId must be present to update the Oracle Account record.");
        var added = await _oracleClient.UpdateAccount(account, partyNumber);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error updating the account in Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> AddAddress(CreateAddressModel model, SalesforceActionTransaction transaction)
    {
        var address = RemapSalesforceAddressToOracleAddress(model.Address1, model.Address2, model.Country);
        var accountNumber = model.ParentOracleAccountId;
        var added = await _oracleClient.CreateAddress(accountNumber, address);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error adding the address to Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> UpdateAddress(UpdateAddressModel model, SalesforceActionTransaction transaction)
    {
        var address = RemapSalesforceAddressToOracleAddress(model.Address1, model.Address2, model.Country);
        var accountNumber = model.ParentOracleAccountId;
        // TODO: Is this refactored anyway?
        var partyNumber = model.AddressOracleId; // This is the actual Id of the Address record in Oracle (it has to be stored on the record itself)
        var added = await _oracleClient.UpdateAddress(accountNumber, address, partyNumber);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error updating the address in Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    private CreateOracleAccountViewModel RemapSalesforceAccountToOracleAccount(string name, string taxId)
    {
        var account = new CreateOracleAccountViewModel
        {
            OrganizationName = name,
            TaxpayerIdentificationNumber = taxId
        };
        return account;
    }

    private CreateOracleCustomerAccountViewModel RemapOrganizationToOracleCustomerAccount(CreateAccountModel model)
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

    private CreateOracleAddressViewModel RemapSalesforceAddressToOracleAddress(string address1, string address2, string country)
    {
        var address = new CreateOracleAddressViewModel
        {
            Address1 = address1,
            Address2 = address2,
            Country = country
        };
        return address;
    }
}
