using System.Collections.Generic;

namespace Sinch.Numbers.Available
{
    public sealed class AvailableNumber
    {
        /// <summary>
        ///     The phone number in<see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format with
        ///     leading +.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Region code to filter by. ISO 3166-1 alpha-2 country code of the phone number.
        ///     <example>US, GB or SE.</example>
        /// </summary>
        public string RegionCode { get; set; }

        /// <summary>
        ///     The number type.
        /// </summary>
        public Types Type { get; set; }

        /// <summary>
        ///     The capability of the number.
        /// </summary>
        public IList<Product> Capability { get; set; }

        /// <summary>
        ///     An object giving details on currency code and the amount charged.
        /// </summary>
        public Money SetupPrice { get; set; }

        /// <summary>
        ///     An object giving details on currency code and the amount charged.
        /// </summary>
        public Money MonthlyPrice { get; set; }

        /// <summary>
        ///     How often the recurring price is charged in months.
        /// </summary>
        public int PaymentIntervalMonths { get; set; }

        /// <summary>
        ///     Whether or not supplementary documentation will be required to complete the number rental.
        /// </summary>
        public bool SupportingDocumentationRequired { get; set; }
    }
}
