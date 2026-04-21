using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Sinch.Conversation.Webhooks;
using Xunit;

namespace Sinch.Tests.Conversation.Webhooks
{
    public class HmacAuthenticationValidationTests
    {
        private const string TimestampHeader = "x-sinch-webhook-signature-timestamp";
        private const string NonceHeader = "x-sinch-webhook-signature-nonce";
        private const string AlgorithmHeader = "x-sinch-webhook-signature-algorithm";
        private const string SignatureHeader = "x-sinch-webhook-signature";

        [Fact]
        public void ValidateAuthenticationHeader_WithSupportedAlgorithm_ReturnsTrue()
        {
            const string secret = "my_secret_key";
            const string jsonPayload = "{\"event\":\"test\"}";
            const string timestamp = "1736760161";
            const string nonce = "01JHFFHWYY7HSS4FWTMDTQEK8V";
            const string algorithm = "HmacSHA256";

            const string toBeSigned = $"{jsonPayload}.{nonce}.{timestamp}";
            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(toBeSigned));
            var signature = Convert.ToBase64String(hash);

            IReadOnlyDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = [timestamp],
                [NonceHeader] = [nonce],
                [AlgorithmHeader] = [algorithm],
                [SignatureHeader] = [signature]
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithCaseInsensitiveHeaders_ReturnsTrue()
        {
            const string secret = "my_secret_key";
            const string jsonPayload = "{\"event\":\"test\"}";
            const string timestamp = "1736760161";
            const string nonce = "01JHFFHWYY7HSS4FWTMDTQEK8V";
            const string algorithm = "HmacSHA256";

            const string toBeSigned = $"{jsonPayload}.{nonce}.{timestamp}";
            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(toBeSigned));
            var signature = Convert.ToBase64String(hash);

            IReadOnlyDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>
            {
                [TimestampHeader.ToUpperInvariant()] = [timestamp],
                [NonceHeader.ToUpperInvariant()] = [nonce],
                [AlgorithmHeader.ToUpperInvariant()] = [algorithm],
                [SignatureHeader.ToUpperInvariant()] = [signature]
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithUnsupportedAlgorithm_ThrowsException()
        {
            const string secret = "some_secret";
            const string jsonPayload = "{}";
            const string timestamp = "1234567890";
            const string nonce = "nonce-value";
            const string algorithm = "HmacSHA512";

            IReadOnlyDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = [timestamp],
                [NonceHeader] = [nonce],
                [AlgorithmHeader] = [algorithm],
                [SignatureHeader] = ["whatever"]
            };

            Action act = () => HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            act.Should().Throw<NotSupportedException>().WithMessage("*Unsupported HMAC algorithm*");
        }

        [Theory]
        [InlineData(SignatureHeader)]
        [InlineData(TimestampHeader)]
        [InlineData(NonceHeader)]
        [InlineData(AlgorithmHeader)]
        public void ValidateAuthenticationHeader_MissingHeader_ReturnsFalse(string missingHeader)
        {
            const string secret = "secret";
            const string jsonPayload = "{}";

            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ["123"],
                [NonceHeader] = ["n"],
                [AlgorithmHeader] = ["HmacSHA256"],
                [SignatureHeader] = ["s"]
            };

            headers.Remove(missingHeader);

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_EmptySecret_ReturnsFalse()
        {
            const string secret = "";
            const string jsonPayload = "{}";
            const string timestamp = "123";
            const string nonce = "n";
            const string algorithm = "HmacSHA256";

            IReadOnlyDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = [timestamp],
                [NonceHeader] = [nonce],
                [AlgorithmHeader] = [algorithm],
                [SignatureHeader] = ["sig"]
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            result.Should().BeFalse();
        }
    }
}
