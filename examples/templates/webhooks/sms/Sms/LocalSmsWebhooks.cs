using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Sinch.SMS.Webhooks;

namespace SmsWebhookTemplate.Sms
{
    // Minimal local ISmsWebhooks implementation used when SinchClient isn't configured.
    // Kept lightweight for local testing and demo purposes. For production, use the SDK
    // implementation from SinchClient or implement additional security checks.
    public class LocalSmsWebhooks : ISmsWebhooks
    {
        private readonly JsonSerializerOptions _opts;

        public LocalSmsWebhooks()
        {
            _opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _opts.Converters.Add(new Sinch.SMS.Hooks.SmsEventConverter());
        }

        public Sinch.SMS.Hooks.ISmsEvent ParseEvent(string json)
        {
            return JsonSerializer.Deserialize<Sinch.SMS.Hooks.ISmsEvent>(json, _opts)
                   ?? throw new JsonException("Unable to parse SMS event");
        }

        public bool ValidateAuthenticationHeader(string secret, IDictionary<string, string> headers, string body)
        {
            // Basic validation: if no secret configured, accept for local testing
            if (string.IsNullOrEmpty(secret)) return true;

            if (!headers.TryGetValue("x-sinch-webhook-signature", out var signature)) return false;
            if (!headers.TryGetValue("x-sinch-webhook-signature-timestamp", out var timestamp)) return false;
            if (!headers.TryGetValue("x-sinch-webhook-signature-nonce", out var nonce)) return false;
            if (!headers.TryGetValue("x-sinch-webhook-signature-algorithm", out var algorithm)) return false;

            var toSign = $"{body}.{nonce}.{timestamp}";

            System.Security.Cryptography.HMAC? hmac = algorithm switch
            {
                "HmacSHA256" => new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)),
                "HmacSHA512" => new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secret)),
                _ => null
            };

            if (hmac == null) return false;

            using (hmac)
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign));
                var computed = Convert.ToBase64String(hash);
                return computed == signature;
            }
        }
    }
}

