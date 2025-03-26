using System;

namespace Sinch.Numbers
{
    public sealed class SinchNumbersConfiguration
    {
        public string? UrlOverride { get; init; }

        internal Uri ResolveUrl()
        {
            const string numbersApiUrl = "https://numbers.api.sinch.com/";
            return new Uri(UrlOverride ?? numbersApiUrl);
        }
    }
}
