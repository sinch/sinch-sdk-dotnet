using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Sinch.Core
{
    internal static class HeaderValidation
    {
        public static bool ValidateAuthHeader(string hmacSecret, string json, string xSinchSignatureHeaderValue)
        {
            if (string.IsNullOrEmpty(hmacSecret) || string.IsNullOrEmpty(xSinchSignatureHeaderValue) ||
                string.IsNullOrEmpty(json))
            {
                return false;
            }

            var result = ComputeHmacSha1(hmacSecret, json);
            return string.Equals(xSinchSignatureHeaderValue, result);
        }

        public static bool ValidateAuthHeader(string hmacSecret, string json, HttpHeaders headers)
        {
            var headersNormalized = headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            if (!headersNormalized.TryGetValue("x-sinch-signature", out var signature))
            {
                return false;
            }

            var signatureValue = signature.FirstOrDefault();
            if (string.IsNullOrEmpty(signatureValue))
            {
                return false;
            }

            return ValidateAuthHeader(hmacSecret, json, signatureValue);
        }

        private static string ComputeHmacSha1(string secret, string body)
        {
            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
