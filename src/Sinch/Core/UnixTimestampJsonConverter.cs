using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    public sealed class UnixTimestampJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;
            var value = reader.GetString();
            if (value == null)
                throw new JsonException("Expected Unix timestamp as a string.");
            if (!long.TryParse(value, out var timestamp))
            {
                throw new JsonException("Expected Unix timestamp as a number.");
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
