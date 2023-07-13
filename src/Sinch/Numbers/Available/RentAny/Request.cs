using System.Collections.Generic;

namespace Sinch.Numbers.Available.RentAny
{
    /// <summary>
    ///     The request to search and rent a number that matches the criteria.
    /// </summary>
    public sealed class Request
    {
        public NumberPattern NumberPattern { get; set; }

        /// <summary>
        ///     Region code to filter by. ISO 3166-1 alpha-2 country code of the phone number.
        ///     <example>
        ///         US, GB or SE.
        ///     </example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionCode { get; set; }
#else
        public string RegionCode { get; set; }
#endif

        /// <summary>
        ///     Number type to filter by. MOBILE, LOCAL or TOLL_FREE. Default: LOCAL
        /// </summary>
        public Types Type { get; set; } = Types.Local;

        /// <summary>
        ///     Number capabilities to filter by, SMS and/or VOICE
        /// </summary>
        public IList<Product> Capabilities { get; set; }

        /// <summary>
        ///     The current SMS configuration for this number.<br /><br />
        ///     Once the servicePlanId is sent, it enters scheduled provisioning.<br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running.
        ///     Once processed successfully, the servicePlanId sent will appear directly under the smsConfiguration object.
        /// </summary>
        public SmsConfiguration SmsConfiguration { get; set; }

        /// <summary>
        ///     The current voice configuration for this number. During scheduled provisioning, the app ID value may be empty in a
        ///     response if it is still processing or if it has failed.
        ///     The status of scheduled provisioning will show under a scheduledVoiceProvisioning object if it's still running.
        ///     Once processed successfully, the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        public VoiceConfiguration VoiceConfiguration { get; set; }
    }
}
