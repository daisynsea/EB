using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class GetOracleSalesOrderActivity : TaskActivity<string, OracleOrder>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    public GetOracleSalesOrderActivity(ILogger<GetSalesOrderLinesActivity> logger) => _logger = logger;

    protected override OracleOrder Execute(TaskContext context, string input)
    {
        throw new NotImplementedException();
    }
}
