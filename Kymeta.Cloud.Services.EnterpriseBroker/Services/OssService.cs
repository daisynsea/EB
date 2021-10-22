namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IOssService
{
    /// <summary>
    /// Adds a new account to the OSS service
    /// </summary>
    /// <param name="account">Account model</param>
    /// <returns>Added account</returns>
    Task<Tuple<Account, string>> AddAccount(SalesforceActionObject model, long? oracleAccountId);
    /// <summary>
    /// Update an existing account to OSS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateAccount(SalesforceActionObject model);
}

public class OssService : IOssService
{
    private readonly IConfiguration _config;
    private readonly IAccountsClient _accountsClient;
    private readonly IUsersClient _usersClient;
    private User _systemUser = new User { FirstName = "System", LastName = "User", Email = "kcssystemuser@kymeta.io" };

    public OssService(IConfiguration config, IAccountsClient accountsClient, IUsersClient usersClient)
    {
        _config = config;
        _accountsClient = accountsClient;
        _usersClient = usersClient;

        // system user configs
        _systemUser.Id = new Guid(config["SystemUserId"]);
        _systemUser.AccountId = new Guid(config["KymetaAccountId"]);
    }

    public async Task<Tuple<Account, string>> AddAccount(SalesforceActionObject model, long? oracleAccountId)
    {
        // Verify exists
        var existingAccount = await GetAccount(model.ObjectId);
        if (existingAccount != null) // If account exists, return from this action with an error
        {
            return new Tuple<Account, string>(null, $"Account with Salesforce ID {model.ObjectId} already exists in the system.");
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var account = RemapSalesforceAccountToOssAccount(model, existingUser, oracleAccountId);
        var added = await _accountsClient.AddAccount(account);
        if (!string.IsNullOrEmpty(added.Item2)) return new Tuple<Account, string>(null, $"There was an error adding the account to OSS: {added.Item2}");
        return new Tuple<Account, string>(added.Item1, string.Empty);
    }

    public async Task<Tuple<Account, string>> UpdateAccount(SalesforceActionObject model)
    {
        var existingAccount = await GetAccount(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            return new Tuple<Account, string>(null, $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.");
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var account = RemapSalesforceAccountToOssAccount(model, existingUser);
        var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
        if (!string.IsNullOrEmpty(updated.Item2)) return new Tuple<Account, string>(null, $"There was an error updating the account in OSS: {updated.Item2}");
        return new Tuple<Account, string>(updated.Item1, string.Empty);
    }

    public async Task<Account> GetAccount(string salesforceId)
    {
        return await _accountsClient.GetAccountBySalesforceId(salesforceId);
    }

    private Account RemapSalesforceAccountToOssAccount(SalesforceActionObject model, User user, long? oracleAccountId = null)
    {
        var account = new Account
        {
            SalesforceAccountId = model.ObjectId,
            Enabled = true,
            Name = model.ObjectValues?.GetValue("name")?.ToString(),
            Origin = CreatedOriginEnum.SF,
            CreatedById = user.Id,
            ModifiedById = user.Id,
            OracleAccountId = oracleAccountId.HasValue ? oracleAccountId.Value.ToString() : null,
            // TODO: Add ParentId based on the Parent account id property from Salesforce
            ParentId = _config.GetValue<Guid>("KymetaAccountId")
        };

        return account;
    }
}