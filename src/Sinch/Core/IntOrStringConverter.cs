using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    /// <summary>
    /// Reads a JSON value that is either a number or a string into a <see cref="string"/>.
    /// Writes as a JSON string.
    /// </summary>
    internal sealed class IntOrStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetInt64().ToString()
                : reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
