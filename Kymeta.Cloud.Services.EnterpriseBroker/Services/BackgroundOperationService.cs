using Microsoft.Extensions.Hosting;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services
{
    public class BackgroundOperationService : BackgroundService
    {
        private readonly ILogger<BackgroundOperationService> _logger;
        public IServiceProvider Services { get; }

        public BackgroundOperationService(IServiceProvider services, ILogger<BackgroundOperationService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"Background Operation Service is starting.");
                // init the sync from Salesforce to Kymeta Cloud (OSS)
                await SynchronizeProducts(stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Background Operation execution cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Execution stopping due to an unhandeled exception.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Operation Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        /// <summary>
        /// Synchronize "Product" objects from Salesforce to our Cloud Redis cache
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task SynchronizeProducts(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Synchronize Products is working.");

            using IServiceScope scope = Services.CreateScope();
            var sfProcessingService = scope.ServiceProvider.GetRequiredService<ISalesforceProcessingService>();
            await sfProcessingService.SynchronizeProducts(stoppingToken);
        }
    }
}
