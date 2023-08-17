using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    internal static class Utils
    {
        /// <summary>
        ///     Gets a enum string from EnumMember attribute
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <returns>Value of EnumMember attribute</returns>
        public static string GetEnumString<T>(T value)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, value)!;
            var enumMemberAttribute =
                ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .SingleOrDefault();

            if (enumMemberAttribute == null)
            {
                return value.ToString();
            }

            return enumMemberAttribute.Value!;
        }

        /// <summary>
        ///     Matches string to enum if possible
        /// </summary>
        /// <param name="value">The string to match</param>
        /// <typeparam name="T">Enum to match</typeparam>
        /// <returns>Enum Value</returns>
        /// <exception cref="NullReferenceException">Throws if values is null</exception>
        /// <exception cref="InvalidOperationException">Throws if cannot convert value to enum</exception>
        public static T ParseEnum<T>(string value)
        {
            if (value is null) throw new NullReferenceException("a value is null");

            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute =
                    ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute),
                        true))
                    .SingleOrDefault();
                // if EnumMember is missing, try match string representation of enum
                if (enumMemberAttribute == null && name == value)
                {
                    return (T)Enum.Parse(enumType, name);
                }

                if (enumMemberAttribute != null && enumMemberAttribute.Value == value)
                {
                    return (T)Enum.Parse(enumType, name);
                }
            }

            throw new InvalidOperationException($"Failed to parse {nameof(T)} enum");
        }

        public static bool IsLastPage(int page, int pageSize, int totalCount)
        {
            return (page + 1) * pageSize >= totalCount;
        }
    }

    internal class SinchEnumConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Utils.ParseEnum<T>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }
}
