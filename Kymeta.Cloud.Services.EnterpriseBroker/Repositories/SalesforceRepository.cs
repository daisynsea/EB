/*
 * Delete this file later when we're actually pulling from Salesforce instead of CosmosDB
 * 
 */

using Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface ISalesforceRepository
{
    Task<IEnumerable<SalesforceProductObjectModel>> GetProducts();
    Task<ConfiguratorMetadata> GetConfiguratorMetadata();
}

public class SalesforceRepository : ISalesforceRepository
{
    public Container Container { get; }

    private ISalesforceClient _salesforceClient;

    public PartitionKey ResolvePartitionKey(string objectType) => new PartitionKey(objectType);

    public SalesforceRepository(
            IConfiguration config,
            CosmosClient cosmosClient,
            ISalesforceClient salesforceClient)
    {
        string databaseName = config["AzureDocumentDB:KCSDatabase"] ?? "KymetaCloudServices";
        string containerName = config["AzureDocumentDB:ConfiguratorProducts"] ?? "Products";
        Container = cosmosClient.GetContainer(databaseName, containerName);
        _salesforceClient = salesforceClient;
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

    public async Task<ConfiguratorMetadata> GetConfiguratorMetadata()
    {
        var result = await _salesforceClient.GetSalesforceConfiguratorMetadataModel();
        if (result == null) return null;

        // TODO: Transform
        var model = new ConfiguratorMetadata();
        model.Rules = result.Terminal_Builder_Rules__c;
        if (!string.IsNullOrEmpty(result.Osprey_Spec_Sheet__c)) model.OspreySpecSheet = JsonSerializer.Deserialize<IEnumerable<SpecSheetRow>>(result.Osprey_Spec_Sheet__c, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return model;
    }
}
