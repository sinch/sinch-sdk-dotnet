using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Sinch.SMS.SinchEvents;

/// <summary>
///     Validates HMAC authentication headers for Sinch event requests.
/// </summary>
internal sealed class HmacAuthenticationValidation
{
    private const string TimestampHeader = "x-sinch-webhook-signature-timestamp";
    private const string NonceHeader = "x-sinch-webhook-signature-nonce";
    private const string AlgorithmHeader = "x-sinch-webhook-signature-algorithm";
    private const string SignatureHeader = "x-sinch-webhook-signature";

    /// <summary>
    ///     Validates the HMAC authentication header from a Sinch event request.
    /// </summary>
    /// <param name="secret">The Sinch event secret.</param>
    /// <param name="headers">HTTP headers from the request (case-insensitive lookup will be performed).</param>
    /// <param name="jsonPayload">The raw JSON payload body.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    /// <exception cref="System.NotSupportedException">Thrown when the HMAC algorithm is not supported.</exception>
    public static bool ValidateAuthenticationHeader(
        string secret,
        IDictionary<string, string> headers,
        string jsonPayload)
    {
        var caseInsensitiveHeaders = new Dictionary<string, string>(headers, StringComparer.OrdinalIgnoreCase);

        bool TryGetHeader(string name, out string value) =>
            caseInsensitiveHeaders.TryGetValue(name, out value!) && !string.IsNullOrEmpty(value);

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
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToSign));
            return Convert.ToBase64String(hash);
        }
        catch (CryptographicException ex)
        {
            throw new NotSupportedException($"Unsupported HMAC algorithm: {algorithm}", ex);
        }
    }

    private static HMAC CreateHmac(string algorithm, string secret)
    {
        var secretBytes = System.Text.Encoding.UTF8.GetBytes(secret);

        return algorithm switch
        {
            "HmacSHA256" => new HMACSHA256(secretBytes),
            _ => throw new CryptographicException($"Algorithm '{algorithm}' is not supported")
        };
    }
}
