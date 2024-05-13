using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Sinch
{
    internal sealed class ApiErrorResponse
    {
        public ApiError? Error { get; set; }

        public int? Code { get; set; }

        public string? Text { get; set; }
    }

    internal sealed class ApiError
    {
        public int Code { get; set; }

        public string? Message { get; set; }

        public string? Status { get; set; }

        public List<JsonNode>? Details { get; set; }
    }
}
