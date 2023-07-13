namespace Sinch.Numbers
{

    /// <summary>
    ///     Gives the details on currency code and the amount charged.
    /// </summary>
    public sealed class Money
    {
        /// <summary>
        ///     The 3-letter currency code defined in <see href="https://www.iso.org/iso-4217-currency-codes.html">ISO 4217</see>.
        /// </summary>
#if NET_7
        public required string CurrencyCode { get; set; }
#else
        public string CurrencyCode { get; set; }
#endif

        /// <summary>
        ///     Amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
