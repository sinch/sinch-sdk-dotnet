using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class PriceBase
    {
        /// <summary>
        ///     The maximum price charged for this verification process.<br/><br/>
        ///     This property will appear in the body of the response with a delay.
        ///     It will become visible only when the verification status is other than PENDING
        /// </summary>
        public PriceDetail VerificationPrice { get; set; }
    }

    public class Price : PriceBase
    {
        public PriceDetail TerminationPrice { get; set; }

        /// <summary>
        ///     The time of the call for which the fee was charged. <br/><br/>
        ///     Present only when termination debiting is enabled (disabled by default).<br/><br/>
        ///     Depending on the type of rounding used, the value is the actual call time rounded
        ///     to the nearest second, minute or other value
        /// </summary>
        public int BillableDuration { get; set; }
    }
}
