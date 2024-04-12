using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    public class JsonObjectAsStringJsonConverter : JsonConverter<JsonObject>
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
