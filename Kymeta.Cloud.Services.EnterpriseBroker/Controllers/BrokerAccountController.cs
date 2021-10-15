using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/broker/account")]
public class BrokerAccountController : ControllerBase
{
    private readonly ILogger<BrokerAccountController> _logger;
    private readonly IAccountBrokerService _accountBrokerService;

    public BrokerAccountController(ILogger<BrokerAccountController> logger, IAccountBrokerService accountBrokerService)
    {
        _logger = logger;
        _accountBrokerService = accountBrokerService;
    }

    [HttpPost]
    public async Task<ActionResult<SalesforceProcessResponse>> ProcessAccountAction([FromBody] SalesforceActionObject model)
    {
        try
        {
            var result = await _accountBrokerService.ProcessSalesforceAction(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing account action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}