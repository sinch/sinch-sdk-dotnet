using System;

namespace Sinch.Fax
{
    public sealed class SinchFaxConfiguration
    {
        /// <summary>
        ///     Sets the region for the Fax API.
        ///     Required. See <see cref="FaxRegion" /> for available values.
        /// </summary>
        public FaxRegion? Region { get; init; }

        public string? UrlOverride { get; init; }

        internal Uri ResolveUrl()
        {
            const string faxApiUrlTemplate = "https://{0}.fax.api.sinch.com/";
            return new Uri(UrlOverride ?? string.Format(faxApiUrlTemplate, Region!.Value));
        }

        internal void Validate()
        {
            if (Region == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(SinchFaxConfiguration)}.{nameof(Region)} is required. " +
                    $"Set it to one of the values in {nameof(FaxRegion)}, e.g. {nameof(FaxRegion)}.{nameof(FaxRegion.UsEastCoast)}.");
            }
        }
    }
}
