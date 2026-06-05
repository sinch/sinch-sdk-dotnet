using Microsoft.AspNetCore.Mvc;

namespace SinchEvents.Template.Voice;

/// <summary>
/// Attribute you can apply to controllers or actions to enable Voice Sinch Event signature validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class VoiceSinchEventAttribute : TypeFilterAttribute
{
    public VoiceSinchEventAttribute(bool requireAuthentication)
        : base(typeof(VoiceSinchEventsFilter))
    {
        Arguments = [requireAuthentication];
    }
}
