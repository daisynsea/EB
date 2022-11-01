using Kymeta.Cloud.Services.EnterpriseBroker.Models;
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

    public BrokerProductsController(IConfiguration config, ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo, IProductsBrokerService sfProductsBrokerService)
    {
        _config = config;
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
        _sfProductBrokerService = sfProductsBrokerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SalesforceProductObjectModel>>> GetProducts()
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

    /// <summary>
    /// Synchronize Products (including file assets) with OSS persistence
    /// </summary>
    /// <returns></returns>
    [HttpGet("sync")]
    public async Task<ActionResult<List<SalesforceProductObjectModel>>> SynchronizeProductsFromSalesforce()
    {
        try
        {
            var productsSynchronized = await _sfProductBrokerService.SynchronizeProducts();
            if (productsSynchronized == null || !productsSynchronized.Any()) return new BadRequestObjectResult($"Encountered an error while attempting to synchronize Products from Salesforce.");

            return productsSynchronized.ToList();
        }
        catch (SynchronizeProductsException spex)
        {
            // catch errors specific to the Synchronization operation
            _logger.LogError(spex.Message);
            return new BadRequestObjectResult(spex.Message);
        }
        catch (Exception ex)
        {
            // unknown error/exception
            _logger.LogError(ex, $"Error fetching Product files from Salesforce due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("report")]
    public async Task<ActionResult<List<SalesforceProductObjectModel>>> GetProductReport()
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