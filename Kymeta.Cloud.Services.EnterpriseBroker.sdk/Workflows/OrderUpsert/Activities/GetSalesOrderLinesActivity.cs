using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.OrderUpsert.Activities;

public class GetSalesOrderLinesActivity : AsyncTaskActivity<string, IList<SalesOrderLineItems>>
{
    private readonly ISalesforceRestClient _client;
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    public GetSalesOrderLinesActivity(ISalesforceRestClient client, ILogger<GetSalesOrderLinesActivity> logger)
    {
        _client = client;
        _logger = logger;
    }

   
    protected override async Task<IList<SalesOrderLineItems>> ExecuteAsync(TaskContext context, string input)
    {
        SalesOrderLineResponse response = await _client.GetOrderProducts(input, default);
        return response.Records.ToList();
    }
   
}
