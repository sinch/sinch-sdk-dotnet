using System;
using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Extensions;
using Sinch.Verification.Common;
using Sinch.Verification.Status;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationStatusTests
    {

        public static WhatsAppVerificationStatusResponse whatsAppVerificationStatusResponse = new WhatsAppVerificationStatusResponse()
        {
            Id = "the id",
            Status = VerificationStatus.Fail,
            Reason = Reason.Fraud,
            Reference = "my reference",
            Identity = Identity.Number("+33123456789"),
            CountryId = "es-ES",
            VerificationTimestamp = new DateTime(2024, 5, 22, 9, 38, 59, 559, DateTimeKind.Utc).AddNanoseconds(43700),
            Source = Source.Intercepted,
            Price = new Price
            {
                VerificationPrice = new PriceDetail
                {
                    CurrencyId = "verificationPrice currency id",
                    Amount = 3.141516
                }
            }
        };

        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };


        [Fact]
        public void DeSerializeVerificationStatusWhatsAppResponse()
        {
            var data = Helpers.LoadResources("Verification/Status/VerificationStatusResponseWhatsAppDto.json");
            var response = JsonSerializer.Deserialize<IVerificationStatusResponse>(data, _jsonSerializerOptions);

            var actual = response.Should().BeOfType<WhatsAppVerificationStatusResponse>().Subject;
            Helpers.BeEquivalentToWithJsonElement(actual, whatsAppVerificationStatusResponse);
        }
    }
}
