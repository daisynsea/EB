using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class SetSalesOrderWithOracleActivity : AsyncTaskActivity<OracleSalesforceSyncRequest, bool>
{
    private readonly ILogger<SetSalesOrderWithOracleActivity> _logger;
    private readonly ISalesforceRestClient _salesforceRestClient;

    public SetSalesOrderWithOracleActivity(ILogger<SetSalesOrderWithOracleActivity> logger, ISalesforceRestClient salesforceRestClient)
    {
        _logger = logger;
        _salesforceRestClient = salesforceRestClient;
    }

    protected override async Task<bool> ExecuteAsync(TaskContext context, OracleSalesforceSyncRequest input)
    {
        var response = await _salesforceRestClient.SyncFromOracle(input, default);
        return response.IsSuccess();
    }
}