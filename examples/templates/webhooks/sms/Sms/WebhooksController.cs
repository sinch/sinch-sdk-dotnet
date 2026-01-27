using Microsoft.AspNetCore.Mvc;
using Sinch.SMS.Hooks;

namespace SmsWebhookTemplate.Sms;

[ApiController]
[Route("webhooks/sms")]
public class WebhooksController : ControllerBase
{
    private readonly ISmsWebhooks _webhooks;
    private readonly ServerBusinessLogic _service;
    private readonly IConfiguration _configuration;

    public WebhooksController(ISmsWebhooks webhooks, ServerBusinessLogic service, IConfiguration configuration)
    {
        _webhooks = webhooks;
        _service = service;
        _configuration = configuration;
    }

    [HttpPost("SmsEvent")]
    [Consumes("application/json")]
    public async Task<IActionResult> SmsDeliveryEvent()
    {
        var secret = _configuration["Sinch:WebhookSecret"] ?? string.Empty;

        using var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        var headers = Request.Headers.ToDictionary(
            h => h.Key,
            h => h.Value.ToString(),
            StringComparer.OrdinalIgnoreCase);

        if (!_webhooks.ValidateAuthenticationHeader(secret, headers, body))
        {
            return Unauthorized();
        }

        var evt = _webhooks.ParseEvent(body);
        await _service.HandleEvent(evt);

        return Ok();
    }
}
