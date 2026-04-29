using System.Collections.Generic;
using System.Net.Http;

namespace Sinch.Verification.SinchEvents
{
    public interface IVerificationSinchEvents
    {
        /// <summary>
        ///     Validates the authentication header of an incoming Sinch event request.
        /// </summary>
        /// <param name="method">The HTTP method of the incoming request (e.g. HttpMethod.Post).</param>
        /// <param name="path">The request path, e.g. <c>/webhooks/verification</c>.</param>
        /// <param name="headers">The request headers, used to extract the Authorization signature.</param>
        /// <param name="body">The raw request body as a string.</param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path,
            IDictionary<string, IEnumerable<string>> headers,
            string body);
    }
}
