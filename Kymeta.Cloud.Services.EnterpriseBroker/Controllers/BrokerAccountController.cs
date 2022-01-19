using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/account")]
[ExcludeFromCodeCoverage]
public class BrokerAccountController : ControllerBase
{
    private readonly ILogger<BrokerAccountController> _logger;
    private readonly IAccountBrokerService _accountBrokerService;

    public BrokerAccountController(ILogger<BrokerAccountController> logger, IAccountBrokerService accountBrokerService)
    {
        _logger = logger;
        _accountBrokerService = accountBrokerService;
    }

    /// <summary>
    /// This endpoint accepts a payload including Contacts and Addresses
    /// </summary>
    /// <param name="model">Incoming Payload</param>
    /// <returns>Response model</returns>
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UnifiedResponse>> ProcessAccount([FromBody] SalesforceAccountModel model)
    {
        try
        {
            var result = await _accountBrokerService.ProcessAccountAction(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing create account action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}