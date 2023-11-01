namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK as a hosting region.
    /// </summary>
    public record SmsHostingRegion(string Value)
    {
        /// <summary>
        ///     USA
        /// </summary>
        public static readonly SmsHostingRegion Us = new("Us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsHostingRegion Eu = new("Eu");
    }
}
