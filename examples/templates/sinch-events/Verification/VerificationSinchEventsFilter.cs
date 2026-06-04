using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.Verification.SinchEvents;

namespace SinchEvents.Template.Verification;

/// <summary>
/// Action filter that validates the Verification signed request before executing the action.
/// </summary>
public class VerificationSinchEventsFilter(IVerificationSinchEvents sinchEvents, bool requireAuthentication)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

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