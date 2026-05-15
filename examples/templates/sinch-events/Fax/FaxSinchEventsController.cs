using Microsoft.AspNetCore.Mvc;
using Sinch.Fax.SinchEvents;
using SinchEvents.Template;

namespace SinchEvents.Template.Fax;

/// <summary>
///     Example controller for receiving Fax Sinch Events.
///     Set requireAuthentication to true once you have configured an HMAC secret
///     at 'Sinch:Fax:SinchEventsSecret' in appsettings.
///     See https://developers.sinch.com/docs/fax/api-reference/fax/
/// </summary>
[ApiController]
[FaxSinchEvent(requireAuthentication: false)]
public class FaxSinchEventsController : ControllerBase
{
    private readonly IFaxSinchEvents _sinchEvents;
    private readonly FaxServerBusinessLogic _businessLogic;

    public FaxSinchEventsController(IFaxSinchEvents sinchEvents, FaxServerBusinessLogic businessLogic)
    {
        _sinchEvents = sinchEvents;
        _businessLogic = businessLogic;
    }

    [HttpPost("FaxEvent")]
    [Consumes("application/json")]
    public async Task<IActionResult> FaxEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;
        var sinchEvent = _sinchEvents.ParseEvent(body!);
        await _businessLogic.HandleEvent(sinchEvent);

        return Ok();
    }
}
