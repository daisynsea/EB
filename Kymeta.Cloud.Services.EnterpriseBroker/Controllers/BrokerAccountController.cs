using Microsoft.AspNetCore.Mvc;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/broker/account")]
public class BrokerAccountController : ControllerBase
{
    private ILogger<BrokerAccountController> _logger;

    public BrokerAccountController(ILogger<BrokerAccountController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> ProcessSalesforceAccountAction()
    {
        return Ok();
    }
}