using System;
using Sinch.Conversation;
using Sinch.SMS;
using Sinch.Voice;

namespace Sinch.Core
{
    internal class UrlResolver
    {
        private readonly ApiUrlOverrides? _apiUrlOverrides;

        public UrlResolver(ApiUrlOverrides? apiUrlOverrides)
        {
            _apiUrlOverrides = apiUrlOverrides;
        }

        private const string ConversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";

        public Uri ResolveConversationUrl(ConversationRegion conversationRegion)
        {
            return new Uri(_apiUrlOverrides?.ConversationUrl ??
                           string.Format(ConversationApiUrlTemplate,
                               conversationRegion.Value));
        }

        private const string TemplatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";

        public Uri ResolveTemplateUrl(ConversationRegion conversationRegion)
        {
            return new Uri(_apiUrlOverrides?.TemplatesUrl ??
                           string.Format(TemplatesApiUrlTemplate,
                               conversationRegion.Value));
        }

        private const string AuthApiUrl = "https://auth.sinch.com";

        public Uri ResolveAuthApiUrl()
        {
            return new Uri(_apiUrlOverrides?.AuthUrl ?? AuthApiUrl);
        }

        private const string VoiceApiUrlTemplate = "https://{0}.api.sinch.com/";

        public Uri ResolveVoiceApiUrl(VoiceRegion voiceRegion)
        {
            return new Uri(_apiUrlOverrides?.VoiceUrl ?? string.Format(VoiceApiUrlTemplate, voiceRegion.Value));
        }

        // apparently, management api for applications have a different set url
        private const string VoiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";

        public Uri ResolveVoiceApiApplicationManagementUrl()
        {
            return new Uri(_apiUrlOverrides?.VoiceApplicationManagementUrl ?? VoiceApiApplicationManagementUrl);
        }

        private const string VerificationApiUrl = "https://verification.api.sinch.com/";

        public Uri ResolveVerificationUrl()
        {
            return new Uri(_apiUrlOverrides?.VerificationUrl ?? VerificationApiUrl);
        }

        private const string NumbersApiUrl = "https://numbers.api.sinch.com/";

        public Uri ResolveNumbersUrl()
        {
            return new Uri(_apiUrlOverrides?.NumbersUrl ?? NumbersApiUrl);
        }

        private const string SmsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";

        public Uri ResolveSmsUrl(SmsRegion smsRegion)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.SmsUrl)) return new Uri(_apiUrlOverrides.SmsUrl);

            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            return new Uri(string.Format(SmsApiUrlTemplate, smsRegion.Value));
        }

        private const string SmsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";

        public Uri ResolveSmsServicePlanIdUrl(SmsServicePlanIdRegion smsServicePlanIdRegion)
        {
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.SmsUrl)) return new Uri(_apiUrlOverrides.SmsUrl);

            return new Uri(string.Format(SmsApiServicePlanIdUrlTemplate,
                smsServicePlanIdRegion.Value.ToLowerInvariant()));
        }
    }
}
