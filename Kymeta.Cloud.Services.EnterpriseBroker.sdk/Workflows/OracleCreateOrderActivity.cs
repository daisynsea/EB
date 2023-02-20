using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows
{
    public class OracleCreateOrderActivity: AsyncTaskActivity<SalesforceOrder, OracleResponse<CreateOrderResponse>>
    {
        private readonly IOracleRestClient _client;
        private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
        public OracleCreateOrderActivity(IOracleRestClient client, ILogger<UpdateOracleSalesOrderActivity> logger) 
        {
            _client = client;
            _logger = logger;
        }

        protected override async Task<OracleResponse<CreateOrderResponse>> ExecuteAsync(TaskContext context, SalesforceOrder input)
        {
            OracleCreateOrder oracleModel = MapToOracle(input);
            _logger.LogInformation($"Map Salesforce Order {JsonConvert.SerializeObject(input)} to Oracle Order {JsonConvert.SerializeObject(oracleModel)}.");

            var result =  await _client.CreateOrder(oracleModel, CancellationToken.None);
            _logger.LogInformation($"Oracle CreateOrder response: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        private OracleCreateOrder MapToOracle(SalesforceOrder input)
        {
            throw new NotImplementedException();
        }
    }
}
