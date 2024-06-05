using System;
using System.Text.Json;
using FluentAssertions;
using Sinch.Fax.Faxes;
using Sinch.Fax.Hooks;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class HooksTests
    {
        [Fact]
        public void DeserializeIncomingFax()
        {
            string json =
                "{ \"event\": \"INCOMING_FAX\", \"eventTime\": \"2021-11-01T23:25:39Z\", \"fax\": { \"id\": \"01HDFHACK1YN7CCDYRA6ZRMA8Z\", \"direction\": \"INBOUND\", \"from\": \"+14155552222\", \"to\": \"+14155553333\", \"numberOfPages\": 1, \"status\": \"COMPLETED\", \"price\": { \"amount\": 0.07, \"currencyCode\": \"USD\" }, \"createTime\": \"2021-11-01T23:25:39Z\", \"completedTime\": \"2021-11-01T23:25:39Z\", \"callbackUrl\": \"https://www.my-website.com/callback\", \"callbackUrlContentType\": \"multipart/form-data\", \"imageConversionMethod\": \"HALFTONE\", \"projectId\": \"YOUR_PROJECT_ID\", \"serviceId\": \"YOUR_SERVICE_ID\" }, \"file\": \"ZmFzZGZkYXNkYWY=\", \"fileType\":\"PDF\" }";

            var @event = JsonSerializer.Deserialize<IFaxEvent>(json);
            var fax = @event.Should().BeOfType<IncomingFaxEvent>().Which;
            fax.EventTime.Should().Be(new DateTime(2021, 11, 01, 23, 25, 39));
            fax.Fax!.Price!.Amount.Should().Be(0.07f);
            fax.File.Should().BeEquivalentTo("ZmFzZGZkYXNkYWY=");
            fax.FileType.Should().BeEquivalentTo(FileType.PDF);
        }

        [Fact]
        public void DeserializeCompletedFax()
        {
            string json =
                "{ \"event\": \"FAX_COMPLETED\", \"eventTime\": \"2021-11-01T23:25:39Z\", \"fax\": { \"id\": \"01HDFHACK1YN7CCDYRA6ZRMA8Z\", \"direction\": \"INBOUND\", \"from\": \"+14155552222\", \"to\": \"+14155553333\", \"numberOfPages\": 1, \"status\": \"COMPLETED\", \"price\": { \"amount\": 0.07, \"currencyCode\": \"USD\" }, \"createTime\": \"2021-11-01T23:25:39Z\", \"completedTime\": \"2021-11-01T23:25:39Z\", \"callbackUrl\": \"https://www.my-website.com/callback\", \"callbackUrlContentType\": \"multipart/form-data\", \"imageConversionMethod\": \"HALFTONE\", \"projectId\": \"YOUR_PROJECT_ID\", \"serviceId\": \"YOUR_SERVICE_ID\" }, \"files\": [ {\"file\":\"ZmFzZGZkYXNkYWY=\", \"fileType\":\"PDF\" } ]}";

            var @event = JsonSerializer.Deserialize<IFaxEvent>(json);
            var fax = @event.Should().BeOfType<CompletedFaxEvent>().Which;
            fax.EventTime.Should().Be(new DateTime(2021, 11, 01, 23, 25, 39));
            fax.Fax!.Price!.Amount.Should().Be(0.07f);
        }
    }
}
