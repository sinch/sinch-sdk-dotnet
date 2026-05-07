using Microsoft.AspNetCore.Mvc;
using Sinch.Numbers.SinchEvents;

namespace SinchEvents.Template.Numbers;

/// <summary>
///     Apply to a controller or action to enable Numbers Sinch Event HMAC validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class NumbersSinchEventAttribute : TypeFilterAttribute
{
    public NumbersSinchEventAttribute(bool requireAuthentication)
        : base(typeof(NumbersSinchEventsFilter))
    {
        Arguments = [requireAuthentication];
    }
}
