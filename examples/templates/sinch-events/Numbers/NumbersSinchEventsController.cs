using Microsoft.AspNetCore.Mvc;
using Sinch.Numbers.SinchEvents;
using SinchEvents.Template;

namespace SinchEvents.Template.Numbers;

/// <summary>
///     Example controller for receiving Numbers Sinch Events.
///     Set requireAuthentication to true once you have configured an HMAC secret
///     at 'Sinch:Numbers:HmacSecret' in appsettings.
///     See https://developers.sinch.com/docs/numbers/api-reference/numbers/tag/Numbers-Callbacks
/// </summary>
[ApiController]
[NumbersSinchEvent(requireAuthentication: false)]
public class NumbersSinchEventsController : ControllerBase
{
    private readonly INumbersSinchEvents _sinchEvents;

    public NumbersSinchEventsController(INumbersSinchEvents sinchEvents)
    {
        _sinchEvents = sinchEvents;
    }

    [HttpPost("NumbersEvent")]
    [Consumes("application/json")]
    public IActionResult NumbersEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;
        var sinchEvent = _sinchEvents.ParseEvent(body!);

        // Handle: sinchEvent.EventType, .ResourceId, .ResourceType, .Status, etc.

        return Ok();
    }
}
