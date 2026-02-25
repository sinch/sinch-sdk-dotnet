using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class CapabilityTests : ConversationTestBase
    {
        private readonly string _url =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/capability:query";

        [Fact]
        public async Task Lookup_WithContactRecipient_ReturnsRequestId()
        {
            var expectedRequest = new
            {
                app_id = "01W4FFL35P4NC4K35CONVAPP001",
                recipient = new
                {
                    contact_id = "01W4FFL35P4NC4K35CONTACT001"
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, _url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    app_id = "01W4FFL35P4NC4K35CONVAPP001",
                    recipient = new
                    {
                        contact_id = "01W4FFL35P4NC4K35CONTACT001"
                    },
                    request_id = "01W4FFL35P4NC4K35CAPABILITY"
                }));

            var response = await Conversation.Capabilities.Lookup(new LookupCapabilityRequest
            {
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Recipient = new ContactRecipient
                {
                    ContactId = "01W4FFL35P4NC4K35CONTACT001"
                }
            });

            response.Should().NotBeNull();
            response.AppId.Should().Be("01W4FFL35P4NC4K35CONVAPP001");
            response.RequestId.Should().Be("01W4FFL35P4NC4K35CAPABILITY");
            var contactRecipient = response.Recipient.Should().BeOfType<ContactRecipient>().Subject;
            contactRecipient.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
        }
    }
}
