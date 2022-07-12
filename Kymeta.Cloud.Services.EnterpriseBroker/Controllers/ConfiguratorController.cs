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

    public ConfiguratorController(ILogger<ConfiguratorController> logger, IConfiguratorQuoteRequestService quoteRequestService)
    {
        _logger = logger;
        _quoteRequestService = quoteRequestService;
    }

    [HttpPost("createquoterecord"), AllowAnonymous]
    public async Task<ActionResult<string>> CreateQuoteOrderId([FromBody] QuoteRequestViewModel model)
    {
        try
        {
            var result = await _quoteRequestService.InsertQuoteRequest(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing address action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}