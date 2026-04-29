using System.Collections.Generic;
using System.Net.Http;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Verification.SinchEvents
{
    internal sealed class VerificationSinchEvents(ApplicationSignedAuth applicationSignedAuth,
        ILoggerAdapter<IVerificationSinchEvents>? logger = null) : IVerificationSinchEvents
    {
        public bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers, string body)
        {
            return AuthorizationHeaderValidation.Validate(method, path, headers, body, applicationSignedAuth, logger);
        }
    }
}
