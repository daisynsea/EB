using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities
{
    public class OracleCreateOrderActivity : AsyncTaskActivity<OracleCreateOrder, OracleSalesOrderResponseModel<CreateOrderResponse>>
    {
        private readonly IOracleRestClient _client;
        private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
        public OracleCreateOrderActivity(IOracleRestClient client, ILogger<UpdateOracleSalesOrderActivity> logger)
        {
            _client = client;
            _logger = logger;
        }

        protected override async Task<OracleSalesOrderResponseModel<CreateOrderResponse>> ExecuteAsync(TaskContext context, OracleCreateOrder input)
        {
            _logger.LogInformation($" Oracle Order {JsonConvert.SerializeObject(input)}.");

            OracleResponse<CreateOrderResponse> orderResponse = await _client.CreateOrder(input, CancellationToken.None);
            _logger.LogInformation($"Oracle CreateOrder response: {JsonConvert.SerializeObject(orderResponse)}");

            if (orderResponse.IsSuccessStatusCode())
            {
                return new OracleSalesOrderResponseModel<CreateOrderResponse>
                {
                    IntegrationStatus = IntegrationConstants.Success,
                    IntergrationError = IntegrationConstants.Clear,
                    ResponseModel = orderResponse.Payload
                };
            }
            return new OracleSalesOrderResponseModel<CreateOrderResponse>
            {
                IntegrationStatus = IntegrationConstants.Failure,
                IntergrationError = orderResponse.ErrorMessage,
                ResponseModel = null
            };
        }
    }
}
