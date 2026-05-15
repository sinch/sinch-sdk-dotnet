using Microsoft.AspNetCore.Mvc;
using Sinch.Fax.SinchEvents;

namespace SinchEvents.Template.Fax;

/// <summary>
///     Apply to a controller or action to enable Fax Sinch Event HMAC validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class FaxSinchEventAttribute : TypeFilterAttribute
{
    public FaxSinchEventAttribute(bool requireAuthentication)
        : base(typeof(FaxSinchEventsFilter))
    {
        Arguments = [requireAuthentication];
    }
}
