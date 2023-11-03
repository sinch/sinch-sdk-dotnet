using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Sinch.Numbers;
using Sinch.Numbers.Available.Rent;
using Xunit;

namespace Sinch.Tests.e2e
{
    public class NumbersTests : TestBase
    {
        [Fact]
        public async Task ListRegions()
        {
            var response = await SinchClientMockStudio.Numbers.Regions.List(new List<Types>() { Types.Local, Types.Mobile });
            response = response.ToList();
            response.Should().HaveCount(18);
            var brRegion = response.First(x => x.RegionCode == "CL");
            brRegion.RegionName.Should().Be("Chile");
            brRegion.Types.Should().Contain(new[] { Types.Mobile, Types.Local });
        }

        [Fact]
        public async Task Rent()
        {
            var response = await SinchClientMockStudio.Numbers.Available.Rent("+447520652221", new RentActiveNumberRequest()
            {
                SmsConfiguration = null,
                VoiceConfiguration = null,
            });
            response.RegionCode.Should().Be("GB");
            response.NextChargeDate.Should().Be(28.October(2022).At(13, 54, 54, 247));
        }

        [Fact]
        public async Task ListAvailableWithPattern()
        {
            var response = await SinchClientMockStudio.Numbers.Available.List(new Sinch.Numbers.Available.List.ListAvailableNumbersRequest
            {
                RegionCode = "US",
                Type = Types.Local,
                NumberPattern = new NumberPattern
                {
                    Pattern = "122",
                    SearchPattern = SearchPattern.End
                }
            });
            response.AvailableNumbers.Should().HaveCount(5);
        }

        [Fact]
        public async Task ListAvailableWithCapabilities()
        {
            var response = await SinchClientMockStudio.Numbers.Available.List(new Sinch.Numbers.Available.List.ListAvailableNumbersRequest
            {
                RegionCode = "US",
                Type = Types.Local,
                Capabilities = new List<Product>()
                {
                    Product.Voice
                }
            });
            response.AvailableNumbers.Should().HaveCount(5);
        }

        [Fact]
        public async Task ListActiveWithPageSize()
        {
            var response = await SinchClientMockStudio.Numbers.Active.List(new Sinch.Numbers.Active.List.ListActiveNumbersRequest
            {
                RegionCode = "GB",
                Type = Types.Mobile,
                PageSize = 1
            });
            response.TotalSize.Should().Be(6);
            response.ActiveNumbers.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListActiveWithPageToken()
        {
            var response = await SinchClientMockStudio.Numbers.Active.List(new Sinch.Numbers.Active.List.ListActiveNumbersRequest
            {
                RegionCode = "GB",
                Type = Types.Mobile,
                PageSize = 1,
                PageToken =
                    "CgtwaG9uZU51bWJlchJoCjl0eXBlLmdvb2dsZWFwaXMuY29tL3NpbmNoLnVuaWZpZWRudW1iZXIudjEuTnVtYmVyUGFnZU1hcmsSKwoaMDFnZTI0Z3JwejFjcXcyN2c4MnBoZnEzN3oSDSs0NDc1MjA2NTI2NDI=",
            });
            response.TotalSize.Should().Be(6);
            response.ActiveNumbers.Should().HaveCount(1);
            response.NextPageToken.Should().BeEmpty();
        }

        [Fact]
        public async Task GetActive()
        {
            var response = await SinchClientMockStudio.Numbers.Active.Get("+447520650626");
            response.ExpireAt.Should().Be(8.October(2022).At(7, 52, 49, 454));
        }

        [Fact]
        public async Task Release()
        {
            var response = await SinchClientMockStudio.Numbers.Active.Release("+447520650626");
            response.PhoneNumber.Should().Be("+447520650626");
        }
    }
}
