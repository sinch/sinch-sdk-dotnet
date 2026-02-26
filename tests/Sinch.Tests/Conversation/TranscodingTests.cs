using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class TranscodingTests : ConversationTestBase
    {
        private const string TranscodeUrl = $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:transcode";

        [Fact]
        public async Task Transcode_TextMessage_ReturnsTranscodedMessage()
        {
            var expectedRequest = new
            {
                app_id = "APP_001",
                app_message = new
                {
                    text_message = new
                    {
                        text = "Hello, World!"
                    }
                },
                channels = new[] { "SMS", "WHATSAPP" }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, TranscodeUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    transcoded_message = new Dictionary<string, string>
                    {
                        ["SMS"] = "Hello, World!",
                        ["WHATSAPP"] = "{\"type\":\"text\",\"text\":{\"body\":\"Hello, World!\"}}"
                    }
                }));

            var response = await Conversation.Transcoding.Transcode(new TranscodeRequest
            {
                AppId = "APP_001",
                AppMessage = new AppMessage(new TextMessage("Hello, World!")),
                Channels =
                [
                    ConversationChannel.Sms,
                    ConversationChannel.WhatsApp
                ]
            });

            response.Should().NotBeNull();
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.Sms);
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.WhatsApp);
            response.TranscodedMessage![ConversationChannel.Sms].Should().Be("Hello, World!");
            var whatsAppPayload = JsonSerializer.Deserialize<JsonElement>(
                    response.TranscodedMessage[ConversationChannel.WhatsApp]);
            whatsAppPayload.GetProperty("type").GetString().Should().Be("text");
            whatsAppPayload.GetProperty("text").GetProperty("body").GetString().Should().Be("Hello, World!");
        }

        [Fact]
        public async Task Transcode_WithFromAndTo_IncludesOptionalFields()
        {
            var expectedRequest = new
            {
                app_id = "APP_001",
                app_message = new
                {
                    text_message = new
                    {
                        text = "Hi"
                    }
                },
                channels = new[] { "SMS" },
                from = "+12025550001",
                to = "+12025550002"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, TranscodeUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    transcoded_message = new Dictionary<string, string>
                    {
                        ["SMS"] = "Hi"
                    }
                }));

            var response = await Conversation.Transcoding.Transcode(new TranscodeRequest
            {
                AppId = "APP_001",
                AppMessage = new AppMessage(new TextMessage("Hi")),
                Channels = [ConversationChannel.Sms],
                From = "+12025550001",
                To = "+12025550002"
            });

            response.Should().NotBeNull();
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.Sms);
            response.TranscodedMessage![ConversationChannel.Sms].Should().Be("Hi");
        }
    }
}
