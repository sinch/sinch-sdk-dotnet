using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    /// <summary>
    ///     Converts JsonNode to String if it's just a string, and to json escaped string if it's an object.
    /// </summary>
    public sealed class JsonNodeAsStringJsonConverter : JsonConverter<JsonNode>
    {
        public override JsonNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException("Read is not supported");
        }

        public override void Write(Utf8JsonWriter writer, JsonNode value, JsonSerializerOptions options)
        {
            var result = value.GetValueKind() switch
            {
                JsonValueKind.Object => value.ToJsonString(new JsonSerializerOptions(options)
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }),
                JsonValueKind.String => value.ToString(),
                _ => throw new ArgumentOutOfRangeException(
                    $"Expected json object or string, found {value.GetValueKind()}")
            };
            JsonSerializer.Serialize(writer, result, options);
        }
    }

    // JsonNode variant not working for object
    public sealed class JsonObjectAsStringJsonConverter : JsonConverter<JsonObject>
    {
        public override JsonObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = reader.GetString();
            if (json is null)
            {
                return null;
            }

            return JsonNode.Parse(json)?.AsObject();
        }

        public override void Write(Utf8JsonWriter writer, JsonObject value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToJsonString(), options);
        }
    }
}
