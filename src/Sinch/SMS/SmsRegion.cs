namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK.
    ///     By default, your account will be created in the US environment.
    ///     If you want access to servers in other parts of the world, contact your Sinch account manager.
    ///     Customer Data will be stored in the corresponding locations listed below.
    /// </summary>
    public enum SmsRegion
    {
        /// <summary>
        ///     USA
        /// </summary>
        Us,

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        Eu,

        /// <summary>
        ///     Australia
        /// </summary>
        Au,

        /// <summary>
        ///     Brazil
        /// </summary>
        Br,

        /// <summary>
        ///     Canada
        /// </summary>
        Ca
    }
}
