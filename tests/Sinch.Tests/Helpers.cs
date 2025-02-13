using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Sinch.Tests
{
    internal static class Helpers
    {
        public static T GetPrivateField<T, TInstance>(TInstance instance, string fieldName)
        {
            var field = instance!.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field!.GetValue(instance)!;
        }

        public static DateTime ParseUtc(string time)
        {
            return DateTime.Parse(time, CultureInfo.InvariantCulture).ToUniversalTime();
        }

        /// <summary>
        ///     Loads a file from provided path nested in /Resources folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadResources(string path)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", path);
            return File.ReadAllText(filePath);
        }
    }
}
