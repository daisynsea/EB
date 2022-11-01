using Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
[ExcludeFromCodeCoverage]
public class BrokerProductsController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<BrokerProductsController> _logger;
    private readonly ISalesforceProductsRepository _sfProductsRepo;
    private readonly IProductsBrokerService _sfProductBrokerService;
    private readonly IFileStorageService _fileStorageService;

    public BrokerProductsController(IConfiguration config, ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo, IProductsBrokerService sfProductsBrokerService, IFileStorageService fileStorageService)
    {
        _config = config;
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
        _sfProductBrokerService = sfProductsBrokerService;
        _fileStorageService = fileStorageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Models.Salesforce.External.SalesforceProductObjectModel>>> GetProducts()
    {
        try
        {
            var result = await _sfProductsRepo.GetProducts();
            return result?.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching products from Cosmos DB due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [MapToApiVersion("2")]
    [HttpGet]
    public async Task<ActionResult<List<SalesforceProductObjectModel>>> GetProductsV2()
    {
        try
        {
            // TODO: check redis for hash key, if not present, then trigger sync
            // TODO: continue

            // TODO: if Products are not available from Redis & cannot rebuild cache hash key then return error to configurator

            // TODO: fetch asset references for Products from REDIS
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching Products due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    // TODO: change to "synchronize products" endpoint which includes file fetching/sync
    [HttpGet("sync")]
    public async Task<ActionResult<List<string>>> SynchronizeProductsFromSalesforce()
    {
        try
        {
            // fetch the active Products from the Product Report
            var productsReportResult = await _sfProductBrokerService.GetSalesforceProductReport();
            if (productsReportResult == null) return new BadRequestObjectResult($"An error occurred while attempting to fetch the Products report.");

            var productsReport = productsReportResult.ToList();
            // isolate the Ids from the report objects
            var productIds = productsReport.Select(pr => pr.Id);
            if (productIds == null || !productIds.Any()) return new BadRequestObjectResult($"No products contained in the Product Report from Salesforce.");

            // fetch existing assets blob storage
            var existingBlobAssets = await _fileStorageService.GetBlobs("KymetaCloudCdn", _config["AzureStorage:Accounts:KymetaCloudCdn"], _config["AzureStorage:Containers:CdnAssets"]);

            // fetch all the uploaded assets (Related Files) for the Products & upload them to blob storage
            var productsRelatedFiles = await _sfProductBrokerService.GetProductsRelatedFiles(productIds);
            if (productsRelatedFiles.HasErrors)
            {
                _logger.LogCritical($"Error fetching Salesforce Products related files.", productsRelatedFiles.Results);
                return new BadRequestObjectResult(new { Message = $"Unable to fetch Salesforce Products related files.", Results = productsRelatedFiles.Results });
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
                if (!string.IsNullOrEmpty(file.Result.Name) && !file.Result.Name.ToLower().Contains("catalogimg"))
                {
                    continue;
                }

                // concat full filename for comparison
                var fileName = $"{file.Result.Name}.{file.Result.FileExtension}";
                // check if blob exist & compare modified date
                var existingBlobMatch = existingBlobAssets.FirstOrDefault(blob => blob.FileName == fileName);
                // include the file only if it does not exist or has a modified date newer than what is currently in blob storage
                if (existingBlobMatch == null || existingBlobMatch.ModifiedOn < file.Result.ModifiedDate)
                {
                    filesToUpload.Add(file.Result);
                }
            }

            // upload the files that either do not exist or are newer versions than what blob storage contains
            var assetsUploaded = await _sfProductBrokerService.UploadSalesforceAssetFiles(filesToUpload);


            // clean up assets that no longer remain in Salesforce
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
                    var path = blob.Uri.Substring(imagesIndex);
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



            return new JsonResult(assetsUploaded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching Product files from Salesforce due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("report")]
    public async Task<ActionResult<List<Models.Salesforce.External.SalesforceProductObjectModel>>> GetProductReport()
    {
        try
        {
            var result = await _sfProductBrokerService.GetSalesforceProductReport();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching Products Report from SF due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}