using System;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Auth
{
    internal sealed class BasicAuth : ISinchAuth
    {
        private readonly string _appKey;
        private readonly string _appSecret;

        public string Scheme { get; } = AuthSchemes.Basic;

        public BasicAuth(string appKey, string appSecret)
        {
            _appKey = appKey;
            _appSecret = appSecret;
        }

        public Task<string> GetAuthToken(bool force = false)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{_appKey}:{_appSecret}");
            return Task.FromResult(Convert.ToBase64String(plainTextBytes));
        }
    }
}
