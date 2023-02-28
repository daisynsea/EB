using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows
{
    public class OracleCreateOrderActivity: AsyncTaskActivity<OracleCreateOrder, OracleResponse<CreateOrderResponse>>
    {
        private readonly IOracleRestClient _client;
        private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
        public OracleCreateOrderActivity(IOracleRestClient client, ILogger<UpdateOracleSalesOrderActivity> logger) 
        {
            _client = client;
            _logger = logger;
        }

        protected override async Task<OracleResponse<CreateOrderResponse>> ExecuteAsync(TaskContext context, OracleCreateOrder input)
        {
            _logger.LogInformation($" Oracle Order {JsonConvert.SerializeObject(input)}.");

            var result =  await _client.CreateOrder(input, CancellationToken.None);
            _logger.LogInformation($"Oracle CreateOrder response: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
