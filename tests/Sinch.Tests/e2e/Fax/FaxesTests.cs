using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Fax.Faxes;
using Xunit;

namespace Sinch.Tests.e2e.Fax
{
    public class FaxesTests : FaxTestBase
    {
        [Fact]
        public async Task SendContentUrls()
        {
            var response = await FaxClient.Faxes.Send(new SendFaxRequest(new List<string>()
            {
                "http://fax-db/fax1.pdf",
                "http://fax-db/fax2.pdf",
            })
            {
                To = "+12015555555"
            });
            response.Should().BeEquivalentTo(new Sinch.Fax.Faxes.Fax()
            {
                Id = "01HXVD9FPQ8MAJ2650W0KTY7D4",
                Direction = Direction.Outbound,
                Status = FaxStatus.InProgress,
                CreateTime = DateTime.Parse("2024-05-14T11:20:05Z", null, DateTimeStyles.AdjustToUniversal),
                HeaderPageNumbers = true,
                HeaderTimeZone = "America/New_York",
                ServiceId = "01HXGS1GE2SXS6HKQDMPYM1JHY",
                ProjectId = ProjectId,
                MaxRetries = 3,
                ImageConversionMethod = ImageConversionMethod.Halftone,
                To = "+12015555555",
                RetryDelaySeconds = 60,
                ContentUrl = new List<string>()
                {
                    "http://fax-db/fax1.pdf",
                    "http://fax-db/fax2.pdf",
                }
            });
        }
        
        [Fact]
        public async Task GetFax()
        {
            var fax = await FaxClient.Faxes.Get("01HXVD9FPQ8MAJ2650W0KTY7D4");
            fax.Should().BeEquivalentTo(new Sinch.Fax.Faxes.Fax()
            {
                Id = "01HXVD9FPQ8MAJ2650W0KTY7D4",
                Direction = Direction.Outbound,
                To = "+12015555555",
                ContentUrl = new List<string>()
                {
                    "https://developers.sinch.com/fax/fax.pdf",
                    "https://developers.sinch.com/fax/fax.pdf"
                },
                NumberOfPages = 0,
                Status = FaxStatus.InProgress,
                CreateTime = DateTime.Parse("2024-05-14T11:20:05Z", null, DateTimeStyles.AdjustToUniversal),
                CompletedTime = DateTime.Parse("0001-01-01T00:00:00", null, DateTimeStyles.AdjustToUniversal),
                HeaderPageNumbers = true,
                HeaderTimeZone = "America/New_York",
                RetryDelaySeconds = 60,
                ImageConversionMethod = ImageConversionMethod.Halftone,
                ServiceId = "01HXGS1GE2SXS6HKQDMPYM1JHY",
                MaxRetries = 3,
                HasFile = false,
                ProjectId = ProjectId
            });
        }
    }
}
