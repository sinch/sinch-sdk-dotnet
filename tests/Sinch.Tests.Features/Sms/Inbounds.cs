using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS.Inbounds;
using Sinch.SMS.Inbounds.List;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class Inbounds
    {
        private ISinchSmsInbounds _inbounds;
        private IInbound _inbound;
        private ListInboundsResponse _list;
        private List<IInbound> _listInbounds;
        private int _totalPages;

        [Given(@"the SMS service ""Inbounds"" is available")]
        public void GivenTheSmsServiceIsAvailable()
        {
            _inbounds = Utils.SinchClient.Sms.Inbounds;
        }

        [When(@"I send a request to retrieve an inbound message")]
        public async Task WhenISendARequestToRetrieveAnInboundMessage()
        {
            _inbound = await _inbounds.Get("01W4FFL35P4NC4K35INBOUND01");
        }

        [Then(@"the response contains the inbound message details")]
        public void ThenTheResponseContainsTheInboundMessageDetails()
        {
            _inbound.As<SmsInbound>().Should().BeEquivalentTo(new SmsInbound
            {
                Id = "01W4FFL35P4NC4K35INBOUND01",
                From = "12015555555",
                To = "12017777777",
                Body = "Hello John!",
                OperatorId = "311071",
                ReceivedAt = Helpers.ParseUtc("2024-06-06T14:16:54.777Z")
            });
        }

        [When(@"I send a request to list the inbound messages")]
        public async Task WhenISendARequestToListTheInboundMessages()
        {
            _list = await _inbounds.List(new ListInboundsRequest()
            {
                PageSize = 2,
                To = ["12017777777", "12018888888"]
            });
        }

        [Then(@"the response contains ""(.*)"" inbound messages")]
        public void ThenTheResponseContainsInboundMessages(int count)
        {
            _list.Inbounds.Should().HaveCount(count);
        }

        [When(@"I send a request to list all the inbound messages")]
        public async Task WhenISendARequestToListAllTheInboundMessages()
        {
            var allList = _inbounds.ListAuto(new ListInboundsRequest()
            {
                PageSize = 2,
                To = ["12017777777", "12018888888"]
            });
            _listInbounds = new List<IInbound>();
            await foreach (var inbound in allList)
            {
                _listInbounds.Add(inbound);
            }
        }

        [Then(@"the inbound messages list contains ""(.*)"" inbound messages")]
        public void ThenTheInboundMessagesListContainsInboundMessages(int count)
        {
            _listInbounds.Should().HaveCount(count);
        }

        [When(@"I iterate manually over the inbound messages pages")]
        public async Task WhenIIterateManuallyOverTheInboundMessagesPages()
        {
            //TODO(DEVEXP-992): implement iterator
            _listInbounds = new List<IInbound>();
            _totalPages = 0;
            ListInboundsResponse listBatchesResponse = null;
            while (true)
            {
                listBatchesResponse = await _inbounds.List(new ListInboundsRequest()
                {
                    PageSize = 2,
                    Page = listBatchesResponse?.Page + 1,
                    To = ["12017777777", "12018888888"]
                });
                if (listBatchesResponse?.Inbounds?.Count() == 0) break;
                _listInbounds.AddRange(listBatchesResponse.Inbounds!);
                _totalPages++;
            }
        }

        [Then(@"the inbound messages iteration result contains the data from ""(.*)"" pages")]
        public void ThenTheInboundMessagesIterationResultContainsTheDataFromPages(int count)
        {
            _totalPages.Should().Be(count);
        }
    }
}
