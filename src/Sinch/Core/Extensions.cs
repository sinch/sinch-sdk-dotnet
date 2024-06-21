using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sinch.Core
{
    internal static class Extensions
    {
        /// <summary>
        ///     Throws an exception if the IsSuccessStatusCode property for the HTTP response is false.
        /// </summary>
        /// <param name="httpResponseMessage">HttpResponseMessage to check for an error.</param>
        /// <param name="options"></param>
        /// <exception cref="SinchApiException">An exception which represents API error with additional info.</exception>
        public static async Task EnsureSuccessApiStatusCode(this HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions options)
        {
            if (httpResponseMessage.IsSuccessStatusCode) return;
            ApiErrorResponse? apiErrorResponse = null;
            if (httpResponseMessage.IsJson())
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                apiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(content, options);
                if (apiErrorResponse?.Error == null && apiErrorResponse?.Text == null)
                {
                    var anotherError = JsonSerializer.Deserialize<ApiError>(content, options);
                    throw new SinchApiException(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase, null,
                        anotherError);
                }
            }

            throw new SinchApiException(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase, null,
                apiErrorResponse);
        }

        public static async Task<T?> TryGetJson<T>(this HttpResponseMessage httpResponseMessage)
        {
            var response = default(T);
            if (httpResponseMessage.IsJson()) response = await httpResponseMessage.Content.ReadFromJsonAsync<T>();

            return response;
        }

        public static bool IsJson(this HttpResponseMessage httpResponseMessage)
        {
            return httpResponseMessage.Content.Headers.ContentType?.MediaType == "application/json";
        }

        public static bool IsPdf(this HttpResponseMessage httpResponseMessage)
        {
            return httpResponseMessage.Content.Headers.ContentType?.MediaType == "application/pdf";
        }
    }
}
