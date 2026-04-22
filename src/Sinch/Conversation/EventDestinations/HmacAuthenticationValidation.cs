using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sinch.Conversation.EventDestinations;

/// <summary>
///     Validates HMAC authentication headers for Conversation event destination requests.
/// </summary>
internal sealed class HmacAuthenticationValidation
{
    private const string TimestampHeader = "x-sinch-webhook-signature-timestamp";
    private const string NonceHeader = "x-sinch-webhook-signature-nonce";
    private const string AlgorithmHeader = "x-sinch-webhook-signature-algorithm";
    private const string SignatureHeader = "x-sinch-webhook-signature";

    /// <summary>
    ///     Validates the HMAC authentication header from a Conversation event destination request.
    /// </summary>
    /// <param name="secret">The event destination secret.</param>
    /// <param name="headers">HTTP headers from the request as single-value entries.</param>
    /// <param name="jsonPayload">The raw JSON payload body.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    /// <exception cref="NotSupportedException">Thrown when the HMAC algorithm is not supported.</exception>
    public static bool ValidateAuthenticationHeader(
        string secret,
        IDictionary<string, string> headers,
        string jsonPayload)
    {
        var multiValueHeaders = new Dictionary<string, IEnumerable<string>>(headers.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            multiValueHeaders[header.Key] = [header.Value];
        }

        return ValidateAuthenticationHeader(secret, multiValueHeaders, jsonPayload);
    }

    /// <summary>
    ///     Validates the HMAC authentication header from a Conversation event destination request.
    /// </summary>
    /// <param name="secret">The event destination secret.</param>
    /// <param name="headers">HTTP headers from the request (case-insensitive lookup will be performed).</param>
    /// <param name="jsonPayload">The raw JSON payload body.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    /// <exception cref="NotSupportedException">Thrown when the HMAC algorithm is not supported.</exception>
    public static bool ValidateAuthenticationHeader(
        string secret,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        string jsonPayload)
    {
        var caseInsensitiveHeaders =
            new Dictionary<string, IEnumerable<string>>(headers, StringComparer.OrdinalIgnoreCase);

        bool TryGetHeader(string name, out string value)
        {
            value = string.Empty;
            if (!caseInsensitiveHeaders.TryGetValue(name, out var values))
            {
                return false;
            }

            var headerValue = values.FirstOrDefault();
            if (string.IsNullOrEmpty(headerValue))
            {
                return false;
            }

            value = headerValue;
            return true;
        }

        if (string.IsNullOrEmpty(secret) ||
            !TryGetHeader(TimestampHeader, out var timestampHeader) ||
            !TryGetHeader(NonceHeader, out var nonceHeader) ||
            !TryGetHeader(AlgorithmHeader, out var algorithmHeader) ||
            !TryGetHeader(SignatureHeader, out var signatureHeader))
        {
            return false;
        }

        var toBeSignedData = $"{jsonPayload}.{nonceHeader}.{timestampHeader}";
        var signedData = Encode(secret, algorithmHeader, toBeSignedData);

        return string.Equals(signatureHeader, signedData, StringComparison.Ordinal);
    }

    private static string Encode(string secret, string algorithm, string stringToSign)
    {
        try
        {
            using var hmac = CreateHmac(algorithm, secret);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
            return Convert.ToBase64String(hash);
        }
        catch (CryptographicException ex)
        {
            throw new NotSupportedException($"Unsupported HMAC algorithm: {algorithm}", ex);
        }
    }

    private static HMAC CreateHmac(string algorithm, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);

        return algorithm switch
        {
            "HmacSHA256" => new HMACSHA256(secretBytes),
            _ => throw new CryptographicException($"Algorithm '{algorithm}' is not supported")
        };
    }
}
