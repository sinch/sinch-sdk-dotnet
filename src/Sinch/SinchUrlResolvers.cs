using System;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;
using Sinch.Voice;

namespace Sinch
{
    internal static class SinchUrlResolvers
    {
        internal static Uri ResolveAuthUrl(SinchOAuthConfiguration config)
            => new(config.UrlOverride ?? "https://auth.sinch.com");

        internal static Uri ResolveNumbersUrl(SinchNumbersConfiguration config)
            => new(config.UrlOverride ?? "https://numbers.api.sinch.com/");

        internal static Uri ResolveConversationUrl(SinchConversationConfiguration config)
            => new(config.ConversationUrlOverride ?? $"https://{config.Region!.Value}.conversation.api.sinch.com/");

        internal static Uri ResolveConversationTemplateUrl(SinchConversationConfiguration config)
            => new(config.TemplateUrlOverride ?? $"https://{config.Region!.Value}.template.api.sinch.com/");

        internal static Uri ResolveFaxUrl(SinchFaxConfiguration config)
            => new(config.UrlOverride ?? "https://fax.api.sinch.com/");

        internal static Uri ResolveVerificationUrl(SinchVerificationConfiguration config)
            => new(config.UrlOverride ?? "https://verification.api.sinch.com/");

        internal static Uri ResolveVoiceUrl(SinchVoiceConfiguration config)
            => new(config.VoiceUrlOverride ?? $"https://{config.Region.Value}.api.sinch.com/");

        internal static Uri ResolveVoiceApplicationManagementUrl(SinchVoiceConfiguration config)
            => new(config.ApplicationManagementUrlOverride ?? "https://callingapi.sinch.com/");

        internal static Uri ResolveSmsUrl(SinchSmsConfiguration config)
            => new(config.UrlOverride ?? $"https://zt.{config.Region!.Value}.sms.api.sinch.com");

        internal static Uri ResolveSmsServicePlanIdUrl(ServicePlanIdConfiguration config)
            => new(config.UrlOverride ?? $"https://{config.ServicePlanIdRegion.Value.ToLowerInvariant()}.sms.api.sinch.com");
    }
}
