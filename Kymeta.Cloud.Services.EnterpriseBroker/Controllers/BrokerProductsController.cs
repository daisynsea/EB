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
    private readonly ISalesforceClient _salesforceClient;
    private readonly IFileStorageClient _fileStorageClient;

    public BrokerProductsController(IConfiguration config, ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo, IProductsBrokerService sfProductsBrokerService, ISalesforceClient salesforceClient, IFileStorageClient fileStorageClient)
    {
        _config = config;
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
        _sfProductBrokerService = sfProductsBrokerService;
        _salesforceClient = salesforceClient;
        _fileStorageClient = fileStorageClient;
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
            // fetch the active Products from the Product Report
            var productsReportResult = await _sfProductBrokerService.GetSalesforceProductReport();
            if (productsReportResult == null) return new BadRequestObjectResult($"An error occurred while attempting to fetch the Products report.");

            var productsReport = productsReportResult.ToList();
            var productIds = productsReport.Select(pr => pr.Id);



            return productsReportResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching products from Salesforce due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("files")]
    public async Task<ActionResult<List<string>>> GetProductFiles()
    {
        try
        {
            // fetch the active Products from the Product Report
            var productsReportResult = await _sfProductBrokerService.GetSalesforceProductReport();
            if (productsReportResult == null) return new BadRequestObjectResult($"An error occurred while attempting to fetch the Products report.");

            var productsReport = productsReportResult.ToList();
            // isolate the Ids from the report objects
            var productIds = productsReport.Select(pr => pr.Id);
            if (productIds == null || !productIds.Any()) return NotFound();

            // fetch all the uploaded assets (Related Files) for the Products & upload them to blob storage
            var assetsUploaded = await _sfProductBrokerService.GetLatestProductFiles(productIds);

            #region Files (Related List)
            //// fetch Product files
            //var productRelatedFiles = await _salesforceClient.GetRelatedFiles(productIds);
            //if (productRelatedFiles == null)
            //{
            //    _logger.LogError($"Error fetching related files.");
            //    return new BadRequestObjectResult($"Unable to fetch related files for Products.");
            //}
            //// map files to simplified Tuple containing ProductId and ContentDocumentId (FileId)
            //var productRelatedFileIds = productRelatedFiles.Records.Select(pf => new Tuple<string, string>(pf.LinkedEntityId, pf.ContentDocumentId));
            //// flatten to just a list of file Ids
            //var fileIdsIsolated = productRelatedFileIds.Select(pfi => pfi.Item2);
            //// fetch file metadata for all files
            //var fileMetadataResult = await _salesforceClient.GetFileMetadataByManyIds(fileIdsIsolated);
            //var assetsUploaded = new List<string>();
            //// verify we received file metadata
            //if (fileMetadataResult != null)
            //{
            //    // iterate through file results
            //    foreach (var file in fileMetadataResult.Results)
            //    {
            //        // check for any errors
            //        if (file.StatusCode != 200 || file.Result == null)
            //        {
            //            _logger.LogError($"Error fetching file metadata. [{file.Result?.ErrorCode}] : {file.Result?.Message}");
            //            // skip this file
            //            continue;
            //        }
            //        // locate matching productId
            //        var productId = productRelatedFileIds.FirstOrDefault(pfi => pfi.Item2 == file.Result.Id)?.Item1;
            //        // bypass if productId is not found... should never happen
            //        if (productId == null) continue;
            //        // acquire specific product from report
            //        var product = productsReport.FirstOrDefault(pr => pr.Id == productId);
            //        // bypass if product is not found... should never happen
            //        if (product == null) continue;

            //        // download file data
            //        using var fileContent = await _salesforceClient.DownloadFileContent(file.Result.DownloadUrl);
            //        Console.WriteLine(fileContent);

            //        // upload only catalogimg assets
            //        if (file.Result.Name.Contains("catalogimg"))
            //        {
            //            // call FileStorage service to upload to blob storage account for CDN
            //            var fileName = $"{file.Result.Name}.{file.Result.FileExtension}";
            //            var isFileUploaded = await _fileStorageClient.UploadBlobFile(fileContent, fileName, _config["AzureStorage:Accounts:KymetaCloudCdn"], "assets", "images");
            //            Console.WriteLine(isFileUploaded);

            //            // append reference to product.Assets for image paths
            //            var assetPath = $"/assets/images/{fileName}";
            //            if (product.Assets == null)
            //            {
            //                product.Assets = new List<string> { assetPath };
            //            }
            //            else
            //            {
            //                product.Assets.Append(assetPath);
            //            }
            //            assetsUploaded.Add(assetPath);
            //        }
            //    }
            //}
            #endregion

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