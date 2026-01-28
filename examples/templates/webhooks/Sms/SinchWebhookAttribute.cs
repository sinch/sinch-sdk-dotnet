using Microsoft.AspNetCore.Mvc;

namespace Webhook.Template.Sms;

/// <summary>
/// Attribute you can apply to controllers or actions to enable Sinch SMS webhook validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class SinchWebhookAttribute : TypeFilterAttribute
{
    public SinchWebhookAttribute(bool requireAuthentication = true)
        : base(typeof(SinchWebhookFilter))
    {
        Arguments = [requireAuthentication];
    }
}
