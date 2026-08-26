using System;
using System.Globalization;

namespace Sinch.Tests.Features
{
    internal static class Helpers
    {
        public const string MOCKSERVER_BASE_URL = "https://sinch-sdk-mockserver.sliplane.app";

        public const string MOCKSERVER_AUTH_URL = MOCKSERVER_BASE_URL + "/authentication/";
        public const string MOCKSERVER_NUMBERS_URL = MOCKSERVER_BASE_URL + "/numbers/";
        public const string MOCKSERVER_SMS_URL = MOCKSERVER_BASE_URL + "/sms/";
        public const string MOCKSERVER_VERIFICATION_URL = MOCKSERVER_BASE_URL + "/verification/";
        public const string MOCKSERVER_VOICE_URL = MOCKSERVER_BASE_URL + "/voice/";
        public const string MOCKSERVER_VOICE_APPLICATION_MANAGEMENT_URL = MOCKSERVER_BASE_URL + "/voice-application-management/";

        public static DateTime ParseUtc(string time)
        {
            return DateTime.Parse(time, CultureInfo.InvariantCulture).ToUniversalTime();
        }
    }
}
