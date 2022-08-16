using Kymeta.Cloud.Logging.Activity;
using Newtonsoft.Json;

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
    /// Update an existing child account to OSS
    /// </summary>
    /// <param name="account"></param>
    /// <param name="transaction"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateChildAccount(Account account, string userName, SalesforceActionTransaction transaction);
    /// <summary>
    /// Utility method to update a list of child Accounts in OSS
    /// </summary>
    /// <param name="existingAccount"></param>
    /// <param name="children"></param>
    /// <param name="userName"></param>
    /// <param name="salesforceTransaction"></param>
    /// <returns></returns>
    Task<Tuple<bool, List<Account>?, string>> UpdateChildAccounts(Account existingAccount, SalesforceAccountModel model, SalesforceActionTransaction salesforceTransaction);
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
    /// <summary>
    /// Get a list of OSS Accounts by many Salesforce Ids
    /// </summary>
    /// <param name="salesforceIds">List of strings which are Salesforce object Ids</param>
    /// <returns></returns>
    Task<List<Account>> GetAccountsByManySalesforceIds(List<string> salesforceIds);
}

public class OssService : IOssService
{
    private readonly IConfiguration _config;
    private readonly IAccountsClient _accountsClient;
    private readonly IUsersClient _usersClient;
    private readonly IActionsRepository _actionsRepository;
    private readonly IActivityLoggerClient _activityLoggerClient;
    private User _systemUser = new()
    { 
        FirstName = "System", 
        LastName = "User", 
        Email = "kcssystemuser@kymeta.io" 
    };

    public OssService(IConfiguration config, IAccountsClient accountsClient, IUsersClient usersClient, IActionsRepository actionsRepository, IActivityLoggerClient activityLoggerClient)
    {
        _config = config;
        _accountsClient = accountsClient;
        _usersClient = usersClient;
        _actionsRepository = actionsRepository;

        // system user configs
        _systemUser.Id = new Guid(config["SystemUserId"]);
        _systemUser.AccountId = new Guid(config["KymetaAccountId"]);
        _activityLoggerClient = activityLoggerClient;
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
        var account = await RemapSalesforceAccountToOssAccount(model.Name, model.ObjectId, existingUser.Id, null, model.ParentId, model.Pricebook, model.VolumeTier);
        try
        {
            // add the Account
            var addedAccount = await _accountsClient.AddAccount(account);
            if (!string.IsNullOrEmpty(addedAccount.Item2) || !addedAccount.Item1.Id.HasValue) 
            {
                error = $"There was an error adding the Account to OSS: {addedAccount.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }

            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Successful, addedAccount.Item1.Id.ToString());
            await _activityLoggerClient.AddActivity(ActivityEntityTypes.Accounts, existingUser.Id, existingUser.FullName, addedAccount.Item1.Id, $"{addedAccount.Item1.Name}", "addaccount", null, null, JsonConvert.SerializeObject(addedAccount));

            // create an Account Admin role for the account
            var addedRole = await _usersClient.AddRole(new Role { AccountId = addedAccount.Item1.Id.Value, Name = $"{addedAccount.Item1.Name} Account Admin", Description = "Owner of the account. Has all permissions" });
            if (addedRole == null)
            {
                error = $"There was an error adding the Account role.";
                await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }

            await _activityLoggerClient.AddActivity(ActivityEntityTypes.Users, existingUser.Id, existingUser.FullName, addedRole.Id, addedRole.Name, "addrole", null, null, JsonConvert.SerializeObject(addedRole));

            // add all permissions to this role
            var permissions = await _usersClient.GetPermissions(null);
            // if the system user was used, assign default permissions
            if ((existingUser.Permissions == null || !existingUser.Permissions.Any()) && existingUser.Email == "kcssystemuser@kymeta.io")
            {
                // reduce the list to only default Permissions
                existingUser.Permissions = permissions
                    .Where(p => !p.ExcludeFromDefaultRoles)
                    .Select(p => p.Shortcode);
            }
            // everything user has access to, except system admin
            var permissionsToAdd = permissions?
                .Where(p => existingUser.Permissions != null && existingUser.Permissions.Contains(p.Shortcode))
                ?.Where(p => p.Shortcode != "SYSTEM_ADMIN")
                ?.Select(p => p.Id)
                ?.ToList();
            // add the permissions
            var updatedRole = await _usersClient.EditRolePermissions(addedRole.Id, permissionsToAdd);
            if (updatedRole == null)
            {
                error = $"There was an error establishing the Account permissions.";
                await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<Account, string>(null, error);
            }

            return new Tuple<Account, string>(addedAccount.Item1, string.Empty);
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

        // Get the user from OSS system or default to system user when not found
        User existingUser = await _usersClient.GetUserByEmail(model.UserName) ?? _systemUser;

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

    public async Task<Tuple<Account, string>> UpdateChildAccount(Account account, string userName, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(account.SalesforceAccountId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with OSS ID {account.Id} does not exist in OSS.";
            return new Tuple<Account, string>(null, error);
        }
        
        // get the user from OSS system or default to system user when not found
        User existingUser = await _usersClient.GetUserByEmail(userName) ?? _systemUser;
        account.ModifiedById = existingUser.Id;

        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                if (transaction != null) await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Error, existingAccount.Id.ToString(), error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            if (transaction != null) await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Error, existingAccount.Id.ToString(), error);
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

    public async virtual Task<Account> GetAccountBySalesforceId(string salesforceId)
    {
        return await _accountsClient.GetAccountBySalesforceId(salesforceId);
    }

    public async virtual Task<List<Account>> GetAccountsByManySalesforceIds(List<string> salesforceIds)
    {
        return await _accountsClient.GetAccountsByManySalesforceIds(salesforceIds);
    }

    public async virtual Task<Account> RemapSalesforceAccountToOssAccount(
        string name,
        string salesforceId,
        Guid userId,
        string? oracleAccountId = null,
        string? salesforceParentId = null,
        string? priceBook = null,
        string? volumeTier = null
        )
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
            CommercialPriceBook = priceBook?.Contains("CPB"),
            MilitaryPriceBook = priceBook?.Contains("MPB"),
            DiscountTier = GetDiscountTierFromSalesforceAPIValue(volumeTier)
        };
        // Overwrite the default Kymeta ID if the parent Id is present
        if (!string.IsNullOrEmpty(salesforceParentId))
        {
            var parentAccount = await _accountsClient.GetAccountBySalesforceId(salesforceParentId);
            if (parentAccount != null) account.ParentId = parentAccount.Id;
        }

        return account;
    }

    public int? GetDiscountTierFromSalesforceAPIValue(string? salesforceApiValue)
    {
        if (string.IsNullOrEmpty(salesforceApiValue)) return 1;
        if (!salesforceApiValue.Contains("(")) return 1;
        string firstPartOfValue = salesforceApiValue.Split('(')[0];
        if (firstPartOfValue == null) return 1;
        var justTheDigit = new string(firstPartOfValue.Where(char.IsDigit).ToArray());
        if (justTheDigit.Length == 0) return 1;
        return Convert.ToInt32(justTheDigit);
    }

    public async virtual Task LogAction(SalesforceActionTransaction transaction, SalesforceTransactionAction action, ActionObjectType objectType, StatusType status, string? entityId = null, string? errorMessage = null)
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

    public async virtual Task<Tuple<bool, List<Account>?, string>> UpdateChildAccounts(Account existingAccount, SalesforceAccountModel model, SalesforceActionTransaction salesforceTransaction)
    {
        // fetch the child Account metadata from OSS
        var childAccountSalesforceIds = model.ChildAccounts.Select(ca => ca.ObjectId).ToList();
        var childAccounts = await GetAccountsByManySalesforceIds(childAccountSalesforceIds);

        List<Task<Tuple<Account, string>>> updateAccountTasks = new();
        // iterate the childAccounts & check to see that OSS parent references are pointed to ossAccountId
        for (int i = 0; i < childAccounts.Count; i++)
        {
            var childAccount = childAccounts[i];
            if (childAccount.ParentId != existingAccount.Id)
            {
                // if parent reference is not correct, update child account
                childAccount.ParentId = existingAccount.Id;
                // add to list of Tasks to update children in parallel 
                updateAccountTasks.Add(UpdateChildAccount(childAccount, model.UserName, salesforceTransaction)); // need a method that only updates account and doesn't have context of the transaction
            }
        }

        List<Account> updatedChildrenAccounts = new();
        Tuple<bool, List<Account>?, string> result = new(true, updatedChildrenAccounts, null);
        // check to see if we need to update any children
        if (updateAccountTasks.Any())
        {
            // execute requests to create Locations in async fashion
            await Task.WhenAll(updateAccountTasks);
            // get the response data
            var updateChildAccountResults = updateAccountTasks.Select(t => t.Result).ToList();
            for (int i = 0; i < updateChildAccountResults.Count; i++)
            {
                // fetch child update result
                var childUpdateResult = updateChildAccountResults[i];
                if (childUpdateResult.Item1 == null)
                {
                    // something went wrong updating a child, return the error message
                    return new Tuple<bool, List<Account>?, string>(false, null, childUpdateResult.Item2);
                }
                updatedChildrenAccounts.Add(childUpdateResult.Item1);
            }
        }

        // everything was updated as we intend, return success
        return new Tuple<bool, List<Account>?, string>(true, updatedChildrenAccounts, null);
    }
}