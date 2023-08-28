using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static string GetEnumString<T>(this T value) where T : Enum
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
        ///     Gets a enum string from EnumMember attribute
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <returns>Value of EnumMember attribute</returns>
        public static string GetEnumString(Type type, object value)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            var name = Enum.GetName(type, value)!;
            var enumMemberAttribute =
                ((EnumMemberAttribute[])type.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true))
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

        public static string ToSnakeCaseQueryString<T>(T obj) where T : class
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.DeclaredOnly);
            var list = new List<KeyValuePair<string, string>>();
            foreach (var prop in props)
            {
                if (!prop.CanRead)
                    continue;
                var propVal = prop.GetValue(obj);

                if (propVal is not null)
                {
                    // TODO: naming dynamically
                    var propName = StringUtils.ToSnakeCase(prop.Name);
                    var propType = prop.PropertyType;
                    if (typeof(IEnumerable).IsAssignableFrom(propType) &&
                        propType != typeof(string))
                    {
                        list.AddRange(ParamsFromObject(propName, propVal as IEnumerable));
                    }
                    else if (typeof(DateTime).IsAssignableFrom(propType) ||
                             typeof(DateTime?).IsAssignableFrom(propType))
                    {
                        list.Add(new(propName, StringUtils.ToIso8601((DateTime)propVal)));
                    }
                    else if (propType.IsEnum || Nullable.GetUnderlyingType(propType)?.IsEnum == true)
                    {
                        list.Add(new(propName, GetEnumString(propType, propVal)));
                    }
                    else if (typeof(bool).IsAssignableFrom(propType) ||
                             typeof(bool?).IsAssignableFrom(propType))
                    {
                        list.Add(new(propName, prop.GetValue(obj).ToString().ToLowerInvariant()));
                    }
                    else
                    {
                        list.Add(new(propName, prop.GetValue(obj).ToString()));
                    }
                }
            }

            return StringUtils.ToQueryString(list);
        }

        private static IEnumerable<KeyValuePair<string, string>> ParamsFromObject(string paramName, IEnumerable obj)
        {
            return obj.Cast<object>().Select(o =>
                new KeyValuePair<string, string>(paramName, o.ToString().ToUpperInvariant()));
        }

        private static string GetStringRepresentation(object o)
        {
            var type = o.GetType();
            if (typeof(DateTime).IsAssignableFrom(type) || typeof(DateTime?).IsAssignableFrom(type))
            {
                return StringUtils.ToIso8601((DateTime)o);
            }

            if (typeof(Enum).IsAssignableFrom(type))
            {
                return GetEnumString(type, o);
            }

            if (typeof(bool).IsAssignableFrom(type) || typeof(bool?).IsAssignableFrom(type))
            {
                return o.ToString().ToLowerInvariant();
            }

            return o.ToString();
        }
    }

    internal class SinchEnumConverter<T> : JsonConverter<T> where T : Enum
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
