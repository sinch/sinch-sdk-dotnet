using System;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Auth
{
    public class BasicAuth : IAuth
    {
        private readonly string _appKey;
        private readonly string _appSecret;

        public BasicAuth(string appKey, string appSecret)
        {
            _appKey = appKey;
            _appSecret = appSecret;
            Scheme = "Basic";
        }

        public Task<string> GetToken(bool force = false)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{_appKey}:{_appSecret}");
            return Task.FromResult(Convert.ToBase64String(plainTextBytes));
        }

        public string Scheme { get; }
    }
}
