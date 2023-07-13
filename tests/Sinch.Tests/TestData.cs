namespace Sinch.Tests
{
    internal class TestData
    {
        public static readonly object ActiveNumber =
            new
            {
                phoneNumber = "+12025550134",
                projectId = "51bc3f40-f266-4ca8-8938-a1ed0ff32b9a",
                displayName = "string",
                regionCode = "US",
                type = "MOBILE",
                capability = new[] { "SMS" },
                money = new
                {
                    currencyCode = "USD",
                    amount = "2.00"
                },
                paymentIntervalMonths = 0,
                nextChargeDate = "2019-08-24T14:15:22Z",
                expireAt = "2019-08-24T14:15:22Z",
                smsConfiguration = new
                {
                    servicePlanId = "string",
                    scheduledProvisioning = new
                    {
                        servicePlanId = "8200000f74924bd6800000b212f00000",
                        status = "WAITING",
                        lastUpdatedTime = "2019-08-24T14:15:22Z",
                        campaignId = "string",
                        errorCodes = new[] { "PARTNER_SERVICE_UNAVAILABLE" }
                    },
                    campaignId = "string"
                },
                voiceConfiguration = new
                {
                    appId = "string",
                    scheduledVoiceProvisioning = new
                    {
                        appId = "string",
                        status = "WAITING",
                        lastUpdatedTime = "2019-08-24T14:15:22Z"
                    },
                    lastUpdatedTime = "2019-08-24T14:15:22Z"
                }
            };

        public static object AvailableNumber = new
        {
            phoneNumber = "+12025550134",
            regionCode = "US",
            type = "MOBILE",
            capability = new[] { "SMS", "VOICE" },
            setupPrice = new
            {
                currencyCode = "USD",
                amount = "2.00"
            },
            monthlyPrice = new
            {
                currencyCode = "USD",
                amount = "2.00"
            },
            paymentIntervalMonths = 0,
            supportingDocumentationRequired = true
        };
    }
}
