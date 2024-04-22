using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Auth
{
    internal class ApplicationSignedAuth : ISinchAuth
    {
        private readonly string _appSecret;
        private readonly string _appKey;
        private byte[]? _jsonBodyInBytes;
        private string? _httpVerb;
        private string? _requestContentType;
        private string? _requestPath;
        private string? _timestamp;

        public string Scheme { get; } = AuthSchemes.Application;

        public ApplicationSignedAuth(string appKey, string appSecret)
        {
            _appSecret = appSecret;
            _appKey = appKey;
        }

        public string GetSignedAuth(byte[] jsonBodyBytes, string httpVerb,
            string requestPath, string timestamp, string? contentType)
        {
            _jsonBodyInBytes = jsonBodyBytes;
            _httpVerb = httpVerb;
            _requestPath = requestPath;
            _timestamp = timestamp;
            _requestContentType = contentType;
            return GetAuthToken().GetAwaiter().GetResult();
        }


        public Task<string> GetAuthToken(bool force = false)
        {
            var encodedBody = string.Empty;
            if (_jsonBodyInBytes is not null)
            {
                var md5Bytes = MD5.HashData(_jsonBodyInBytes);
                encodedBody = Convert.ToBase64String(md5Bytes);
            }

            var toSign = new StringBuilder().AppendJoin('\n', _httpVerb?.ToUpperInvariant(), encodedBody,
                _requestContentType,
                _timestamp, _requestPath).ToString();

            using var hmac = new HMACSHA256(Convert.FromBase64String(_appSecret));
            var hmacSha256 = hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign));

            var authSignature = Convert.ToBase64String(hmacSha256);

            return Task.FromResult($"{_appKey}:{authSignature}");
        }
    }
}
