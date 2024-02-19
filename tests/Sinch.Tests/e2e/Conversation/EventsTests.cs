using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.AppEvents;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class EventsTests : TestBase
    {
        [Fact]
        public async Task SendRequired()
        {
            var response = await SinchClientMockServer.Conversation.Events.Send(new SendEventRequest
            {
                AppId = "app_id",
                Event = new AppEvent(new ComposingEvent()),
                Recipient = new ContactRecipient()
                {
                    ContactId = "Hey yo"
                },
            });
            response.Should().BeEquivalentTo(new object());
        }
    }
}
