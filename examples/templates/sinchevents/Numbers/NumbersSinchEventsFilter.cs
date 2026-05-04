using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.Numbers.SinchEvents;
using SinchEvents.Template;

namespace SinchEvents.Template.Numbers;

/// <summary>
///     Action filter that validates the Numbers Sinch Event HMAC signature
///     before executing the action.
/// </summary>
public class NumbersSinchEventsFilter(
    INumbersSinchEvents sinchEvents,
    IConfiguration configuration,
    bool requireAuthentication)
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

        var secret = configuration["Sinch:Numbers:HmacSecret"] ?? string.Empty;

        var headers = request.Headers
            .ToDictionary(
                h => h.Key,
                h => h.Value.Select(v => v ?? string.Empty),
                StringComparer.OrdinalIgnoreCase);

        if (!sinchEvents.ValidateAuthenticationHeader(secret, headers, body))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
