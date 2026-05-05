using Sinch.Core;
using Sinch.Numbers.SinchEvents;

namespace SinchEvents.Template.Numbers;

public class NumbersServerBusinessLogic(ILogger<NumbersServerBusinessLogic> logger)
{
    public Task HandleEvent(INumbersSinchEvent sinchEvent)
    {
        logger.LogInformation("Handle event: {Event}", sinchEvent.ToPrettyString());
        return Task.CompletedTask;
    }
}
