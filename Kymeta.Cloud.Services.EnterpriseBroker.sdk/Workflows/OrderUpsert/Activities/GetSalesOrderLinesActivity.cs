using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Model;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities;

public class GetSalesOrderLinesActivity : TaskActivity<SalesforceNeoApproveOrderModel, SalesOrderModel>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    public GetSalesOrderLinesActivity(ILogger<GetSalesOrderLinesActivity> logger) => _logger = logger;

    protected override SalesOrderModel Execute(TaskContext context, SalesforceNeoApproveOrderModel input)
    {
        return new SalesOrderModel();
    }
}
