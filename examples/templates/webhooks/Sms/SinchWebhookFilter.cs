using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinch.SMS.Hooks;

namespace Webhook.Template.Sms;

/// <summary>
/// Action filter that validates Sinch SMS webhook HMAC signature before executing the action.
/// </summary>
public class SinchWebhookFilter : IAsyncActionFilter
{
    private readonly ISmsWebhooks _webhooks;
    private readonly IConfiguration _configuration;
    private readonly bool _requireAuthentication;

    public SinchWebhookFilter(ISmsWebhooks webhooks, IConfiguration configuration, bool requireAuthentication = true)
    {
        _webhooks = webhooks;
        _configuration = configuration;
        _requireAuthentication = requireAuthentication;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        // Always read and cache the body so controllers can reuse it regardless of auth requirement.
        context.HttpContext.Items[SinchWebhookConstants.BodyItemKey] = body;

        if (!_requireAuthentication)
        {
            // Authentication not required — proceed to the action with cached body available.
            await next();
            return;
        }

        var secret = _configuration["Sinch:Sms:WebhookSecret"] ?? string.Empty;

        var headersDictionary = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        if (!_webhooks.ValidateAuthenticationHeader(secret, headersDictionary, body))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
