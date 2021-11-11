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
    Task<Tuple<string, string>> AddAccount(SalesforceActionObject model);
    Task<Tuple<string, string>> UpdateAccount(SalesforceActionObject model);
    Task<Tuple<string, string>> AddAddress(SalesforceActionObject model);
    Task<Tuple<string, string>> UpdateAddress(SalesforceActionObject model);
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

    public async Task<Tuple<string, string>> AddAccount(SalesforceActionObject model)
    {
        var account = RemapSalesforceAccountToOracleAccount(model);
        // create the Organization via REST endpoint
        var added = await _oracleClient.CreateAccount(account);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error adding the account to Oracle: {added.Item2}");

        // TODO: consider using a DB generated value to track AccountNumber (Oracle) value on our end
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        account.PartyId = added.Item1.PartyId.ToString();
        account.AccountNumber = timestamp.ToString();
        var customerAccountEnvelope = OracleSoapTemplates.CreateCustomerAccount(account);

        // create the Customer Account via SOAP service (using added.Item1.PartyId acquired from creating the Organization above)
        var customerAccountServiceUrl = $"{_config["Oracle:Endpoint"]}/crmService/CustomerAccountService";
        var customerAccountResponse = await _oracleClient.SendSoapRequest(customerAccountEnvelope, customerAccountServiceUrl);
        if (!string.IsNullOrEmpty(customerAccountResponse.Item2)) return new Tuple<string, string>(null, $"There was an error creating the Customer Account in Oracle: {customerAccountResponse.Item2}.");

        // deserialize the xml response envelope
        XmlSerializer serializer = new(typeof(CreateCustomerAccountResponseEnvelope));
        var oracleCustomerAccount = (CreateCustomerAccountResponseEnvelope)serializer.Deserialize(customerAccountResponse.Item1.CreateReader());

        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> UpdateAccount(SalesforceActionObject model)
    {
        var account = RemapSalesforceAccountToOracleAccount(model);
        var partyNumber = model.ObjectValues?.GetValue("oracleAccountId")?.ToString();
        if (string.IsNullOrEmpty(partyNumber)) return new Tuple<string, string>(null, $"oracleAccountId must be present to update the Oracle Account record.");
        var added = await _oracleClient.UpdateAccount(account, partyNumber);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error updating the account in Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> AddAddress(SalesforceActionObject model)
    {
        var address = RemapSalesforceAddressToOracleAddress(model);
        var accountNumber = model.ObjectValues?.GetValue("parentAccountOracleId")?.ToString();
        var added = await _oracleClient.CreateAddress(accountNumber, address);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error adding the address to Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    public async Task<Tuple<string, string>> UpdateAddress(SalesforceActionObject model)
    {
        var address = RemapSalesforceAddressToOracleAddress(model);
        var accountNumber = model.ObjectValues?.GetValue("parentAccountOracleId")?.ToString();
        var partyNumber = model.ObjectValues?.GetValue("partyNumber").ToString(); // This is the actual Id of the Address record in Oracle (it has to be stored on the record itself)
        var added = await _oracleClient.UpdateAddress(accountNumber, address, partyNumber);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<string, string>(null, $"There was an error updating the address in Oracle: {added.Item2}");
        return new Tuple<string, string>(added.Item1.PartyNumber, string.Empty);
    }

    private CreateOracleAccountViewModel RemapSalesforceAccountToOracleAccount(SalesforceActionObject model)
    {
        var account = new CreateOracleAccountViewModel();
        account.OrganizationName = model.ObjectValues?.GetValue("name")?.ToString();
        account.AccountType = model.ObjectValues?.GetValue("accountType")?.ToString();
        return account;
    }

    private CreateOracleAddressViewModel RemapSalesforceAddressToOracleAddress(SalesforceActionObject model)
    {
        var address = new CreateOracleAddressViewModel();
        address.Address1 = model.ObjectValues?.GetValue("address1")?.ToString();
        address.Address2 = model.ObjectValues?.GetValue("address2")?.ToString();
        address.Country = model.ObjectValues?.GetValue("country")?.ToString();
        return address;
    }
}
