using Sinch.SMS.Hooks;

namespace Webhook.Template
{
    public static class WebhooksExtensions
    {
        /// <summary>
        /// Convenience overload that allows passing <see cref="IHeaderDictionary"/> (e.g. <c>Request.Headers</c>)
        /// directly to <see cref="ISmsWebhooks.ValidateAuthenticationHeader(string, IDictionary{string,string}, string)"/>.
        /// </summary>
        public static bool ValidateAuthenticationHeader(this ISmsWebhooks webhooks, string secret, IHeaderDictionary headers, string body)
        {
            ArgumentNullException.ThrowIfNull(webhooks);
            var dict = headers.ToDictionaryString();
            return webhooks.ValidateAuthenticationHeader(secret, dict, body);
        }
        
        /// <summary>
        /// Read the request body as a string using the UTF8 encoding.
        /// </summary>
        public static async Task<string> ReadBodyAsStringAsync(this HttpRequest request)
        {
            using var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            return body;
        }
        
        /// <summary>
        /// Convert an <see cref="IHeaderDictionary"/> to a case-insensitive <see cref="IDictionary{string,string}"/>.
        /// </summary>
        private static IDictionary<string, string> ToDictionaryString(this IHeaderDictionary headers, StringComparer? comparer = null) => 
            headers.ToDictionary(h => h.Key, h => h.Value.ToString(), StringComparer.OrdinalIgnoreCase);
    }
}

