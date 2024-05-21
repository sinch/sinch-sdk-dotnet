using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

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
            return GetEnumString(typeof(T), value);
        }

        /// <summary>
        ///     Gets a enum string from EnumMember attribute
        /// </summary>
        /// <returns>Value of EnumMember attribute</returns>
        private static string GetEnumString(Type type, object value)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            var name = Enum.GetName(type, value)!;
            var enumMemberAttribute =
                ((EnumMemberAttribute[])type.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .SingleOrDefault();

            if (enumMemberAttribute == null)
            {
                return value.ToString() ?? throw new InvalidOperationException("value.ToString() is null");
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

            throw new InvalidOperationException($"Failed to parse {enumType.Name} enum for value {value}");
        }

        public static bool IsLastPage(int page, int pageSize, int totalCount, PageStart pageStart = PageStart.Zero)
        {
            switch (pageStart)
            {
                case PageStart.Zero:
                    page += 1;
                    break;
                case PageStart.First:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pageStart), pageStart, null);
            }

            return page * pageSize >= totalCount;
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

                if (propVal is null) continue;

                var propName = StringUtils.ToSnakeCase(prop.Name);
                var propType = prop.PropertyType;
                if (typeof(IEnumerable).IsAssignableFrom(propType) &&
                    propType != typeof(string))
                {
                    list.AddRange(ParamsFromObject(propName, (propVal as IEnumerable)!));
                }
                else
                {
                    list.Add(new(propName, ToQueryParamString(propVal)));
                }
            }

            return StringUtils.ToQueryString(list);
        }

        public static string ToQueryString<T>(T obj, Func<string?, string?> namingConverter)
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.DeclaredOnly);
            var list = new List<KeyValuePair<string, string>>();
            foreach (var prop in props)
            {
                if (!prop.CanRead)
                    continue;
                var propVal = prop.GetValue(obj);

                if (propVal is null) continue;

                var propName = namingConverter.Invoke(prop.Name);
                if (string.IsNullOrEmpty(propName))
                {
                    continue;
                }

                var propType = prop.PropertyType;
                if (typeof(IEnumerable).IsAssignableFrom(propType) &&
                    propType != typeof(string))
                {
                    list.AddRange(ParamsFromObject(propName, (propVal as IEnumerable)!));
                }
                else
                {
                    list.Add(new(propName, ToQueryParamString(propVal)));
                }
            }

            return StringUtils.ToQueryString(list);
        }

        private static IEnumerable<KeyValuePair<string, string>> ParamsFromObject(string paramName, IEnumerable obj)
        {
            return obj.Cast<object>().Select(o =>
                new KeyValuePair<string, string>(paramName, ToQueryParamString(o)));
        }

        private static string ToQueryParamString(object o)
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
                return o.ToString()?.ToLowerInvariant()!;
            }

            if (typeof(EnumRecord).IsAssignableFrom(type))
            {
                return (type.GetProperty("Value", typeof(string))?.GetValue(o) as string)!;
            }

            return o.ToString()!;
        }

        public static void ThrowNullDeserialization(Type type)
        {
            throw new InvalidOperationException($"{type.Name} deserialization result is null");
        }
    }

    internal enum PageStart
    {
        Zero = 0,
        First = 1,
    }
}
