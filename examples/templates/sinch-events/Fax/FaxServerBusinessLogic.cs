using Sinch.Core;
using Sinch.Fax.SinchEvents;

namespace SinchEvents.Template.Fax;

public class FaxServerBusinessLogic(ILogger<FaxServerBusinessLogic> logger)
{
    public Task HandleEvent(IFaxSinchEvent sinchEvent)
    {
        logger.LogInformation("Handle event: {Event}", sinchEvent.ToPrettyString());
        return Task.CompletedTask;
    }
}
