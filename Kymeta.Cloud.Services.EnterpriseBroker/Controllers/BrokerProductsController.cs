using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
[ExcludeFromCodeCoverage]
public class BrokerProductsController : ControllerBase
{
    private readonly ILogger<BrokerProductsController> _logger;
    private readonly ISalesforceProductsRepository _sfProductsRepo;

    public BrokerProductsController(ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo)
    {
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
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
}
