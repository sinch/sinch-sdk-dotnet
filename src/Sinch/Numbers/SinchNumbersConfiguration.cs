using System;

namespace Sinch.Numbers
{
    public sealed class SinchNumbersConfiguration
    {
        public string? UrlOverride { get; init; }

        internal Uri ResolveUrl()
        {
            const string numbersApiUrl = "https://numbers.api.sinch.com/";
            if (!string.IsNullOrEmpty(UrlOverride))
            {
                return new Uri(UrlOverride);
            }

            return new Uri(numbersApiUrl);
        }
    }
}
