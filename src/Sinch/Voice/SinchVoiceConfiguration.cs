using System;

namespace Sinch.Voice
{
    public sealed class SinchVoiceConfiguration
    {
        public required string AppKey { get; init; }

        public required string AppSecret { get; init; }

        public string? VoiceUrlOverride { get; init; }

        public string? ApplicationManagementUrlOverride { get; init; }

        public VoiceRegion Region { get; init; } = VoiceRegion.Global;

        internal Uri ResolveUrl()
        {
            const string voiceApiUrlTemplate = "https://{0}.api.sinch.com/";
            return new Uri(VoiceUrlOverride ?? string.Format(voiceApiUrlTemplate, Region.Value));
        }

        internal Uri ResolveApplicationManagementUrl()
        {
            if (!string.IsNullOrEmpty(ApplicationManagementUrlOverride))
            {
                return new Uri(ApplicationManagementUrlOverride);
            }

            // apparently, management api for applications have a different set url
            const string voiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";
            return new Uri(voiceApiApplicationManagementUrl);
        }

        internal SinchVoiceConfiguration Validate()
        {
            if (string.IsNullOrEmpty(AppKey))
                throw new ArgumentNullException(nameof(AppKey), "The value should be present");

            if (string.IsNullOrEmpty(AppSecret))
                throw new ArgumentNullException(nameof(AppSecret), "The value should be present");

            return this;
        }
    }
}
