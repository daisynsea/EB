using Microsoft.AspNetCore.Mvc;
namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/contact")]
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
    [HttpPost]
    public async Task<ActionResult<CreateContactResponse>> CreateContact([FromBody] CreateContactModel model)
    {
        try
        {
            var result = await _contactService.CreateContact(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing create contact action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// This endpoint accepts a contact Payload
    /// </summary>
    /// <param name="model">Incoming Payload</param>
    /// <returns>Response model</returns>
    [HttpPut]
    public async Task<ActionResult<UpdateContactResponse>> UpdateContact([FromBody] UpdateContactModel model)
    {
        try
        {
            var result = await _contactService.UpdateContact(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing update contact action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}