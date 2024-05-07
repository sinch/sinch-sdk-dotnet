using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    public record EnumRecord(string Value)
    {
        public override string ToString()
        {
            return Value;
        }
    }

    public class EnumRecordJsonConverter<T> : JsonConverter<T> where T : EnumRecord
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Activator.CreateInstance(typeToConvert, reader.GetString()) as T ??
                   throw new InvalidOperationException("Created instance is null");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }

        // Added to properly deserialize the enum records as dictionary key
        public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Activator.CreateInstance(typeToConvert, reader.GetString()) as T ??
                   throw new InvalidOperationException("Created instance is null");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(value.Value);
        }
    }
}
