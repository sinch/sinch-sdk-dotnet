using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Report.Response
{
    /// <summary>
    /// Represents reasons for various states or actions.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Reason>))]
    public record Reason(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// The reason for pending verification.
        /// </summary>
        public static readonly Reason Pending = new Reason("PENDING");

        /// <summary>
        /// The reason for a successful verification.
        /// </summary>
        public static readonly Reason Successful = new Reason("SUCCESSFUL");

        /// <summary>
        /// The reason for a failed verification attempt.
        /// </summary>
        public static readonly Reason Fail = new Reason("FAIL");

        /// <summary>
        /// The reason for a denied verification attempt by Sinch or your backend.
        /// </summary>
        public static readonly Reason Denied = new Reason("DENIED");

        /// <summary>
        /// The reason for a verification attempt that was denied by callback.
        /// </summary>
        public static readonly Reason DeniedByCallback = new Reason("Denied by callback");

        /// <summary>
        /// The reason for an invalid callback during verification.
        /// </summary>
        public static readonly Reason InvalidCallback = new Reason("Invalid callback");

        /// <summary>
        /// The reason for an internal error during the verification process.
        /// </summary>
        public static readonly Reason InternalError = new Reason("Internal error");

        /// <summary>
        /// The reason for a destination being denied during verification.
        /// </summary>
        public static readonly Reason DestinationDenied = new Reason("Destination denied");

        /// <summary>
        /// The reason for a network error or the number being unreachable during verification.
        /// </summary>
        public static readonly Reason NetworkErrorOrUnreachable = new Reason("Network error or number unreachable");

        /// <summary>
        /// The reason for a failed verification attempt while pending.
        /// </summary>
        public static readonly Reason FailedPending = new Reason("Failed pending");

        /// <summary>
        /// The reason for an SMS delivery failure during verification.
        /// </summary>
        public static readonly Reason SMSDeliveryFailure = new Reason("SMS delivery failure");

        /// <summary>
        /// The reason for an invalid CLI (Caller Line Identity) during verification.
        /// </summary>
        public static readonly Reason InvalidCLI = new Reason("Invalid CLI");

        /// <summary>
        /// The reason for an invalid code provided during verification.
        /// </summary>
        public static readonly Reason InvalidCode = new Reason("Invalid code");

        /// <summary>
        /// The reason for a verification code that has expired.
        /// </summary>
        public static readonly Reason Expired = new Reason("Expired");

        /// <summary>
        /// The reason for the call being hung up without entering a valid code.
        /// </summary>
        public static readonly Reason HungUpWithoutValidCode = new Reason("Hung up without entering valid code");

        /// <summary>
        /// The reason for fraud detected during the verification process.
        /// </summary>
        public static readonly Reason Fraud = new Reason("Fraud");

        /// <summary>
        /// The reason for not enough credit to complete the verification.
        /// </summary>
        public static readonly Reason NotEnoughCredit = new Reason("Not enough credit");

        /// <summary>
        /// The reason for a blocked verification attempt.
        /// </summary>
        public static readonly Reason Blocked = new Reason("Blocked");
    }
}
