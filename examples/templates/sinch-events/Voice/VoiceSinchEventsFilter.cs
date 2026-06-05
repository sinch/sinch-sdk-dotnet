using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.Voice.SinchEvents;

namespace SinchEvents.Template.Voice;

/// <summary>
/// Action filter that validates the Voice Sinch Event signed request before executing the action.
/// </summary>
public class VoiceSinchEventsFilter(IVoiceSinchEvents sinchEvents, bool requireAuthentication)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        context.HttpContext.Items[SinchEventsConstants.BodyItemKey] = body;

        if (!requireAuthentication)
        {
            await next();
            return;
        }

        if (!sinchEvents.ValidateAuthenticationHeader(
                HttpMethod.Post,
                request.Path,
                request.Headers,
                body))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
