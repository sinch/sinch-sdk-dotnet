using System;
using System.Linq;
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
        public static string GetEnumString<T>(T value)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, value)!;
            var enumMemberAttribute =
                ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .Single();
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
                    .Single();
                if (enumMemberAttribute.Value == value) return (T)Enum.Parse(enumType, name);
            }

            throw new InvalidOperationException($"Failed to parse {nameof(T)} enum");
        }

        public static bool IsLastPage(int page, int pageSize, int totalCount)
        {
            return (page + 1) * pageSize >= totalCount;
        }
    }
}
