using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers;
using Sinch.Numbers.Regions;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class AvailableRegions
    {
        private ISinchNumbersRegions _sinchNumbersRegions;
        private IEnumerable<Region> _regionsResponse;
        private IEnumerable<Types> _regionTypes;

        [Given(@"the Numbers service ""Regions"" is available")]
        public void GivenTheNumbersServiceIsAvailable()
        {
            _sinchNumbersRegions = Utils.SinchNumbersClient().Regions;
        }

        [When(@"I send a request to list all the regions")]
        public async Task WhenISendARequestToListAllTheRegions()
        {
            _regionsResponse = await _sinchNumbersRegions.List();
        }

        [Then(@"the response contains ""(.*)"" regions")]
        public void ThenTheResponseContainsRegions(int count)
        {
            _regionsResponse.Should().HaveCount(count);
            _regionTypes = _regionsResponse.SelectMany(x => x.Types);
        }

        [Then(@"the response contains ""(.*)"" TOLL_FREE regions")]
        public void ThenTheResponseContainsTollFreeRegions(int count)
        {
            _regionTypes.Where(x => x == Types.TollFree).Should().HaveCount(count);
        }

        [Then(@"the response contains ""(.*)"" MOBILE regions")]
        public void ThenTheResponseContainsMobileRegions(int count)
        {
            _regionTypes.Where(x => x == Types.Mobile).Should().HaveCount(count);
        }

        [Then(@"the response contains ""(.*)"" LOCAL regions")]
        public void ThenTheResponseContainsLocalRegions(int count)
        {
            _regionTypes.Where(x => x == Types.Local).Should().HaveCount(count);
        }

        [When(@"I send a request to list the TOLL_FREE regions")]
        public async Task WhenISendARequestToListTheTollFreeRegions()
        {
            _regionsResponse = await _sinchNumbersRegions.List([Types.TollFree]);
        }

        [When(@"I send a request to list the TOLL_FREE or MOBILE regions")]
        public async Task WhenISendARequestToListTheTollFreeOrMobileRegions()
        {
            _regionsResponse = await _sinchNumbersRegions.List([Types.TollFree, Types.Mobile]);
            _regionTypes = _regionsResponse.SelectMany(x => x.Types);
        }
    }
}
