using Microsoft.AspNetCore.Mvc;

namespace SinchEvents.Template.Sms;

/// <summary>
/// Attribute you can apply to controllers or actions to enable Sms Sinch events validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class SmsSinchEventAttribute : TypeFilterAttribute
{
    public SmsSinchEventAttribute(bool requireAuthentication)
        : base(typeof(SmsSinchEventsFilter))
    {
        Arguments = [requireAuthentication];
    }
}
