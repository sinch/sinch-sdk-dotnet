using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Sinch.Tests.Features
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Extracts all headers from an HttpResponseMessage into a single dictionary.
        /// Used in tests to simulate webhook request headers for validation.
        /// </summary>
        public static Dictionary<string, IEnumerable<string>> GetAllHeaders(this HttpResponseMessage response) =>
            response.Headers.Concat(response.Content.Headers).ToDictionary(x => x.Key, y => y.Value);
    }
}
