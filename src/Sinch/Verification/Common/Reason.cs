using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Common
{
    /// <summary>
    ///     Displays the reason why a verification has FAILED, was DENIED, or was ABORTED.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Reason>))]
    public record Reason(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The reason for fraud detected during the verification process.
        /// </summary>
        public static readonly Reason Fraud = new("Fraud");

        /// <summary>
        ///     The reason for not enough credit to complete the verification.
        /// </summary>
        public static readonly Reason NotEnoughCredit = new("Not enough credit");

        /// <summary>
        ///     The reason for a blocked verification attempt.
        /// </summary>
        public static readonly Reason Blocked = new("Blocked");

        /// <summary>
        ///     The reason for a verification attempt that was denied by callback.
        /// </summary>
        public static readonly Reason DeniedByCallback = new("Denied by callback");

        /// <summary>
        ///     The reason for an invalid callback during verification.
        /// </summary>
        public static readonly Reason InvalidCallback = new("Invalid callback");

        /// <summary>
        ///     The reason for an internal error during the verification process.
        /// </summary>
        public static readonly Reason InternalError = new("Internal error");

        /// <summary>
        ///     The reason for a destination being denied during verification.
        /// </summary>
        public static readonly Reason DestinationDenied = new("Destination denied");

        /// <summary>
        ///     The reason for a network error or the number being unreachable during verification.
        /// </summary>
        public static readonly Reason NetworkErrorOrUnreachable = new("Network error or number unreachable");

        /// <summary>
        ///     The reason for a failed verification attempt while pending.
        /// </summary>
        public static readonly Reason FailedPending = new("Failed pending");

        /// <summary>
        ///     The reason for an SMS delivery failure during verification.
        /// </summary>
        public static readonly Reason SMSDeliveryFailure = new("SMS delivery failure");

        /// <summary>
        ///     The reason for an invalid CLI (Caller Line Identity) during verification.
        /// </summary>
        public static readonly Reason InvalidCLI = new("Invalid CLI");

        /// <summary>
        ///     The reason for an invalid code provided during verification.
        /// </summary>
        public static readonly Reason InvalidCode = new("Invalid code");

        /// <summary>
        ///     The reason for a verification code that has expired.
        /// </summary>
        public static readonly Reason Expired = new("Expired");

        /// <summary>
        ///     The reason for the call being hung up without entering a valid code.
        /// </summary>
        public static readonly Reason HungUpWithoutValidCode = new("Hung up without entering valid code");
    }
}
