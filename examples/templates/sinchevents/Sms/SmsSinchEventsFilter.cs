using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.SMS.SinchEvents;

namespace SinchEvents.Template.Sms;

/// <summary>
/// Action filter that validates Sms Sinch Event HMAC signature before executing the action.
/// </summary>
    public class SmsSinchEventsFilter(ISmsSinchEvents smsSinchEvents, IConfiguration configuration, bool requireAuthentication)
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
            // Authentication not required — proceed to the action with cached body available.
            await next();
            return;
        }

        var secret = configuration["Sinch:Sms:SinchEventSecret"] ?? string.Empty;

        var headersDictionary = request.Headers.ToDictionary(
            h => h.Key,
            h => h.Value.AsEnumerable()!,
            StringComparer.OrdinalIgnoreCase);

        if (!smsSinchEvents.ValidateAuthenticationHeader(secret, headersDictionary, body))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
