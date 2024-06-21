using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    ///     When your receive a callback from the fax api notifying you of a failure, it will contain two values that can help you understand what went wrong: an errorCode and an type.
    ///     The type will give you a general idea of why the operation failed, whereas the errorCode describes the issue in more detail.Below we list the error_codes for the API, segmented by their corresponding error_type.
    /// </summary>
    /// 
    [JsonConverter(typeof(EnumRecordJsonConverter<ErrorType>))]
    public record ErrorType(string Value) : EnumRecord(Value)
    {
        public static readonly ErrorType DocumentConversionError = new("DOCUMENT_CONVERSION_ERROR");
        public static readonly ErrorType CallError = new("CALL_ERROR");
        public static readonly ErrorType FaxError = new("FAX_ERROR");
        public static readonly ErrorType FatalError = new("FATAL_ERROR");
        public static readonly ErrorType GeneralError = new("GENERAL_ERROR");
    }
}
