using System;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.SinchEvents;

namespace Sinch.Verification
{
    public interface ISinchVerificationClient
    {
        /// <summary>
        ///     Start new verification requests and report on existing verification requests.
        /// </summary>
        ISinchVerification Verification { get; }

        /// <summary>
        ///     Get the status of specific verification requests in the verification process.
        ///     Returns the status of pending and completed verifications.
        ///     You can retrieve the status of verification requests by using the ID of the request,
        ///     the phone number of the user being verified, or a custom reference string.
        /// </summary>
        ISinchVerificationStatus VerificationStatus { get; }

        /// <summary>
        ///     Parse and validate incoming Verification Sinch event payloads.
        /// </summary>
        IVerificationSinchEvents SinchEvents { get; }
    }

    internal sealed class SinchVerificationClient : ISinchVerificationClient
    {
        internal SinchVerificationClient(Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http, ApplicationSignedAuth applicationSignedAuth)
        {
            Verification = new SinchVerification(loggerFactory?.Create<SinchVerification>(), baseAddress, http);
            VerificationStatus = new SinchVerificationStatus(loggerFactory?.Create<SinchVerificationStatus>(), baseAddress, http);
            SinchEvents = new VerificationSinchEvents(applicationSignedAuth, loggerFactory?.Create<IVerificationSinchEvents>());
        }

        /// <inheritdoc />
        public ISinchVerification Verification { get; }

        /// <inheritdoc />
        public ISinchVerificationStatus VerificationStatus { get; }

        /// <inheritdoc />
        public IVerificationSinchEvents SinchEvents { get; }
    }
}
