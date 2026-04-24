using Microsoft.AspNetCore.Mvc;
using Sinch.SMS.SinchEvents;

namespace SinchEvents.Template.Sms;

/// <summary>
/// Ensure valid authentication to handle request.
/// See https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/#tag/Webhooks/section/Callbacks
/// Contact your account manager to configure your sinch event sending headers validation and
/// set requireAuthentication to true to validate request from Sinch servers.
/// See https://developers.sinch.com/docs/numbers/api-reference/numbers/tag/Numbers-Callbacks for
/// more information.
/// </summary>

[ApiController]
[SmsSinchEvent(requireAuthentication: false)]
public class SmsSinchEventsController : ControllerBase
{
    private readonly ISinchSmsSinchEvents _smsSinchEvents;
    private readonly ServerBusinessLogic _sinchEventsBusinessLogic;
    private readonly IConfiguration _configuration;

    public SmsSinchEventsController(ISinchSmsSinchEvents smsSinchEvents, ServerBusinessLogic sinchEventsBusinessLogic, IConfiguration configuration)
    {
        _smsSinchEvents = smsSinchEvents;
        _sinchEventsBusinessLogic = sinchEventsBusinessLogic;
        _configuration = configuration;
    }

    [HttpPost("SmsEvent")]
    [Consumes("application/json")]
    public async Task<IActionResult> SmsDeliveryEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;

        var smsEvent = _smsSinchEvents.ParseEvent(body!);
        await _sinchEventsBusinessLogic.HandleEvent(smsEvent);

        return Ok();
    }
}
