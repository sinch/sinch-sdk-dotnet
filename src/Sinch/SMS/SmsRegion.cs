namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK.
    ///     By default, your account will be created in the US environment.
    ///     If you want access to servers in other parts of the world, contact your Sinch account manager.
    ///     Customer Data will be stored in the corresponding locations listed below.
    /// </summary>
    public record SmsRegion(string Value)
    {
        /// <summary>
        ///     USA
        /// </summary>
        public static readonly SmsRegion Us = new("Us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsRegion Eu = new("Eu");

        /// <summary>
        ///     Australia
        /// </summary>
        public static readonly SmsRegion Au = new("Au");

        /// <summary>
        ///     Brazil
        /// </summary>
        public static readonly SmsRegion Br = new("Br");

        /// <summary>
        ///     Canada
        /// </summary>
        public static readonly SmsRegion Ca = new("Ca");
    }
}
