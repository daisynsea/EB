﻿using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class UpdateOracleSalesOrderActivity : AsyncTaskActivity<OracleUpdateOrder, OracleSalesOrderResponseModel>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleSalesOrderResponseModel> ExecuteAsync(TaskContext context, OracleUpdateOrder input)
    {
        if (!input.IsValid())
        {
            throw new InvalidOperationException("Please provide valid sales order!");
        }

        OracleResponse<UpdateOrderResponse> orderResponse = await _oracleRestClient.UpdateOrder(input, default);
        if (orderResponse.IsSuccessStatusCode())
        {
            return new OracleSalesOrderResponseModel
            {
                IntegrationStatus = IntegrationConstants.Success,
                IntergrationError = IntegrationConstants.Clear,
                OracleSalesOrderId = orderResponse.Payload.HeaderId.ToString()
            };
        }

        return new OracleSalesOrderResponseModel
        {
            IntegrationStatus = IntegrationConstants.Failure,
            IntergrationError = orderResponse.ErrorMessage,
            OracleSalesOrderId = input.OrderKey
        };
    }

}
