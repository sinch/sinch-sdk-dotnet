using System.Text.Json.Serialization;

namespace Sinch.Fax
{
    public class PagedResponse
    {
        /// <summary>
        ///     Current page
        /// </summary>
        /// TODO: Remove [JsonPropertyName("pageNumber")] after mock server update.
        [JsonPropertyName("pageNumber")]
        public int Page { get; set; }

        /// <summary>
        ///     Total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     Number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Total size of the result.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
