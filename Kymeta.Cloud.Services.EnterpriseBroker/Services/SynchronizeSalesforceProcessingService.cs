namespace Kymeta.Cloud.Services.EnterpriseBroker.Services
{
    internal interface ISalesforceProcessingService
    {
        Task SynchronizeProducts(CancellationToken stoppingToken);
    }

    public class SalesforceProcessingService : ISalesforceProcessingService
    {
        private readonly IConfiguration _config;
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IProductsBrokerService _productsBrokerService;

        public SalesforceProcessingService(IConfiguration config, ILogger<SalesforceProcessingService> logger, IProductsBrokerService productsBrokerService)
        {
            _config = config;
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
                // fetch synchronize interval from config
                var synchronizeProductsInterval = _config["SynchronizeProductsIntervalHours"];
                // error if no config value is present
                if (string.IsNullOrEmpty(synchronizeProductsInterval)) throw new Exception("Missing 'SynchronizeProductsInterval' configuration value.");
                // parse the config value into int data type
                var isConfigParsed = int.TryParse(synchronizeProductsInterval, out int hoursFromConfig);
                // take hours value from config for timespan, default interval to 1 hour if not able to parse config value
                var delayInterval = TimeSpan.FromHours(isConfigParsed ? hoursFromConfig : 1);
                // delay execution by specified interval
                await Task.Delay(delayInterval, stoppingToken);
            }
        }
    }
}
