using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories
{
    public interface ICacheRepository
    {
        void AddProduct(SalesforceProductObjectModelV2 model);
        void UpdateProduct(SalesforceProductObjectModelV2 model);
        SalesforceProductObjectModelV2 GetProduct(string productId);
        IEnumerable<SalesforceProductObjectModelV2> GetProducts();
        void DeleteProducts(IEnumerable<string> productIds);
        void ClearProductsCacheCompletely();
        void SetProducts(IEnumerable<SalesforceProductObjectModelV2> products);
        void SetSalesforceEventReplayId(string field, string replayId);
    }
    public class CacheRepository : ICacheRepository
    {
        private IRedisClient _redisClient;
        private const string _SALESFORCE_PRODUCTS_KEY = "EB:SalesforceProducts";

        public CacheRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public void AddProduct(SalesforceProductObjectModelV2 model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }

        public void ClearProductsCacheCompletely()
        {
            _redisClient.KeyRemove(_SALESFORCE_PRODUCTS_KEY);
        }

        public void DeleteProducts(IEnumerable<string> productIds)
        {
            _redisClient.HashRemoveFields(_SALESFORCE_PRODUCTS_KEY, productIds.ToArray());
        }

        public SalesforceProductObjectModelV2 GetProduct(string productId)
        {
            return _redisClient.HashGet<SalesforceProductObjectModelV2>(_SALESFORCE_PRODUCTS_KEY, productId);
        }

        public IEnumerable<SalesforceProductObjectModelV2> GetProducts()
        {
            var dictionary = _redisClient.HashGetAll<string, SalesforceProductObjectModelV2>(_SALESFORCE_PRODUCTS_KEY);
            return dictionary?.Values;
        }

        public void SetProducts(IEnumerable<SalesforceProductObjectModelV2> products)
        {
            var dictionary = products.ToDictionary(x => x.Id, y => y);
            _redisClient.HashSet(_SALESFORCE_PRODUCTS_KEY, dictionary);
        }

        public void UpdateProduct(SalesforceProductObjectModelV2 model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }

        public void SetSalesforceEventReplayId(string field, string replayId)
        {
            _redisClient.HashSetField("EB:PlatformEvents:ReplayIds", field, replayId);
        }
    }
}
