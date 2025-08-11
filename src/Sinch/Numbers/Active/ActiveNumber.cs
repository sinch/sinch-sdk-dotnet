using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Numbers.VoiceConfigurations;

namespace Sinch.Numbers.Active
{
    public sealed class ActiveNumber
    {
        /// <summary>
        ///     The phone number in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format with
        ///     leading +. <br /> Example:
        ///     <example>+12025550134</example>
        /// </summary>

        public required string PhoneNumber { get; set; }


        /// <summary>
        ///     Project ID. Your project ID can be found on your
        ///     <see href="https://dashboard.sinch.com/settings/project-management">Sinch Customer Dashboard</see>
        /// </summary>

        public required string ProjectId { get; set; }



        /// <summary>
        ///     User supplied name for the phone number.
        /// </summary>

        public required string DisplayName { get; set; }



        /// <summary>
        ///     ISO 3166-1 alpha-2 country code of the phone number.<br />
        ///     Example: US, UK or SE.
        /// </summary>

        public required string RegionCode { get; set; }



        /// <summary>
        ///     The number type.
        /// </summary>
        public Types? Type { get; set; }

        /// <summary>
        ///     The capability of the number.
        /// </summary>

        public required List<Product> Capability { get; set; }


        /// <summary>
        ///     An object giving details on currency code and the amount charged.
        /// </summary>

        public required Money Money { get; set; }



        /// <summary>
        ///     How often the recurring price is charged in months.
        /// </summary>
        public int? PaymentIntervalMonths { get; set; }

        /// <summary>
        ///     The date of the next charge.
        /// </summary>
        public DateTime? NextChargeDate { get; set; }

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
        public SmsConfiguration? SmsConfiguration { get; set; }

        /// <summary>
        ///     The current voice configuration for this number.<br /><br />
        ///     During scheduled provisioning, the app ID value may be empty in a response if it is still processing or if it has
        ///     failed.<br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully, the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        [JsonConverter(typeof(VoiceConfigurationConverter))]
        public VoiceConfiguration? VoiceConfiguration { get; set; }
    }
}
