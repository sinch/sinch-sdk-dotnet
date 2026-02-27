using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Sinch
{
    internal abstract class ApiErrorResponseBase
    {
        public ApiError? Error { get; set; }

        public string? Text { get; set; }

        public abstract string GetErrorCode();
    }

    internal sealed class ApiErrorResponse : ApiErrorResponseBase
    {
        public int? Code { get; init; }

        public override string GetErrorCode()
        {
            return Code?.ToString() ?? string.Empty;
        }
    }

    internal sealed class ApiSmsErrorResponse : ApiErrorResponseBase
    {
        public string? Code { get; init; }

        public override string GetErrorCode()
        {
            return Code ?? string.Empty;
        }
    }

    internal sealed class ApiError
    {
        public int Code { get; set; }

        public string? Message { get; set; }

        public string? Status { get; set; }

        public List<JsonNode>? Details { get; set; }
    }
}
