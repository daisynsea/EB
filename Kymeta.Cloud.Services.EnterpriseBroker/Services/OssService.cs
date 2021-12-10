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
    Task<Tuple<Account, string>> AddAccount(CreateAccountModel model, string oracleAccountId, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing account to OSS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Tuple<Account, string>> UpdateAccount(UpdateAccountModel model, SalesforceActionTransaction transaction);
    Task<Tuple<Account, string>> UpdateAccountAddress(UpdateAddressModel model, SalesforceActionTransaction transaction);
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

    public async Task<Tuple<Account, string>> AddAccount(CreateAccountModel model, string oracleAccountId, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction.Id, SalesforceTransactionAction.CreateAccountInOss, StatusType.Started, transaction.Action);
        string error = null;
        // Verify exists
        var existingAccount = await GetAccount(model.ObjectId);
        if (existingAccount != null) // If account exists, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} already exists in the system.";
            await LogAction(transaction.Id, SalesforceTransactionAction.CreateAccountInOss, StatusType.Error, transaction.Action, null, error);
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
        var account = RemapSalesforceAccountToOssAccount(model.Name, model.ObjectId, existingUser.Id, oracleAccountId, model.ParentId, billingAddress);
        try
        {
            var added = await _accountsClient.AddAccount(account);
            if (!string.IsNullOrEmpty(added.Item2)) 
            {
                error = $"There was an error adding the account to OSS: {added.Item2}";
                await LogAction(transaction.Id, SalesforceTransactionAction.CreateAccountInOss, StatusType.Error, transaction.Action, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction.Id, SalesforceTransactionAction.CreateAccountInOss, StatusType.Successful, transaction.Action, added.Item1.Id.ToString());
            return new Tuple<Account, string>(added.Item1, string.Empty);
        } catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction.Id, SalesforceTransactionAction.CreateAccountInOss, StatusType.Error, transaction.Action, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Tuple<Account, string>> UpdateAccount(UpdateAccountModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAccountInOss, StatusType.Started, transaction.Action);
        string error = null;

        var existingAccount = await GetAccount(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.";
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAccountInOss, StatusType.Error, transaction.Action, null, error);
            return new Tuple<Account, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var account = RemapSalesforceAccountToOssAccount(model.Name, model.ObjectId, existingUser.Id, null, model.ParentId);
        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAccountInOss, StatusType.Error, transaction.Action, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAccountInOss, StatusType.Successful, transaction.Action, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAccountInOss, StatusType.Error, transaction.Action, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Tuple<Account, string>> UpdateAccountAddress(UpdateAddressModel model, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAddressInOss, StatusType.Started, transaction.Action);
        string error = null;

        var existingAccount = await GetAccount(model.ParentAccountId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ParentAccountId} does not exist in OSS.";
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAddressInOss, StatusType.Error, transaction.Action, null, error);
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
                await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAddressInOss, StatusType.Error, transaction.Action, null, error);
                return new Tuple<Account, string>(null, error);
            }
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAddressInOss, StatusType.Successful, transaction.Action, updated.Item1.Id.ToString());
            return new Tuple<Account, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction.Id, SalesforceTransactionAction.UpdateAddressInOss, StatusType.Error, transaction.Action, null, error);
            return new Tuple<Account, string>(null, error);
        }
    }

    public async Task<Account> GetAccount(string salesforceId)
    {
        return await _accountsClient.GetAccountBySalesforceId(salesforceId);
    }

    private Account RemapSalesforceAccountToOssAccount(
        string name,
        string salesforceId,
        Guid userId,
        string? oracleAccountId = null,
        string? salesforceParentId = null,
        CreateAddressModel? billingAddress = null)
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
            account.ParentId = Guid.Parse(salesforceParentId);
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

    private async Task LogAction(Guid transactionId, SalesforceTransactionAction action, StatusType status, ActionType? transactionAction, string? entityId = null, string? errorMessage = null)
    {
        await _actionsRepository.AddTransactionRecord(transactionId, transactionAction?.ToString() ?? "Unset", new SalesforceActionRecord
        {
            Action = action,
            Status = status,
            Timestamp = DateTime.UtcNow,
            ErrorMessage = errorMessage,
            EntityId = entityId
        });
    }
}