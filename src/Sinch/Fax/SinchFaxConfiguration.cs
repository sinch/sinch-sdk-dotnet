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

        private static string RegionRequiredMessage =>
            $"{nameof(SinchFaxConfiguration)}.{nameof(Region)} is required. " +
            $"Set it to one of the values in {nameof(FaxRegion)}, e.g. {nameof(FaxRegion)}.{nameof(FaxRegion.UsEastCoast)}.";

        internal Uri ResolveUrl()
        {
            if (UrlOverride is not null)
                return new Uri(UrlOverride);

            if (Region is null)
                throw new InvalidOperationException(RegionRequiredMessage);

            return new Uri(string.Format("https://{0}.fax.api.sinch.com/", Region.Value));
        }

        internal void Validate()
        {
            if (Region == null)
                throw new InvalidOperationException(RegionRequiredMessage);
        }
    }
}
