﻿using Kymeta.Cloud.Services.EnterpriseBroker.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IProductsBrokerService
{
    Task<IEnumerable<SalesforceProductObjectModel>> SynchronizeProducts();
    Task<List<SalesforceProductObjectModel>> GetSalesforceProductReport();
    Task<SalesforceFileResponseModel> GetProductsRelatedFiles(IEnumerable<string> productIds);
    Task<List<string>> UploadSalesforceAssetFiles(List<SalesforceFileObjectModel> salesforceFiles);
}

public class ProductsBrokerService : IProductsBrokerService
{
    private readonly IConfiguration _config;
    private readonly ILogger<ProductsBrokerService> _logger;
    private readonly ISalesforceClient _salesforceClient;
    private readonly IFileStorageClient _fileStorageClient;
    private readonly IFileStorageService _fileStorageService;

    public ProductsBrokerService(IConfiguration config, ILogger<ProductsBrokerService> logger, ISalesforceClient salesforceClient, IFileStorageClient fileStorageClient, IFileStorageService fileStorageService)
    {
        _config = config;
        _logger = logger;
        _salesforceClient = salesforceClient;
        _fileStorageClient = fileStorageClient;
        _fileStorageService = fileStorageService;
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

    public async Task<IEnumerable<SalesforceProductObjectModel>> SynchronizeProducts()
    {
        // fetch the active Products from the Product Report
        var productsReportResult = await GetSalesforceProductReport();
        if (productsReportResult == null) throw new SynchronizeProductsException($"An error occurred while attempting to fetch the Products report.");

        var products = productsReportResult.ToList();
        // isolate the Ids from the report objects
        var productIds = products.Select(pr => pr.Id);
        if (productIds == null || !productIds.Any()) throw new SynchronizeProductsException($"No products contained in the Product Report from Salesforce.");
        
        // fetch existing assets blob storage
        var existingBlobAssets = await _fileStorageService.GetBlobs("KymetaCloudCdn", _config["AzureStorage:Accounts:KymetaCloudCdn"], _config["AzureStorage:Containers:CdnAssets"]);

        // fetch all the uploaded assets (Related Files) for the Products & upload them to blob storage
        var productsRelatedFiles = await GetProductsRelatedFiles(productIds);
        if (productsRelatedFiles.HasErrors)
        {
            _logger.LogCritical($"Error fetching Salesforce Products related files.", productsRelatedFiles.Results);
            throw new SynchronizeProductsException($"Unable to fetch Salesforce Products related files.");
        }

        // iterate through Salesforce file metadata results
        var filesToUpload = new List<SalesforceFileObjectModel>();
        foreach (var file in productsRelatedFiles.Results)
        {
            if (file.StatusCode != 200 || file.Result == null)
            {
                _logger.LogError($"There was a problem fetching file metadata from Salesforce.", file.Result);
                continue;
            }

            // bypass files that aren't `catalogimg` because they are not relevant
            if (!string.IsNullOrEmpty(file.Result.Name) && !file.Result.Name.ToLower().Contains("catalogimg")) continue;

            // concat full filename for comparison
            var fileName = $"{file.Result.Name}.{file.Result.FileExtension}";
            // check if blob exist & compare modified date
            var existingBlobMatch = existingBlobAssets.FirstOrDefault(blob => blob.FileName == fileName);
            // include the file only if it does not exist or has a modified date newer than what is currently in blob storage
            if (existingBlobMatch == null || existingBlobMatch.ModifiedOn < file.Result.ModifiedDate)
            {
                filesToUpload.Add(file.Result);
                // proceed to next file
                continue;
            }

            // append the existing asset path to the product match (when applicable)
            var assetPathIdx = existingBlobMatch.Uri.IndexOf("asset");
            // using range, fetch the substring from the given index
            var assetPathSimple = existingBlobMatch.Uri[assetPathIdx..];
            AppendAssetReferenceToProduct(ref products, assetPathSimple);
        }

        // upload the files that either do not exist or are newer versions than what blob storage contains
        var assetsUploaded = await UploadSalesforceAssetFiles(filesToUpload);
        // iterate successful uploads
        foreach (var assetPath in assetsUploaded)
        {
            // append the asset path to the product match (when applicable)
            AppendAssetReferenceToProduct(ref products, assetPath);
        }

        // clean up blob storage assets that no longer exist in Salesforce
        var blobsToDelete = new List<FileItem>();
        foreach (var blob in existingBlobAssets)
        {
            // search for match from Salesforce results
            var salesforceFileMatch = productsRelatedFiles.Results.FirstOrDefault(file =>
            {
                var fileName = $"{file.Result.Name}.{file.Result.FileExtension}";
                return blob.FileName == fileName;
            });

            // if no match is found, append the file to the list to be deleted
            if (salesforceFileMatch == null) blobsToDelete.Add(blob);
        }

        // check to see if we have any items to delete from blob storage
        if (blobsToDelete.Any())
        {
            // extract blob path from fully qualified URI
            var blobPaths = blobsToDelete.Select(blob => {
                var imagesIndex = blob.Uri.IndexOf("images");
                // using range, fetch the substring from the given index
                var path = blob.Uri[imagesIndex..];
                return path;
            });
            // delete the blobs from Azure Storage
            var cleanupResult = await _fileStorageService.DeleteBlobs("KymetaCloudCdn", _config["AzureStorage:Accounts:KymetaCloudCdn"], _config["AzureStorage:Containers:CdnAssets"], blobPaths);
            if (!cleanupResult)
            {
                // an error occurred with one or more deletions, log critical error to prompt an investigation
                _logger.LogCritical($"Encountered an error while attempting to delete a file from blob storage.");
            }
        }



        // TODO: add to Redis all the Product Metadata & their asset references (Blob storage (CDN))



        return products;
    }

    public async Task<SalesforceFileResponseModel> GetProductsRelatedFiles(IEnumerable<string> productIds)
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

        return fileMetadataResult;
    }

    public async Task<List<string>> UploadSalesforceAssetFiles(List<SalesforceFileObjectModel> salesforceFiles)
    {
        var assetsUploaded = new List<string>();
        // verify we received file metadata
        if (salesforceFiles == null || !salesforceFiles.Any()) return assetsUploaded;

        // iterate through file results
        foreach (var file in salesforceFiles)
        {
            if (string.IsNullOrEmpty(file.DownloadUrl))
            {
                _logger.LogError($"Download URL missing for file: [{file.Name}]");
                // skip this file
                continue;
            }

            // download file data
            using var fileContent = await _salesforceClient.DownloadFileContent(file.DownloadUrl);
            if (fileContent == null)
            {
                _logger.LogError($"Error fetching file content for `{file.Name}` with URL `{file.DownloadUrl}`.");
                // skip this file
                continue;
            }

            // upload only catalogimg assets
            if (!string.IsNullOrEmpty(file.Name) && file.Name.Contains("catalogimg"))
            {
                // call FileStorage service to upload to blob storage account for CDN
                var fileName = $"{file.Name}.{file.FileExtension}";
                var isFileUploaded = await _fileStorageClient.UploadBlobFile(fileContent, fileName, _config["AzureStorage:Accounts:KymetaCloudCdn"], "assets", "images");
                Console.WriteLine(isFileUploaded);

                if (!isFileUploaded)
                {
                    // file failed to upload to blob storage
                    _logger.LogCritical($"Failed to upload file '{fileName}' to blob storage (CDN).");
                    // continue to process remaining files
                    continue;
                }

                // append reference to response to indicate file was uploaded to Blob storage (CDN)
                var assetPath = $"/assets/images/{fileName}";
                assetsUploaded.Add(assetPath);
            }
        }

        return assetsUploaded;
    }

    private void AppendAssetReferenceToProduct(ref List<SalesforceProductObjectModel> products, string assetPath)
    {
        var fileNameIndexStart = assetPath.LastIndexOf('/');
        var fileName = assetPath.Substring(fileNameIndexStart);
        // separate the segments of the file name by `.`
        var nameSegments = fileName.Split('.');
        // search the segments for an Id that matches the KPC format
        var productKpc = nameSegments.FirstOrDefault(ns => Regex.IsMatch(ns, @"[0-9a-zA-z]{5,}-\d{5,}-\d$"));
        // if no KPC was found, skip the asset
        if (string.IsNullOrEmpty(productKpc)) return;

        var productMatch = products.FirstOrDefault(p => productKpc == p.ProductCode);
        // validate that we have a matching product in our result set, otherwise skip the asset
        if (productMatch == null) return;

        // we found a matching product, append the asset reference to the product object
        if (productMatch.Assets == null)
        {
            productMatch.Assets = new List<string> { assetPath };
        } 
        else
        {
            productMatch.Assets.Append(assetPath);
        }
    }
}