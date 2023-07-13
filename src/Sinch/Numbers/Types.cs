using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    [JsonConverter(typeof(TypesEnumConverter))]
    public enum Types
    {
        /// <summary>
        ///     Numbers that belong to a specific range.
        /// </summary>
        [EnumMember(Value = "MOBILE")]
        Mobile,

        /// <summary>
        ///     Numbers that are assigned to a specific geographic region
        /// </summary>
        [EnumMember(Value = "LOCAL")]
        Local,

        /// <summary>
        ///     Number that are free of charge for the calling party but billed for all arriving calls.
        /// </summary>
        [EnumMember(Value = "TOLL_FREE")]
        TollFree
    }

    internal class TypesEnumConverter : JsonConverter<Types>
    {
        public override Types Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Utils.ParseEnum<Types>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Types value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }
}
