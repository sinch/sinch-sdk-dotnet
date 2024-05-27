using System;

namespace Sinch.Core
{
    internal static class ExceptionUtils
    {
        public static void CheckEmptyString(string paramName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName, "Should have a value");
            }
        }
    }
}
