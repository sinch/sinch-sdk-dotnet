using System;
using System.Globalization;

namespace Sinch.Tests.Features
{
    internal static  class Helpers
    {
        public static DateTime ParseUtc(string time)
        {
            return DateTime.Parse(time, CultureInfo.InvariantCulture).ToUniversalTime();
        }
    }
}
