using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    public sealed class UnixTimestampSecondsJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException("Expected String token type. Got " + reader.TokenType);
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                throw new JsonException(
                    "Expected Unix timestamp in seconds as a string representing a number, got an empty string.");
            if (!long.TryParse(value, out var timestamp))
            {
                throw new JsonException("Failed to parse Unix timestamp as a string to Long.");
            }

            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value,
            JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            long unixTimestamp = ((DateTimeOffset)value.Value).ToUnixTimeSeconds();
            writer.WriteStringValue(unixTimestamp.ToString());
        }
    }
}
