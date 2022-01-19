using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/contact")]
[ExcludeFromCodeCoverage]
public class BrokerContactController : ControllerBase
{
    private readonly ILogger<BrokerContactController> _logger;
    private readonly IContactBrokerService _contactService;

    public BrokerContactController(ILogger<BrokerContactController> logger, IContactBrokerService contactService)
    {
        _logger = logger;
        _contactService = contactService;
    }

    /// <summary>
    /// This endpoint accepts a contact Payload
    /// </summary>
    /// <param name="model">Incoming Payload</param>
    /// <returns>Response model</returns>
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<ContactResponse>> ProcessContact([FromBody] SalesforceContactModel model)
    {
        try
        {
            var result = await _contactService.ProcessContactAction(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing contact action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}