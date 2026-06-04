using Microsoft.AspNetCore.Mvc;

namespace SinchEvents.Template.Verification;

/// <summary>
/// Attribute you can apply to controllers or actions to enable Verification Sinch Event signature validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class VerificationSinchEventAttribute : TypeFilterAttribute
{
    public VerificationSinchEventAttribute(bool requireAuthentication)
        : base(typeof(VerificationSinchEventsFilter))
    {
        Arguments = [requireAuthentication];
    }
}