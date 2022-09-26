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
    private readonly ILogger<BrokerProductsController> _logger;
    private readonly ISalesforceProductsRepository _sfProductsRepo;
    private readonly IProductsBrokerService _sfProductBrokerService;
    private readonly ISalesforceClient _salesforceClient;

    public BrokerProductsController(ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo, IProductsBrokerService sfProductsBrokerService, ISalesforceClient salesforceClient)
    {
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
        _sfProductBrokerService = sfProductsBrokerService;
        _salesforceClient = salesforceClient;
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
            // fetch Product metadata for all Products in report
            var salesforceProducts = await _salesforceClient.GetProductsByManyIds(productIds);
            // if no products, return empty list
            if (salesforceProducts == null || !salesforceProducts.Any()) return new List<SalesforceProductObjectModel>();

            // fetch image data (using URL from salesforceProducts ItemDetails__c)
            // TODO: this feels VERY hacky.... prone to errors depending on how images are placed in rich text field
            foreach (var product in salesforceProducts)
            {
                var itemDetails = product.ItemDetails;
                if (itemDetails == null) continue;
                Console.WriteLine(itemDetails);
                
                // parse out the refId so we can send request to fetch image data
                var refIdIndexes = Helpers.AllIndexesOf(itemDetails, "refid");
                var refIds = new List<string>();
                foreach (var refIdx in refIdIndexes)
                {
                    // substring, starting at beginning of refid value, offset by the `refid=` text (6 chars)
                    var refSub = itemDetails.Substring(refIdx + 6);
                    var idEndIndex = refSub.IndexOf("\"");
                    // identify the refId for the image
                    var refId = refSub.Substring(0, idEndIndex);
                    Console.WriteLine($"refId: {refId}");
                    refIds.Add(refId);

                    // extract the alt text value (file name)
                    // TODO: OR - use the productId and just name them product.Id (with version appended)
                    var altIndex = refSub.IndexOf("alt");
                    var altSub = refSub.Substring(altIndex + 5);
                    var altEndIndex = altSub.IndexOf("\"");
                    var altValue = altSub.Substring(0, altEndIndex);
                    Console.WriteLine($"alt: {altValue}");
                }
                Console.WriteLine($"refIds: {refIds}");
            }

            
            
            // TODO: OR - PROVE THIS OUT
            // TODO: fetch related files based on Product Ids (limit to 100 per request)
            // TODO: fetch file data for each file






            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching products from Cosmos DB due to an exception: {ex.Message}");
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
