using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities;

public class GetOracleSalesOrderActivity : AsyncTaskActivity<string, OracleResponse<GetOrderResponse>>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public GetOracleSalesOrderActivity(ILogger<GetSalesOrderLinesActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleResponse<GetOrderResponse>> ExecuteAsync(TaskContext context, string input)
    {
        return await _oracleRestClient.GetOrder(input, default);
    }
}
