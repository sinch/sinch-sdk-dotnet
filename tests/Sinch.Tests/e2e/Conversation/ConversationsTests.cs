using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Conversations;
using Sinch.Conversation.Conversations.Create;
using Sinch.Conversation.Conversations.InjectEvent;
using Sinch.Conversation.Conversations.InjectMessage;
using Sinch.Conversation.Conversations.List;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class ConversationsTests : TestBase
    {
        private readonly Sinch.Conversation.Conversations.Conversation _conversation =
            new()
            {
                Id = "01HMXSGKNG4HCAW3XXTVK6Q8WD",
                AppId = "01HKWRC164GNT3K2GCPK5W2B1J",
                ContactId = "01HKWT3XRVH6RP17S8KSBC4PYR",
                ActiveChannel = ConversationChannel.Instagram,
                Active = true,
                Metadata = "meta",
                CorrelationId = "cor_id",
                MetadataJson = new JsonObject
                {
                    ["hi"] = "hi2",
                    ["hey"] = new JsonArray("a", "b")
                },
                LastReceived = DateTime.Parse("1970-01-01T00:00:00Z", CultureInfo.InvariantCulture).ToUniversalTime()
            };

        [Fact]
        public async Task Create()
        {
            var response = await SinchClientMockServer.Conversation.Conversations.Create(new CreateConversationRequest
            {
                AppId = "01HKWRC164GNT3K2GCPK5W2B1J",
                ActiveChannel = ConversationChannel.Instagram,
                Active = true,
                Metadata = "meta",
                ContactId = "01HKWT3XRVH6RP17S8KSBC4PYR",
                MetadataJson = new JsonObject
                {
                    ["hi"] = "hi2",
                    ["hey"] = new JsonArray("a", "b")
                }
            });

            response.Should().BeEquivalentTo(_conversation, options =>
                options.ExcludingNestedObjects().Excluding(x => x.MetadataJson));
            ValidateMetadata(response);
        }

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.Conversations.Get(_conversation.Id);

            response.Should().BeEquivalentTo(_conversation, options =>
                options.ExcludingNestedObjects().Excluding(x => x.MetadataJson));
            ValidateMetadata(response);
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockServer.Conversation.Conversations.List(new ListConversationsRequest
            {
                AppId = "01HKWRC164GNT3K2GCPK5W2B1J",
                ActiveChannel = ConversationChannel.KakaoTalkChat,
                PageSize = 10,
                PageToken = "ABC",
                ContactId = "01HKWT3XRVH6RP17S8KSBC4PYR",
                OnlyActive = true
            });
            response.Conversations.Should().HaveCount(2);
            response.TotalSize.Should().Be(2);
            response.NextPageToken.Should().BeEquivalentTo("abc");
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.Conversations.Delete(_conversation.Id);
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Update()
        {
            var response = await
                SinchClientMockServer.Conversation.Conversations.Update(_conversation,
                    MetadataUpdateStrategy.MergePatch);

            response.Should().BeEquivalentTo(_conversation, options =>
                options.ExcludingNestedObjects().Excluding(x => x.MetadataJson));
            ValidateMetadata(response);
        }

        [Fact]
        public async Task Inject()
        {
            var op = () => SinchClientMockServer.Conversation.Conversations.InjectMessage(new InjectMessageRequest
            {
                Direction = ConversationDirection.ToApp,
                AcceptTime = DateTime.Parse("1970-01-01T00:00:00Z", CultureInfo.InvariantCulture).ToUniversalTime(),
                AppMessage = new AppMessage(new TextMessage("hi")),
                ChannelIdentity = new ChannelIdentity
                {
                    Identity = "01HN31W37910AANG1JGE8Y6RFF",
                    Channel = ConversationChannel.Instagram
                },
                ContactId = _conversation.ContactId,
                ConversationId = _conversation.Id,
                Metadata = "meta",
                ContactMessage = new ContactMessage(new TextMessage("oi"))
            });
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Stop()
        {
            var op = () => SinchClientMockServer.Conversation.Conversations.Stop(_conversation.Id);
            await op.Should().NotThrowAsync();
        }


        [Fact]
        public async Task InjectEvent()
        {
            var response = await SinchClientMockServer.Conversation.Conversations.InjectEvent(
                new InjectEventRequest(new AppEvent(new ComposingEvent()))
                {
                    ConversationId = _conversation.Id,
                    AcceptTime = DateTime.Parse("1970-01-01T00:00:00Z", CultureInfo.InvariantCulture).ToUniversalTime(),
                    ChannelIdentity = new ChannelIdentity()
                    {
                        Identity = "01HN31W37910AANG1JGE8Y6RFF",
                        Channel = ConversationChannel.Instagram,
                        AppId = "123"
                    },
                    ContactId = _conversation.ContactId,
                    ProcessingMode = ProcessingMode.Dispatch,
                });
            response.Should().BeEquivalentTo(new InjectEventResponse()
            {
                AcceptedTime = DateTime.Parse("2019-08-24T14:15:22Z", CultureInfo.InvariantCulture).ToUniversalTime(),
                EventId = "123"
            });
        }

        private static void ValidateMetadata(Sinch.Conversation.Conversations.Conversation response)
        {
            response.MetadataJson["hi"]!.ToString().Should().BeEquivalentTo("hi2");
            response.MetadataJson["hey"]!.AsArray().Select(x => x.ToString()).ToList().Should().BeEquivalentTo(
                new List<string>
                {
                    "a", "b"
                });
        }
    }
}
