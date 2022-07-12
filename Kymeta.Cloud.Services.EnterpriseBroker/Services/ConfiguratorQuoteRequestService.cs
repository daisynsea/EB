using Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services
{
    public interface IConfiguratorQuoteRequestService
    {
        Task<string> InsertQuoteRequest(QuoteRequestViewModel model);
    }

    public class ConfiguratorQuoteRequestService : IConfiguratorQuoteRequestService
    {
        private readonly IQuotesRepository _quotesRepository;
        public ConfiguratorQuoteRequestService(IQuotesRepository quotesRepository)
        {
            _quotesRepository = quotesRepository;
        }

        public async Task<string> InsertQuoteRequest(QuoteRequestViewModel model)
        {
            model.Id = Guid.NewGuid();
            var documents = await _quotesRepository.GetQuoteRecords();
            var latestOrderId = documents?.FirstOrDefault()?.OrderId ?? 10000;
            model.OrderId = latestOrderId + 1;
            model.CreatedOn = DateTime.UtcNow;
            var document =await _quotesRepository.InsertQuoteRecord(model);
            var orderId = document?.OrderId;
            return Convert.ToString(orderId);
        }
    }
}
