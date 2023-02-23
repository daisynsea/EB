using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class GetOracleSalesOrderActivity : AsyncTaskActivity<string, OracleOrder>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public GetOracleSalesOrderActivity(ILogger<GetSalesOrderLinesActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleOrder> ExecuteAsync(TaskContext context, string input)
    {
        var order = await _oracleRestClient.GetOrder(input, default);
        return new OracleOrder(); 
    }

}
