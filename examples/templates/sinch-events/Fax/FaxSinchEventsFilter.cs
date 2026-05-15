using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.Fax.SinchEvents;
using SinchEvents.Template;

namespace SinchEvents.Template.Fax;

/// <summary>
///     Action filter that validates the Fax Sinch Event HMAC signature
///     before executing the action.
/// </summary>
public class FaxSinchEventsFilter(
    IFaxSinchEvents sinchEvents,
    IConfiguration configuration,
    bool requireAuthentication)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        // Always read and cache the body so controllers can reuse it regardless of auth requirement.
        context.HttpContext.Items[SinchEventsConstants.BodyItemKey] = body;

        if (!requireAuthentication)
        {
            await next();
            return;
        }

        var secret = configuration["Sinch:Fax:SinchEventsSecret"] ?? string.Empty;

        if (!sinchEvents.ValidateAuthenticationHeader(secret, request.Headers, body))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
