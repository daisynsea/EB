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
    Task<Account> AddAccount(Account account);
}

public class OssService : IOssService
{
    public Task<Account> AddAccount(Account account)
    {
        throw new NotImplementedException();
    }
}