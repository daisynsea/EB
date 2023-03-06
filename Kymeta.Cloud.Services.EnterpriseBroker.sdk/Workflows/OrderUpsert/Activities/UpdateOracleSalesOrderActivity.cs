using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities;

public class UpdateOracleSalesOrderActivity : AsyncTaskActivity<OracleUpdateOrder, OracleSalesOrderResponseModel<UpdateOrderResponse>>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleSalesOrderResponseModel<UpdateOrderResponse>> ExecuteAsync(TaskContext context, OracleUpdateOrder input)
    {
        if (!input.IsValid())
        {
            throw new InvalidOperationException("Please provide valid sales order!");
        }

        OracleResponse<UpdateOrderResponse> orderResponse = await _oracleRestClient.UpdateOrder(input, default);
        if (orderResponse.IsSuccessStatusCode())
        {
            return new OracleSalesOrderResponseModel<UpdateOrderResponse>
            {
                IntegrationStatus = IntegrationConstants.Success,
                IntergrationError = IntegrationConstants.Clear,
                ResponseModel = orderResponse.Payload
            };
        }

        return new OracleSalesOrderResponseModel<UpdateOrderResponse>
        {
            IntegrationStatus = IntegrationConstants.Failure,
            IntergrationError = orderResponse.ErrorMessage,
            ResponseModel = null
        };
    }

}
