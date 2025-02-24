using System;
using System.Threading.Tasks;

namespace Sinch.Auth
{
    public sealed class BearerAuth : ISinchAuth
    {
        private readonly string _apiToken;

        public BearerAuth(string apiToken)
        {
            if (string.IsNullOrEmpty(apiToken))
            {
                throw new ArgumentNullException(nameof(apiToken), "Should have a value");
            }

            _apiToken = apiToken;
        }

        public Task<string> GetAuthToken(bool force = false)
        {
            return Task.FromResult(_apiToken);
        }

        public string Scheme { get; } = AuthSchemes.Bearer;
    }
}
