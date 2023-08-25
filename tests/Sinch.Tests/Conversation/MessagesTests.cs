using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class MessagesTests : ConversationTestBase
    {
        [Fact]
        public async Task GetMessage()
        {
            var messageId = "123_abc";
            var responseObj = new
            {
                accept_time = "2019-08-24T14:15:22Z",
                app_message = new
                {
                    message = new
                    {
                        text = "I'm a texter"
                    },
                    explicit_channel_message = new { },
                    additionalProperties = new
                    {
                        contact_name = "string"
                    }
                },
                channel_identity = new
                {
                    app_id = "string",
                    channel = "WHATSAPP",
                    identity = "string"
                },
                contact_id = "string",
                contact_message = new
                {
                    choice_response_message = new
                    {
                        message_id = "string",
                        postback_data = "string"
                    },
                    fallback_message = new
                    {
                        raw_message = "string",
                        reason = new { }
                    },
                    location_message = new
                    {
                        coordinates = new { },
                        label = "string",
                        title = "string"
                    },
                    media_card_message = new
                    {
                        caption = "string",
                        url = "string"
                    },
                    media_message = new
                    {
                        thumbnail_url = "string",
                        url = "string"
                    },
                    reply_to = new
                    {
                        message_id = "string"
                    },
                    text_message = new
                    {
                        text = "string"
                    }
                },
                conversation_id = "string",
                direction = "UNDEFINED_DIRECTION",
                id = "string",
                metadata = "string",
                injected = true
            };
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages/{messageId}")
                .WithQueryString("messages_source", "CONVERSATION_SOURCE")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(responseObj));

            var response = await Conversation.Messages.Get(messageId, MessageSource.ConversationSource);

            response.Should().NotBeNull();
        }
    }
}
