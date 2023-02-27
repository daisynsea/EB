using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class UpdateOracleSalesOrderActivity : AsyncTaskActivity<SalesforceNeoApproveOrderPayload, OracleSalesOrderResponseModel>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleSalesOrderResponseModel> ExecuteAsync(TaskContext context, SalesforceNeoApproveOrderPayload input)
    {
        if (!input.IsValid())
        {
            throw new InvalidOperationException("Please provide valid sales order!");
        }
      
        OracleUpdateOrder oracleOrder = input.MapToOracleUpdate();
        await _oracleRestClient.UpdateOrder(input.NEO_Id__c,oracleOrder, default);
        return new OracleSalesOrderResponseModel();
    }

}
