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
    Task<Tuple<Account, string>> AddAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing account to OSS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing account's address
    /// </summary>
    /// <param name="model"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateAccountAddress(UpdateAddressModel model, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing account's oracle id
    /// </summary>
    /// <param name="salesforceId"></param>
    /// <param name="oracleId"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateAccountOracleId(SalesforceAccountModel model, string oracleId, SalesforceActionTransaction transaction);
    /// <summary>
    /// Get an OSS Account by its Salesforce Id
    /// </summary>
    /// <param name="salesforceId"></param>
    /// <returns></returns>
    Task<Account?> GetAccountBySalesforceId(string salesforceId);
}

public class OssService : IOssService
{
    private readonly IConfiguration _config;
    private readonly IAccountsClient _accountsClient;
    private readonly IUsersClient _usersClient;
    private readonly IActionsRepository _actionsRepository;
    private User _systemUser = new User { FirstName = "System", LastName = "User", Email = "kcssystemuser@kymeta.io" };

    public OssService(IConfiguration config, IAccountsClient accountsClient, IUsersClient usersClient, IActionsRepository actionsRepository)
    {
        _config = config;
        _accountsClient = accountsClient;
        _usersClient = usersClient;
        _actionsRepository = actionsRepository;

        // system user configs
        _systemUser.Id = new Guid(config["SystemUserId"]);
        _systemUser.AccountId = new Guid(config["KymetaAccountId"]);
    }

    public async Task<Tuple<Account, string>> AddAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;
        // Verify exists
        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount != null) // If account exists, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} already exists in the system.";
            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var billingAddress = model.Addresses?.FirstOrDefault(a => a.Type == "Billing & Shipping"); // This string is a picklist value in SF
        var account = await RemapSalesforceAccountToOssAccount(model.Name, model.ObjectId, existingUser.Id, null, model.ParentId, billingAddress);
        try
        {
            var added = await _accountsClient.AddAccount(account);
            if (!string.IsNullOrEmpty(added.Item2)) 
            {
                error = $"There was an error adding the account to OSS: {added.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Successful, added.Item1.Id.ToString());
            return new Tuple<Account, string>(added.Item1, string.Empty);
        } catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Tuple<Account, string>> UpdateAccount(SalesforceAccountModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var account = await RemapSalesforceAccountToOssAccount(model.Name, model.ObjectId, existingUser.Id, null, model.ParentId);
        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Tuple<Account, string>> UpdateAccountAddress(UpdateAddressModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateAddressInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(model.ParentAccountId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ParentAccountId} does not exist in OSS.";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAddressInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var account = RemapSalesforceAddressToOssAccount(model.Address1, model.Address2, model.Country, existingUser.Id);
        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.UpdateAddressInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateAddressInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAddressInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Tuple<Account, string>> UpdateAccountOracleId(SalesforceAccountModel model, string oracleId, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        try
        {
            // construct the payload for the Account update
            var accountChanges = new Account
            {
                ModifiedById = existingUser.Id,
                ModifiedOn = DateTime.UtcNow,
                OracleAccountId = oracleId
            };

            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), accountChanges);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account oracle Id in OSS: {updated.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Account> GetAccountBySalesforceId(string salesforceId)
    {
        return await _accountsClient.GetAccountBySalesforceId(salesforceId);
    }

    private async Task<Account> RemapSalesforceAccountToOssAccount(
        string name,
        string salesforceId,
        Guid userId,
        string? oracleAccountId = null,
        string? salesforceParentId = null,
        SalesforceAddressModel? billingAddress = null)
    {
        var account = new Account
        {
            SalesforceAccountId = salesforceId,
            Enabled = true,
            Name = name,
            Origin = CreatedOriginEnum.SF,
            CreatedById = userId,
            ModifiedById = userId,
            OracleAccountId = oracleAccountId,
            ParentId = _config.GetValue<Guid>("KymetaAccountId"),
            BillingAddressLine1 = billingAddress?.Address1,
            BillingAddressLine2 = billingAddress?.Address2,
            BillingCity = billingAddress?.City,
            BillingPostalCode = billingAddress?.PostalCode,
            BillingState = billingAddress?.StateProvince,
            BillingCountryCode = billingAddress?.Country,
        };
        // Overwrite the default Kymeta ID if the parent Id is present
        if (!string.IsNullOrEmpty(salesforceParentId))
        {
            var parentAccount = await _accountsClient.GetAccountBySalesforceId(salesforceParentId);
            if (parentAccount != null) account.ParentId = parentAccount.Id;
        }

        return account;
    }

    private Account RemapSalesforceAddressToOssAccount(
        string address1,
        string address2,
        string country,
        Guid userId
        )
    {
        var account = new Account
        {
            ModifiedById = userId,
            BillingAddressLine1 = address1,
            BillingAddressLine2 = address2,
            BillingCountryCode = country
        };

        return account;
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
}