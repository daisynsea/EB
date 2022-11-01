using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories
{
    public interface ICacheRepository
    {
        void AddProduct(SalesforceProductObjectModel model);
        void UpdateProduct(SalesforceProductObjectModel model);
        SalesforceProductObjectModel GetProduct(string productId);
        IEnumerable<SalesforceProductObjectModel> GetProducts();
        void DeleteProducts(IEnumerable<string> productIds);
        void ClearCacheCompletely();
        void SetProducts(IEnumerable<SalesforceProductObjectModel> products);
    }
    public class CacheRepository : ICacheRepository
    {
        private IRedisClient _redisClient;
        private const string _SALESFORCE_PRODUCTS_KEY = "SalesforceProducts";

        public CacheRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public void AddProduct(SalesforceProductObjectModel model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }

        public void ClearCacheCompletely()
        {
            _redisClient.KeyRemove(_SALESFORCE_PRODUCTS_KEY);
        }

        public void DeleteProducts(IEnumerable<string> productIds)
        {
            _redisClient.HashRemoveFields(_SALESFORCE_PRODUCTS_KEY, productIds.ToArray());
        }

        public SalesforceProductObjectModel GetProduct(string productId)
        {
            return _redisClient.HashGet<SalesforceProductObjectModel>(_SALESFORCE_PRODUCTS_KEY, productId);
        }

        public IEnumerable<SalesforceProductObjectModel> GetProducts()
        {
            var dictionary = _redisClient.HashGetAll<string, SalesforceProductObjectModel>(_SALESFORCE_PRODUCTS_KEY);
            return dictionary?.Values;
        }

        public void SetProducts(IEnumerable<SalesforceProductObjectModel> products)
        {
            var dictionary = products.ToDictionary(x => x.Id, y => y);
            _redisClient.HashSet(_SALESFORCE_PRODUCTS_KEY, dictionary);
        }

        public void UpdateProduct(SalesforceProductObjectModel model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }
    }
}
