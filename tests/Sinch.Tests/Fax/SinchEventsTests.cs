using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Sinch.Fax.Faxes;
using Sinch.Fax.SinchEvents;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class SinchEventsTests
    {
        private const string IncomingFaxJson =
            "{ \"event\": \"INCOMING_FAX\", \"eventTime\": \"2021-11-01T23:25:39Z\", \"fax\": { \"id\": \"01HDFHACK1YN7CCDYRA6ZRMA8Z\", \"direction\": \"INBOUND\", \"from\": \"+14155552222\", \"to\": \"+14155553333\", \"numberOfPages\": 1, \"status\": \"COMPLETED\", \"price\": { \"amount\": 0.07, \"currencyCode\": \"USD\" }, \"createTime\": \"2021-11-01T23:25:39Z\", \"completedTime\": \"2021-11-01T23:25:39Z\", \"callbackUrl\": \"https://my-server.com/fax-events\", \"callbackUrlContentType\": \"multipart/form-data\", \"imageConversionMethod\": \"HALFTONE\", \"projectId\": \"YOUR_PROJECT_ID\", \"serviceId\": \"YOUR_SERVICE_ID\" }, \"file\": \"ZmFzZGZkYXNkYWY=\", \"fileType\":\"PDF\" }";

        private const string CompletedFaxJson =
            "{ \"event\": \"FAX_COMPLETED\", \"eventTime\": \"2021-11-01T23:25:39Z\", \"fax\": { \"id\": \"01HDFHACK1YN7CCDYRA6ZRMA8Z\", \"direction\": \"INBOUND\", \"from\": \"+14155552222\", \"to\": \"+14155553333\", \"numberOfPages\": 1, \"status\": \"COMPLETED\", \"price\": { \"amount\": 0.07, \"currencyCode\": \"USD\" }, \"createTime\": \"2021-11-01T23:25:39Z\", \"completedTime\": \"2021-11-01T23:25:39Z\", \"callbackUrl\": \"https://my-server.com/fax-events\", \"callbackUrlContentType\": \"multipart/form-data\", \"imageConversionMethod\": \"HALFTONE\", \"projectId\": \"YOUR_PROJECT_ID\", \"serviceId\": \"YOUR_SERVICE_ID\" }, \"files\": [ {\"file\":\"ZmFzZGZkYXNkYWY=\", \"fileType\":\"PDF\" } ]}";

        [Fact]
        public void DeserializeIncomingFax()
        {
            var @event = JsonSerializer.Deserialize<IFaxSinchEvent>(IncomingFaxJson);
            var fax = @event.Should().BeOfType<IncomingFaxEvent>().Which;
            fax.EventTime.Should().Be(new DateTime(2021, 11, 01, 23, 25, 39));
            fax.Fax!.Price!.Amount.Should().Be(0.07f);
            fax.File.Should().BeEquivalentTo("ZmFzZGZkYXNkYWY=");
            fax.FileType.Should().BeEquivalentTo(FileType.PDF);
        }

        [Fact]
        public void DeserializeCompletedFax()
        {
            var @event = JsonSerializer.Deserialize<IFaxSinchEvent>(CompletedFaxJson);
            var fax = @event.Should().BeOfType<CompletedFaxEvent>().Which;
            fax.EventTime.Should().Be(new DateTime(2021, 11, 01, 23, 25, 39));
            fax.Fax!.Price!.Amount.Should().Be(0.07f);
        }

        [Fact]
        public void ParseEvent_ReturnsIncomingFaxEvent()
        {
            var sinchEvents = new FaxSinchEvents(new JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
            var result = sinchEvents.ParseEvent(IncomingFaxJson);
            result.Should().BeOfType<IncomingFaxEvent>();
        }

        [Fact]
        public void ParseEvent_ThrowsOnNull()
        {
            var sinchEvents = new FaxSinchEvents(new JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
            var act = () => sinchEvents.ParseEvent("null");
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Fax Sinch Event*");
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsFalse_WhenSecretWrong()
        {
            var sinchEvents = new FaxSinchEvents(new JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                { "x-sinch-signature", new[] { "invalidsignature" } }
            };
            var result = sinchEvents.ValidateAuthenticationHeader("wrong-secret", headers, IncomingFaxJson);
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsFalse_WhenHeaderMissing()
        {
            var sinchEvents = new FaxSinchEvents(new JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
            var result = sinchEvents.ValidateAuthenticationHeader("secret", new Dictionary<string, IEnumerable<string>>(), "{}");
            result.Should().BeFalse();
        }

        [Fact]
        public void SinchClient_WithoutCredentials_CanParseFaxSinchEvent()
        {
            var client = new SinchClient(new SinchClientConfiguration());
            var ev = client.Fax.SinchEvents.ParseEvent(IncomingFaxJson);
            ev.Should().NotBeNull();
        }
    }
}
