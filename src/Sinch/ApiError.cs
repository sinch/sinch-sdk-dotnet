#if NET6_0_OR_GREATER
using System.Text.Json.Nodes;
using System.Collections.Generic;
#endif


namespace Sinch
{
    internal sealed class ApiErrorResponse
    {
        public ApiError Error { get; set; }

        public string Code { get; set; }

        public string Text { get; set; }
    }

    internal sealed class ApiError
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
#if NET6_0_OR_GREATER
        public List<JsonNode> Details { get; set; }
#endif
    }
}
