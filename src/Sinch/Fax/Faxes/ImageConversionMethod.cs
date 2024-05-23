using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// Determines how documents are converted to black and white. Defaults to value selected on Fax Service object.
    /// </summary>
    /// <value>Determines how documents are converted to black and white. Defaults to value selected on Fax Service object.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<ImageConversionMethod>))]
    public record ImageConversionMethod(string Value) : EnumRecord(Value)
    {
        public static readonly ImageConversionMethod Halftone = new("HALFTONE");
        public static readonly ImageConversionMethod Monochrome = new("MONOCHROME");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
