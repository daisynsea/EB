/*
 * Delete this file later when we're actually pulling from Salesforce instead of CosmosDB
 * 
 */

using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.Azure.Cosmos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface ISalesforceProductsRepository
{
    Task<IEnumerable<SalesforceProductObjectModel>> GetProducts();
}

public class SalesforceProductsRepository : ISalesforceProductsRepository
{
    public Container Container { get; }
    public PartitionKey ResolvePartitionKey(string objectType) => new PartitionKey(objectType);

    public SalesforceProductsRepository(
            IConfiguration config,
            CosmosClient cosmosClient)
    {
        string databaseName = config["AzureDocumentDB:KCSDatabase"] ?? "KymetaCloudServices";
        string containerName = config["AzureDocumentDB:ConfiguratorProducts"] ?? "Products";
        Container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<SalesforceProductObjectModel>> GetProducts()
    {
        var sqlQueryText = "SELECT * FROM c";

        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        FeedIterator<SalesforceProductObjectModel> queryResultSetIterator = this.Container.GetItemQueryIterator<SalesforceProductObjectModel>(queryDefinition);

        List<SalesforceProductObjectModel> records = new List<SalesforceProductObjectModel>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<SalesforceProductObjectModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (SalesforceProductObjectModel record in currentResultSet)
            {
                records.Add(record);
            }
        }

        return records;
    }
}
