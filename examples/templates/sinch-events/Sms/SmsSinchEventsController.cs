using Microsoft.AspNetCore.Mvc;
using Sinch.SMS.Hooks;
using SinchEvents.Template;

namespace SinchEvents.Template.Sms;

/// <summary>
/// Example controller for receiving SMS Sinch Events.
/// Set requireAuthentication to true once you have configured a shared secret
/// at 'Sinch:Sms:WebhookSecret' in appsettings.
/// See https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/
/// </summary>
[ApiController]
[SmsSinchEvent(requireAuthentication: false)]
public class SmsSinchEventsController : ControllerBase
{
    private readonly ISmsWebhooks _smsWebhooks;
    private readonly SmsServerBusinessLogic _businessLogic;

    public SmsSinchEventsController(ISmsWebhooks smsWebhooks, SmsServerBusinessLogic businessLogic)
    {
        _smsWebhooks = smsWebhooks;
        _businessLogic = businessLogic;
    }

    [HttpPost("SmsEvent")]
    [Consumes("application/json")]
    public async Task<IActionResult> SmsEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;

        var smsEvent = _smsWebhooks.ParseEvent(body!);
        await _businessLogic.HandleEvent(smsEvent);

        return Ok();
    }
}
