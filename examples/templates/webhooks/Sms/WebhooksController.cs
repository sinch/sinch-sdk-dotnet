using Microsoft.AspNetCore.Mvc;
using Sinch.SMS.Hooks;

namespace Webhook.Template.Sms;

/// <summary>
/// Ensure valid authentication to handle request.
/// See https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/#tag/Webhooks/section/Callbacks
/// Contact your account manager to configure your callback sending headers validation and
/// set requireAuthentication to true to validate request from Sinch servers.
/// See https://developers.sinch.com/docs/numbers/api-reference/numbers/tag/Numbers-Callbacks for
/// more information.
/// </summary>

[ApiController]
[SinchWebhook(requireAuthentication: false)]
public class WebhooksController : ControllerBase
{
    private readonly ISmsWebhooks _webhooks;
    private readonly ServerBusinessLogic _webhooksBusinessLogic;
    private readonly IConfiguration _configuration;

    public WebhooksController(ISmsWebhooks webhooks, ServerBusinessLogic webhooksBusinessLogic, IConfiguration configuration)
    {
        _webhooks = webhooks;
        _webhooksBusinessLogic = webhooksBusinessLogic;
        _configuration = configuration;
    }

    [HttpPost("SmsEvent")]
    [Consumes("application/json")]
    public async Task<IActionResult> SmsDeliveryEvent()
    {
        var body = HttpContext.Items[SinchWebhookConstants.BodyItemKey] as string;

        var smsEvent = _webhooks.ParseEvent(body!);
        await _webhooksBusinessLogic.HandleEvent(smsEvent);

        return Ok();
    }
}
