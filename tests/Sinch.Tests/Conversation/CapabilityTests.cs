using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class CapabilityTests : ConversationTestBase
    {
        private const string CapabilityUrl = $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/capability:query";

        [Fact]
        public async Task Lookup_WithContactRecipient_ReturnsExpectedResponse()
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
                .When(HttpMethod.Post, CapabilityUrl)
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

        [Fact]
        public async Task Lookup_WithIdentifiedRecipient_ReturnsExpectedResponse()
        {
            var expectedRequest = new
            {
                app_id = "01W4FFL35P4NC4K35CONVAPP001",
                recipient = new
                {
                    identified_by = new
                    {
                        channel_identities = new[]
                        {
                            new { channel = "SMS", identity = "+12345678900" }
                        }
                    }
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, CapabilityUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    app_id = "01W4FFL35P4NC4K35CONVAPP001",
                    recipient = new
                    {
                        identified_by = new
                        {
                            channel_identities = new[]
                            {
                                new { channel = "SMS", identity = "+12345678900" }
                            }
                        }
                    },
                    request_id = "01W4FFL35P4NC4K35CAPABILITY"
                }));

            var response = await Conversation.Capabilities.Lookup(new LookupCapabilityRequest
            {
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Recipient = new Identified
                {
                    IdentifiedBy = new IdentifiedBy
                    {
                        ChannelIdentities =
                        [
                            new() { Channel = ConversationChannel.Sms, Identity = "+12345678900" }
                        ]
                    }
                }
            });

            response.Should().NotBeNull();
            response.AppId.Should().Be("01W4FFL35P4NC4K35CONVAPP001");
            response.RequestId.Should().Be("01W4FFL35P4NC4K35CAPABILITY");
            var identified = response.Recipient.Should().BeOfType<Identified>().Subject;
            identified.IdentifiedBy!.ChannelIdentities.Should().ContainSingle();
            var channelIdentity = identified.IdentifiedBy.ChannelIdentities![0];
            channelIdentity.Channel.Should().Be(ConversationChannel.Sms);
            channelIdentity.Identity.Should().Be("+12345678900");
        }
    }
}
