using Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/configurator")]
[ExcludeFromCodeCoverage]
public class ConfiguratorController : ControllerBase
{
    private readonly ILogger<ConfiguratorController> _logger;
    private readonly IConfiguratorQuoteRequestService _quoteRequestService;
    private readonly ISalesforceRepository _salesforceRepository;

    public ConfiguratorController(ILogger<ConfiguratorController> logger, IConfiguratorQuoteRequestService quoteRequestService, ISalesforceRepository salesforceRepository)
    {
        _logger = logger;
        _quoteRequestService = quoteRequestService;
        _salesforceRepository = salesforceRepository;
    }

    [HttpPost("createquoterecord"), AllowAnonymous]
    public async Task<ActionResult<string>> CreateOrderQuote([FromBody] QuoteRequestViewModel model)
    {
        try
        {
            var result = await _quoteRequestService.InsertQuoteRequest(model);
            if(string.IsNullOrEmpty(result)) return new BadRequestObjectResult("Error occured creating quote record to db.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating quote request record to db, due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("metadata"), AllowAnonymous]
    public async Task<ActionResult<ConfiguratorMetadata>> GetConfiguratorMetadata()
    {
        try
        {
            var metadata = await _salesforceRepository.GetConfiguratorMetadata();
            return metadata;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating quote request record to db, due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}