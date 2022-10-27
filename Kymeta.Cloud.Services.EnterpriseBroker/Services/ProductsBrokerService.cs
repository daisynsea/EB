using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IProductsBrokerService
{
    Task<List<SalesforceProductObjectModel>> GetSalesforceProductReport();
    Task<List<string>> GetLatestProductFiles(IEnumerable<string> productIds);
}

public class ProductsBrokerService : IProductsBrokerService
{
    private readonly IConfiguration _config;
    private readonly ILogger<ProductsBrokerService> _logger;
    private readonly ISalesforceClient _salesforceClient;
    private readonly IFileStorageClient _fileStorageClient;

    public ProductsBrokerService(IConfiguration config, ILogger<ProductsBrokerService> logger, ISalesforceClient salesforceClient, IFileStorageClient fileStorageClient)
    {
        _config = config;
        _logger = logger;
        _salesforceClient = salesforceClient;
        _fileStorageClient = fileStorageClient;
    }

    public async Task<List<SalesforceProductObjectModel>> GetSalesforceProductReport()
    {
        var productResults = new List<SalesforceProductObjectModel>();
        var productReport = await _salesforceClient.GetProductReportFromSalesforce();
        // validate that we received data from Salesforce, if not return null to throw error
        if (productReport == null) return null;
        var rowDataCells = productReport.factMap?.TT?.rows?.ToList();
        var reportColumns = productReport.reportMetadata?.detailColumns?.ToList();
        // find index to get labels and values for rowDataCells
        var indexOfProductCode          = reportColumns?.FindIndex(x => x == "CUSTOMER_PRODUCT_ID");
        var indexOfStage                = reportColumns?.FindIndex(x => x == "Product2.Stage__c");
        var indexOfProductName          = reportColumns?.FindIndex(x => x == "PRODUCT_NAME");
        var indexOfProductGen           = reportColumns?.FindIndex(x => x == "Product2.Product_Gen__c");
        var indexOfProductType          = reportColumns?.FindIndex(x => x == "Product2.ProductType__c");
        var indexOfProductFamily        = reportColumns?.FindIndex(x => x == "FAMILY");
        var indexOfTerminalCategory     = reportColumns?.FindIndex(x => x == "Product2.Terminal_Category__c");
        var indexOfPriceBookName        = reportColumns?.FindIndex(x => x == "NAME");
        var indexOfListPrice            = reportColumns?.FindIndex(x => x == "UNIT_PRICE");
        var indexOfItemDetail           = reportColumns?.FindIndex(x => x == "Product2.ItemDetails__c");
        var indexOfProductDesc          = reportColumns?.FindIndex(x => x == "Product2.cpqProductDescription__c");

        var reportData = new List<SalesforceReportViewModel>();
        for (int i = 0; i < rowDataCells?.Count; i++)
        {
            var row = rowDataCells[i];
            var productCode         = row?.dataCells[indexOfProductCode.GetValueOrDefault()]?.label;
            var recordId            = row?.dataCells[indexOfProductCode.GetValueOrDefault()]?.recordId;
            var stage               = row?.dataCells[indexOfStage.GetValueOrDefault()]?.label;            
            var productName         = row?.dataCells[indexOfProductName.GetValueOrDefault()]?.label;      
            var productGen          = row?.dataCells[indexOfProductGen.GetValueOrDefault()]?.label;       
            var productType         = row?.dataCells[indexOfProductType.GetValueOrDefault()]?.label;      
            var productFamily       = row?.dataCells[indexOfProductFamily.GetValueOrDefault()]?.label;    
            var terminalCategory    = row?.dataCells[indexOfTerminalCategory.GetValueOrDefault()]?.label; 
            var priceBookName       = row?.dataCells[indexOfPriceBookName.GetValueOrDefault()]?.label;    
            var listPrice           = row?.dataCells[indexOfListPrice.GetValueOrDefault()]?.label;        
            var itemDetail          = row?.dataCells[indexOfItemDetail.GetValueOrDefault()]?.label;       
            var productDesc         = row?.dataCells[indexOfProductDesc.GetValueOrDefault()]?.label;      

            var listPriceObj = row?.dataCells[indexOfListPrice.GetValueOrDefault()]?.value;

            listPrice = Convert.ToString(((JValue)((JProperty)((JContainer)listPriceObj).First).Value).Value);
            reportData.Add(new SalesforceReportViewModel
            {
                RecordId            = Convert.ToString(recordId),
                ProductCode         = Convert.ToString(productCode),
                Stage               = Convert.ToString(stage),
                ProductName         = Convert.ToString(productName),
                ProductGen          = Convert.ToString(productGen),
                ProductType         = Convert.ToString(productType),
                ProductFamily       = Convert.ToString(productFamily),
                TerminalCategory    = Convert.ToString(terminalCategory),
                PriceBookName       = Convert.ToString(priceBookName),
                ListPrice           = Convert.ToString(listPrice),
                ItemDetail          = Convert.ToString(itemDetail),
                ProductDesc         = Convert.ToString(productDesc)
            });

        }

        var productFamilies = new List<string> { "Accessories", "Components", "Cables" };
        var products = reportData.Where(r => productFamilies.Contains(r.ProductFamily))?.ToList();
        if (products == null) return productResults;
        
        // isolate the products into distinct elements in the list
        var distinctProducts = products.DistinctBy(a => a.ProductCode).ToList();
        foreach (var reportProduct in distinctProducts)
        {
            if (reportProduct == null) continue;
            // there will be two records for product code, with Wholesale price, MSRP price
            var wholesalePrice = products
                .Where(a => a?.PriceBookName == "Wholesale" && a?.ProductCode == reportProduct.ProductCode)?
                .Select(a => a?.ListPrice)?
                .FirstOrDefault();
            var isWholesalePriceInt = int.TryParse(wholesalePrice, out int wholesalePriceInt);
            var msrpPrice = products
                .Where(a => a?.PriceBookName == "MSRP" && a?.ProductCode == reportProduct.ProductCode)?
                .Select(a => a?.ListPrice)?
                .FirstOrDefault();
            var isMsrpPriceInt = int.TryParse(msrpPrice, out int msrpPriceInt);
            var kitStr = products
                .Where(a => a?.ProductCode == reportProduct.ProductCode)
                .Select(a => a?.ProductDesc)?
                .FirstOrDefault();
            productResults.Add(new Models.Salesforce.External.SalesforceProductObjectModel
            {
                Id = reportProduct.RecordId,
                ProductCode = reportProduct.ProductCode,
                Name = reportProduct.ProductName,
                Description = reportProduct.ProductDesc,
                WholesalePrice = wholesalePriceInt,
                MsrpPrice = msrpPriceInt,
                ProductType = reportProduct.ProductType,
                ProductFamily = reportProduct.ProductFamily,
                Kit = kitStr?.Split(",")
            });
        }

        return productResults;
    }

    public async Task<List<string>> GetLatestProductFiles(IEnumerable<string> productIds)
    {
        // fetch Product files
        var productRelatedFiles = await _salesforceClient.GetRelatedFiles(productIds);
        if (productRelatedFiles == null)
        {
            _logger.LogError($"Error fetching related files for Products.", productIds);
            throw new BadHttpRequestException($"Unable to fetch related files for Products.");
        }
        // map files to simplified Tuple containing ProductId and ContentDocumentId (FileId)
        var productRelatedFileIds = productRelatedFiles.Records.Select(pf => new Tuple<string, string>(pf.LinkedEntityId, pf.ContentDocumentId));
        // flatten to just a list of file Ids
        var fileIdsIsolated = productRelatedFileIds.Select(pfi => pfi.Item2);
        // fetch file metadata for all files
        var fileMetadataResult = await _salesforceClient.GetFileMetadataByManyIds(fileIdsIsolated);
        var assetsUploaded = new List<string>();
        // verify we received file metadata
        if (fileMetadataResult != null)
        {
            // iterate through file results
            foreach (var file in fileMetadataResult.Results)
            {
                // check for any errors
                if (file.StatusCode != 200 || file.Result == null)
                {
                    _logger.LogError($"Error fetching file metadata. [{file.Result?.ErrorCode}] : {file.Result?.Message}");
                    // skip this file
                    continue;
                }

                // download file data
                using var fileContent = await _salesforceClient.DownloadFileContent(file.Result.DownloadUrl);
                if (fileContent == null)
                {
                    _logger.LogError($"Error fetching file content for `{file.Result.Name}` with URL `{file.Result.DownloadUrl}`.");
                    // skip this file
                    continue;
                }

                // upload only catalogimg assets
                if (file.Result.Name.Contains("catalogimg"))
                {
                    // call FileStorage service to upload to blob storage account for CDN
                    var fileName = $"{file.Result.Name}.{file.Result.FileExtension}";
                    var isFileUploaded = await _fileStorageClient.UploadBlobFile(fileContent, fileName, _config["AzureStorage:Accounts:KymetaCloudCdn"], "assets", "images");
                    Console.WriteLine(isFileUploaded);

                    // append reference to response to indicate file was uploaded to Blob storage (CDN)
                    var assetPath = $"/assets/images/{fileName}";
                    assetsUploaded.Add(assetPath);
                }
            }
        }

        return assetsUploaded;
    }
}