using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Send;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class MessagesTests : ConversationTestBase
    {
        [Fact]
        public async Task SendSimple()
        {
            const string expectedJson = @"{
                                        'app_id':'123', 
                                        'message': {
                                            'text_message': { 
                                                'text': 'I\'m a texter' 
                                            }
                                        },
                                        'recipient': { 'contact_id': 'ContactEasy' }
                                        }";
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:send")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(expectedJson)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    accepted_time = "2019-08-24T14:15:22Z",
                    message_id = "string"
                }));

            var response = await Conversation.Messages.Send(new Request
            {
                AppId = "123",
                Message = new AppMessage(new TextMessage
                {
                    Text = "I'm a texter"
                })
                {
                    ExplicitChannelMessage = null,
                    AdditionalProperties = null
                },
                Recipient = new Contact()
                {
                    ContactId = "ContactEasy"
                }
            });

            response.MessageId.Should().Be("string");
            response.AcceptedTime.Should().HaveMinute(15);
        }
    }
}
