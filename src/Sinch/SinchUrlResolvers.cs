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
            => new Uri(config.UrlOverride ?? "https://auth.sinch.com");

        internal static Uri ResolveNumbersUrl(SinchNumbersConfiguration config)
            => new Uri(config.UrlOverride ?? "https://numbers.api.sinch.com/");

        internal static Uri ResolveConversationUrl(SinchConversationConfiguration config)
        {
            return config.ConversationUrlOverride is not null ? 
                new Uri(config.ConversationUrlOverride) : 
                new Uri($"https://{config.Region!.Value}.conversation.api.sinch.com/");
        }

        internal static Uri ResolveConversationTemplateUrl(SinchConversationConfiguration config)
        {
            return config.TemplateUrlOverride is not null ? 
                new Uri(config.TemplateUrlOverride) : 
                new Uri($"https://{config.Region!.Value}.template.api.sinch.com/");
        }

        internal static Uri ResolveFaxUrl(SinchFaxConfiguration config)
        {
            return config.UrlOverride is not null ? 
                new Uri(config.UrlOverride) : 
                new Uri($"https://{config.Region!.Value}.fax.api.sinch.com/");
        }

        internal static Uri ResolveVerificationUrl(SinchVerificationConfiguration config) 
            => new Uri(config.UrlOverride ?? "https://verification.api.sinch.com/");

        internal static Uri ResolveVoiceUrl(SinchVoiceConfiguration config)
        {
            const string voiceApiUrlTemplate = "https://{0}.api.sinch.com/";
            return new Uri(config.VoiceUrlOverride ?? string.Format(voiceApiUrlTemplate, config.Region.Value));
        }

        internal static Uri ResolveVoiceApplicationManagementUrl(SinchVoiceConfiguration config)
        {
            const string voiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";
            return new Uri(config.ApplicationManagementUrlOverride ?? voiceApiApplicationManagementUrl);
        }

        internal static Uri ResolveSmsUrl(SinchSmsConfiguration config)
        {
            return config.UrlOverride is not null ? 
                new Uri(config.UrlOverride) : 
                new Uri($"https://zt.{config.Region!.Value}.sms.api.sinch.com");
        }

        internal static Uri ResolveSmsServicePlanIdUrl(ServicePlanIdConfiguration config)
        {
            const string smsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";
            return new Uri(config.UrlOverride ?? string.Format(smsApiServicePlanIdUrlTemplate,
                config.ServicePlanIdRegion.Value.ToLowerInvariant()));
        }
    }
}
