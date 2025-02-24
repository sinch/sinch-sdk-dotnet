using System;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.SMS;
using Sinch.Voice;

namespace Sinch.Core
{
    internal sealed class UrlResolver
    {
        private readonly ApiUrlOverrides? _apiUrlOverrides;

        public UrlResolver(ApiUrlOverrides? apiUrlOverrides)
        {
            _apiUrlOverrides = apiUrlOverrides;
        }

        public Uri ResolveConversationUrl(ConversationRegion conversationRegion)
        {
            const string conversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";
            return new Uri(_apiUrlOverrides?.ConversationUrl ??
                           string.Format(conversationApiUrlTemplate,
                               conversationRegion.Value));
        }

        public Uri ResolveTemplateUrl(ConversationRegion conversationRegion)
        {
            const string templatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";
            return new Uri(_apiUrlOverrides?.TemplatesUrl ??
                           string.Format(templatesApiUrlTemplate,
                               conversationRegion.Value));
        }

        public Uri ResolveAuth()
        {
            const string authApiUrl = "https://auth.sinch.com";
            return new Uri(_apiUrlOverrides?.AuthUrl ?? authApiUrl);
        }


        public Uri ResolveVoiceUrl(VoiceRegion voiceRegion)
        {
            const string voiceApiUrlTemplate = "https://{0}.api.sinch.com/";
            return new Uri(_apiUrlOverrides?.VoiceUrl ?? string.Format(voiceApiUrlTemplate, voiceRegion.Value));
        }

        public Uri ResolveVoiceApplicationManagementUrl()
        {
            // apparently, management api for applications have a different set url
            const string voiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";
            return new Uri(_apiUrlOverrides?.VoiceApplicationManagementUrl ?? voiceApiApplicationManagementUrl);
        }

        public Uri ResolveNumbersUrl()
        {
            const string numbersApiUrl = "https://numbers.api.sinch.com/";
            return new Uri(_apiUrlOverrides?.NumbersUrl ?? numbersApiUrl);
        }

        public Uri ResolveSmsUrl(SmsRegion smsRegion)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.SmsUrl)) return new Uri(_apiUrlOverrides.SmsUrl);

            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            const string smsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";
            return new Uri(string.Format(smsApiUrlTemplate, smsRegion.Value));
        }

        public Uri ResolveSmsServicePlanIdUrl(SmsServicePlanIdRegion smsServicePlanIdRegion)
        {
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.SmsUrl)) return new Uri(_apiUrlOverrides.SmsUrl);

            const string smsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";
            return new Uri(string.Format(smsApiServicePlanIdUrlTemplate,
                smsServicePlanIdRegion.Value.ToLowerInvariant()));
        }


        public Uri ResolveFaxUrl(FaxRegion? faxRegion)
        {
            const string faxApiUrl = "https://fax.api.sinch.com/";
            const string faxApiUrlTemplate = "https://{0}.fax.api.sinch.com/";
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.FaxUrl))
            {
                return new Uri(_apiUrlOverrides.FaxUrl);
            }

            if (!string.IsNullOrEmpty(faxRegion?.Value))
            {
                return new Uri(string.Format(faxApiUrlTemplate, faxRegion.Value));
            }

            return new Uri(faxApiUrl);
        }
    }
}
