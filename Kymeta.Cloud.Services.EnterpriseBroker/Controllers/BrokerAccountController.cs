using Microsoft.AspNetCore.Mvc;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/account")]
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
    [HttpPost]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccount([FromBody] CreateAccountModel model)
    {
        try
        {
            var result = await _accountBrokerService.ProcessAccountCreate(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing create account action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// This endpoint accepts just the account object
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<UpdateAccountResponse>> UpdateAccount([FromBody] UpdateAccountModel model)
    {
        try
        {
            var result = await _accountBrokerService.ProcessAccountUpdate(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing update account action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}