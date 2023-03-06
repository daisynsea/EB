﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Model;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert;

public class SalesOrderOrchestration : TaskOrchestration<bool, string>
{
    private readonly ILogger<SalesOrderOrchestration> _logger;
    public SalesOrderOrchestration(ILogger<SalesOrderOrchestration> logger) => _logger = logger;

    public override async Task<bool> RunTask(OrchestrationContext context, string input)
    {
        using var ls = _logger.LogEntryExit();

        _logger.LogInformation(
            "Starting orchestration, replaying={replaying}, InstanceId={instanceId}, ExceutionId={executionId}",
            context.IsReplaying,
            context.OrchestrationInstance.InstanceId,
            context.OrchestrationInstance.ExecutionId
            );

        RetryOptions options = GetRetryOptions();

        try
        {
            SalesforceNeoApproveOrderModel eventData = input.ToObject<SalesforceNeoApproveOrderModel>().NotNull();
            string? orderKey = eventData.MapToOracleCreateOrder().OrderKey;

            OracleResponse<GetOrderResponse> oracleOrderResponse = await context.ScheduleTask<OracleResponse<GetOrderResponse>>(typeof(GetOracleSalesOrderActivity), options, orderKey);
            List<SalesOrderLineItems> salesforceOrderLines = await context.ScheduleTask<List<SalesOrderLineItems>>(typeof(GetSalesOrderLinesActivity), options, orderKey);

            if (!oracleOrderResponse.IsSuccessStatusCode())
            {
                var orderToCreate = eventData.MapToOracleCreateOrderWithLines(salesforceOrderLines);
                OracleSalesOrderResponseModel<CreateOrderResponse> response = await context.ScheduleTask<OracleSalesOrderResponseModel<CreateOrderResponse>>(typeof(OracleCreateOrderActivity), options,orderToCreate);
                bool success = await context.ScheduleTask<bool>(typeof(SetSalesOrderWithOracleActivity), options, orderToCreate); //fix
            }
            else
            {
                var orderLatestRevision = oracleOrderResponse.Payload.FindLatestRevision();
                var orderToUpdate = eventData.MapToOracleUpdateOrder(orderLatestRevision); // add lines
                OracleSalesOrderResponseModel<UpdateOrderResponse> response = await context.ScheduleTask<OracleSalesOrderResponseModel<UpdateOrderResponse>>(typeof(UpdateOracleSalesOrderActivity), options, );
                bool success = await context.ScheduleTask<bool>(typeof(SetSalesOrderWithOracleActivity), options, orderToUpdate); //fix
            }


            _logger.LogInformation("Completed orchestration");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Orchestration failed");
            return false;
        }

        return true;
    }

    private RetryOptions GetRetryOptions()
    {
        var firstRetryInterval = TimeSpan.FromSeconds(1);
        var maxNumberOfAttempts = 5;
        var backoffCoefficient = 1.1;

        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
        {
            BackoffCoefficient = backoffCoefficient,
            Handle = HandleError
        };
        return options;
    }

    private bool HandleError(Exception ex)
    {
        _logger.LogError(ex, "Orchestration failed");
        return true;
    }
}


public static class SalesOrderOrchestrationExtensions
{
    public static TaskHubWorker RegisterSalesOrderActivities(this TaskHubWorker worker, IServiceProvider serviceProvider)
    {
        worker.AddTaskOrchestrations(typeof(SalesOrderOrchestration));

        worker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(GetSalesOrderLinesActivity), serviceProvider));
        worker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(SetSalesOrderWithOracleActivity), serviceProvider));
        worker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(UpdateOracleSalesOrderActivity), serviceProvider));
        return worker;
    }
}
