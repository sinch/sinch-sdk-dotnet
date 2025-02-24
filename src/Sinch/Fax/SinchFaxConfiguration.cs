using System;

namespace Sinch.Fax
{
    public sealed class SinchFaxConfiguration
    {
        public FaxRegion? Region { get; init; }

        public string? UrlOverride { get; init; }

        internal Uri ResolveUrl()
        {
            const string faxApiUrl = "https://fax.api.sinch.com/";
            const string faxApiUrlTemplate = "https://{0}.fax.api.sinch.com/";
            if (!string.IsNullOrEmpty(UrlOverride))
            {
                return new Uri(UrlOverride);
            }

            if (!string.IsNullOrEmpty(Region?.Value))
            {
                return new Uri(string.Format(faxApiUrlTemplate, Region?.Value));
            }

            return new Uri(faxApiUrl);
        }
    }
}
