using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Sinch.Core
{
    internal static class Extensions
    {
        /// <summary>
        ///     Throws an exception if the IsSuccessStatusCode property for the HTTP response is false.
        /// </summary>
        /// <param name="httpResponseMessage">HttpResponseMessage to check for an error.</param>
        /// <exception cref="SinchApiException">An exception which represents API error with additional info.</exception>
        public static async Task EnsureSuccessApiStatusCode(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode) return;

            var apiError = await httpResponseMessage.TryGetJson<ApiErrorResponse>();

            throw new SinchApiException(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase, null, apiError);
        }

        public static async Task<T?> TryGetJson<T>(this HttpResponseMessage httpResponseMessage)
        {
            var authResponse = default(T);
            if (httpResponseMessage.IsJson()) authResponse = await httpResponseMessage.Content.ReadFromJsonAsync<T>();

            return authResponse;
        }

        public static bool IsJson(this HttpResponseMessage httpResponseMessage)
        {
            return httpResponseMessage.Content.Headers.ContentType?.MediaType == "application/json";
        }
    }
}
