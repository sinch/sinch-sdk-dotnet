using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class TranscodingTests : ConversationTestBase
    {
        private readonly string _transcodeUrl =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:transcode";

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
                .When(HttpMethod.Post, _transcodeUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(expectedRequest))
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
                Channels = new List<ConversationChannel>
                {
                    ConversationChannel.Sms,
                    ConversationChannel.WhatsApp
                }
            });

            response.Should().NotBeNull();
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.Sms);
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.WhatsApp);
            response.TranscodedMessage![ConversationChannel.Sms].Should().Be("Hello, World!");
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
                .When(HttpMethod.Post, _transcodeUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(expectedRequest))
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
                Channels = new List<ConversationChannel> { ConversationChannel.Sms },
                From = "+12025550001",
                To = "+12025550002"
            });

            response.Should().NotBeNull();
            response.TranscodedMessage.Should().ContainKey(ConversationChannel.Sms);
            response.TranscodedMessage![ConversationChannel.Sms].Should().Be("Hi");
        }
    }
}
