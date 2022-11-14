namespace Kymeta.Cloud.Services.EnterpriseBroker.Services
{
    internal interface ISalesforceProcessingService
    {
        Task SynchronizeProducts(CancellationToken stoppingToken);
    }

    public class SalesforceProcessingService : ISalesforceProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IProductsBrokerService _productsBrokerService;

        public SalesforceProcessingService(ILogger<SalesforceProcessingService> logger, IProductsBrokerService productsBrokerService)
        {
            _logger = logger;
            _productsBrokerService = productsBrokerService;
        }

        public async Task SynchronizeProducts(CancellationToken stoppingToken)
        {
            // prevent execution if background service is stopping
            while (!stoppingToken.IsCancellationRequested)
            {
                // increment count
                executionCount++;
                _logger.LogInformation("Synchronize Products is working. Count: {Count}", executionCount);
                // init the synchronization
                await _productsBrokerService.SynchronizeProducts();
                // delay execution by specified interval
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
