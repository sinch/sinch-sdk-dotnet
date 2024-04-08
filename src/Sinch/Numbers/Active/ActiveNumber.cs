using System;
using System.Collections.Generic;

namespace Sinch.Numbers.Active
{
    public sealed class ActiveNumber
    {
        /// <summary>
        ///     The phone number in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format with
        ///     leading +. <br /> Example:
        ///     <example>+12025550134</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string PhoneNumber { get; set; }
#else
        public string PhoneNumber { get; set; }
#endif

        /// <summary>
        ///     Project ID. Your project ID can be found on your
        ///     <see href="https://dashboard.sinch.com/settings/project-management">Sinch Customer Dashboard</see>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ProjectId { get; set; }
#else
        public string ProjectId { get; set; }
#endif


        /// <summary>
        ///     User supplied name for the phone number.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string DisplayName { get; set; }
#else
        public string DisplayName { get; set; }
#endif


        /// <summary>
        ///     ISO 3166-1 alpha-2 country code of the phone number.<br />
        ///     Example: US, UK or SE.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionCode { get; set; }
#else
        public string RegionCode { get; set; }
#endif


        /// <summary>
        ///     The number type.
        /// </summary>
        public Types Type { get; set; }

        /// <summary>
        ///     The capability of the number.
        /// </summary>
#if NET7_0_OR_GREATER
        public required IList<Product> Capability { get; set; }
#else
        public IList<Product> Capability { get; set; }
#endif

        /// <summary>
        ///     An object giving details on currency code and the amount charged.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Money Money { get; set; }
#else
        public Money Money { get; set; }
#endif


        /// <summary>
        ///     How often the recurring price is charged in months.
        /// </summary>
        public int PaymentIntervalMonths { get; set; }

        /// <summary>
        ///     The date of the next charge.
        /// </summary>
        public DateTime NextChargeDate { get; set; }

        /// <summary>
        ///     The timestamp when the subscription will expire if an expiration date has been set.
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        ///     The current SMS configuration for this number.<br /><br />
        ///     Once the servicePlanId is sent, it enters scheduled provisioning.<br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully, the servicePlanId sent will appear directly under the smsConfiguration object.
        /// </summary>
        public SmsConfiguration SmsConfiguration { get; set; }

        /// <summary>
        ///     The current voice configuration for this number.<br /><br />
        ///     During scheduled provisioning, the app ID value may be empty in a response if it is still processing or if it has
        ///     failed.<br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully, the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        public VoiceConfiguration VoiceConfiguration { get; set; }
    }
}
