using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public interface ISalesforceSubscriptionService
{
    Task<(bool isSuccess, string? error)> ProcessSubscriptionEvent(SalesforceSubscriptionEventPayload payload);
}

public class SalesforceSubscriptionService : ISalesforceSubscriptionService
{
    public readonly IConfiguration _config;
    public readonly ILogger<SalesforceSubscriptionService> _logger;
    public readonly ITerminalsClient _terminalsClient;

    public SalesforceSubscriptionService(IConfiguration config, ILogger<SalesforceSubscriptionService> logger, ITerminalsClient terminalsClient)
    {
        _config = config;
        _logger = logger;
        _terminalsClient = terminalsClient;
    }

    public async Task<(bool isSuccess, string? error)> ProcessSubscriptionEvent(SalesforceSubscriptionEventPayload payload)
    {

        return new ValueTuple<bool, string?>(true, null);
    }
}
