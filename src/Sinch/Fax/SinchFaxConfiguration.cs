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
    }
}
