namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface IOracleService
{
    Task<Tuple<long?, string>> AddAccount(SalesforceActionObject model);
    Task<Tuple<string, string>> UpdateAccount(SalesforceActionObject model);
    Task<Tuple<string, string>> AddAddress(SalesforceActionObject model);
    Task<Tuple<string, string>> UpdateAddress(SalesforceActionObject model);
}

public class OracleService : IOracleService
{
    private readonly IOracleClient _oracleClient;

    public OracleService(IOracleClient oracleClient)
    {
        _oracleClient = oracleClient;
    }

    public async Task<Tuple<long?, string>> AddAccount(SalesforceActionObject model)
    {
        var account = RemapSalesforceAccountToOracleAccount(model);
        // create the Organization via REST endpoint
        var added = await _oracleClient.CreateAccount(account);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<long?, string>(null, $"There was an error adding the account to Oracle: {added.Item2}");

        // TODO: creat the Customer Account via SOAP service (using added.Item1.PartyId which is necessary for the request)


        return new Tuple<long?, string>(added.Item1.PartyId, string.Empty);
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
