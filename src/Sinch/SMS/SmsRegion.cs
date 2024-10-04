namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK as a hosting region.
    /// </summary>
    public record SmsRegion(string Value)
    {
        /// <summary>
        ///     USA
        /// </summary>
        public static readonly SmsRegion Us = new("us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsRegion Eu = new("eu");
    }
}
