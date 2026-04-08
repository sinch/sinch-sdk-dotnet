using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Conversations.Create;
using Sinch.Conversation.Conversations.InjectEvent;
using Sinch.Conversation.Conversations.List;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Xunit;
using ConversationModel = Sinch.Conversation.Conversations.Conversation;

namespace Sinch.Tests.Conversation
{
    public class ConversationsTests : ConversationTestBase
    {
        private const string ConversationId = "01W4FFL35P4NC4K35CONVERS001";
        private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";
        private const string ContactId = "01W4FFL35P4NC4K35CONTACT001";

        private readonly string _baseConversationsUrl =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/conversations";

        #region Create Tests

        [Fact]
        public async Task Create_WithValidRequest_ReturnsConversation()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, _baseConversationsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent(AppId)
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ConversationModel
                    {
                        Id = ConversationId,
                        AppId = AppId,
                        ContactId = ContactId,
                        Active = true
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.Create(new CreateConversationRequest
            {
                AppId = AppId,
                ContactId = ContactId,
                Active = true,
                ActiveChannel = ConversationChannel.Messenger
            });

            response.Should().NotBeNull();
            response.Id.Should().Be(ConversationId);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidId_ReturnsConversation()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseConversationsUrl}/{ConversationId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ConversationModel
                    {
                        Id = ConversationId,
                        AppId = AppId,
                        Active = true
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.Get(ConversationId);

            response.Should().NotBeNull();
            response.Id.Should().Be(ConversationId);
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithPageSize_ReturnsListConversationsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseConversationsUrl}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListConversationsResponse
                    {
                        Conversations = new List<ConversationModel>
                        {
                            new() { Id = ConversationId },
                            new() { Id = "01W4FFL35P4NC4K35CONVERS002" }
                        },
                        NextPageToken = "next_token",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.List();

            response.Should().NotBeNull();
            response.Conversations.Should().HaveCount(2);
            response.NextPageToken.Should().Be("next_token");
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllConversations()
        {
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseConversationsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListConversationsResponse
                    {
                        Conversations = new List<ConversationModel>
                        {
                            new() { Id = ConversationId },
                            new() { Id = "01W4FFL35P4NC4K35CONVERS002" }
                        },
                        NextPageToken = "next_token",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseConversationsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListConversationsResponse
                    {
                        Conversations = new List<ConversationModel>
                        {
                            new() { Id = "01W4FFL35P4NC4K35CONVERS003" }
                        },
                        NextPageToken = "",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var results = new List<ConversationModel>();
            await foreach (var conv in Conversation.Conversations.ListAuto())
                results.Add(conv);

            results.Should().HaveCount(3);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region ListRecent Tests

        [Fact]
        public async Task ListRecent_WithPageSize_ReturnsListRecentConversationsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseConversationsUrl}:recent*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListRecentConversationsResponse
                    {
                        Conversations =
                        [
                            new()
                            {
                                Conversation = new ConversationModel
                                    { Id = ConversationId }
                            },

                            new()
                            {
                                Conversation = new ConversationModel
                                    { Id = "01W4FFL35P4NC4K35CONVERS002" }
                            }
                        ],
                        NextPageToken = "next_token",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.ListRecent(new ListRecentConversationsRequest
            {
                AppId = AppId,
                PageSize = 2
            });

            response.Should().NotBeNull();
            response.Conversations.Should().HaveCount(2);
            response.NextPageToken.Should().Be("next_token");
        }

        [Fact]
        public async Task ListRecentAuto_WithMultiplePages_IteratesThroughAllRecentConversations()
        {
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, $"{_baseConversationsUrl}:recent")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListRecentConversationsResponse
                    {
                        Conversations =
                        [
                            new()
                            {
                                Conversation = new ConversationModel
                                    { Id = ConversationId }
                            },

                            new()
                            {
                                Conversation = new ConversationModel
                                    { Id = "01W4FFL35P4NC4K35CONVERS002" }
                            }
                        ],
                        NextPageToken = "next_token",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, $"{_baseConversationsUrl}:recent")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ListRecentConversationsResponse
                    {
                        Conversations = new List<ConversationRecentMessage>
                        {
                            new()
                            {
                                Conversation = new ConversationModel
                                    { Id = "01W4FFL35P4NC4K35CONVERS003" }
                            }
                        },
                        NextPageToken = "",
                        TotalSize = 3
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var results = new List<ConversationRecentMessage>();
            await foreach (var item in Conversation.Conversations.ListRecentAuto(
                new ListRecentConversationsRequest
                {
                    AppId = AppId,
                    PageSize = 2
                }))
                results.Add(item);

            results.Should().HaveCount(3);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidRequest_ReturnsUpdatedConversation()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"{_baseConversationsUrl}/{ConversationId}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ConversationModel
                    {
                        Id = ConversationId,
                        AppId = "01W4FFL35P4NC4K35CONVAPP002",
                        Active = false,
                        CorrelationId = "my-correlator"
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.Update(
                new ConversationModel
                {
                    Id = ConversationId,
                    Active = false,
                    AppId = "01W4FFL35P4NC4K35CONVAPP002",
                    CorrelationId = "my-correlator"
                });

            response.Should().NotBeNull();
            response.Id.Should().Be(ConversationId);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidId_DeletesSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{_baseConversationsUrl}/{ConversationId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ConversationModel(),
                        options: SinchConversationClient.JsonSerializerOptionsInner));

            await Conversation.Conversations.Delete(ConversationId);
        }

        #endregion

        #region Stop Tests

        [Fact]
        public async Task Stop_WithValidId_StopsSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_baseConversationsUrl}/{ConversationId}:stop")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new ConversationModel(),
                        options: SinchConversationClient.JsonSerializerOptionsInner));

            await Conversation.Conversations.Stop(ConversationId);
        }

        #endregion

        #region InjectEvent Tests

        [Fact]
        public async Task InjectEvent_WithValidRequest_ReturnsInjectEventResponse()
        {
            var acceptTime = new DateTime(2024, 6, 6, 15, 15, 15, DateTimeKind.Utc);

            var injectEventRequest = new InjectEventRequest(
                new AppEvent(new ComposingEvent()))
            {
                ConversationId = ConversationId,
                AcceptTime = acceptTime
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_baseConversationsUrl}/{ConversationId}:inject-event")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(injectEventRequest,
                    SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK,
                    JsonContent.Create(new InjectEventResponse
                    {
                        EventId = "01W4FFL35P4NC4K35CONVEVENT1",
                        AcceptedTime = Helpers.ParseUtc("2025-06-06T15:15:15.000Z")
                    }, options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Conversations.InjectEvent(injectEventRequest);

            response.Should().NotBeNull();
            response.EventId.Should().Be("01W4FFL35P4NC4K35CONVEVENT1");
        }

        #endregion

        #region Deserialization Tests

        [Fact]
        public void DeserializeListRecentConversationsResponse()
        {
            var json = Helpers.LoadResources("Conversation/Conversations/ListRecentConversationsResponse.json");

            var result = JsonSerializer.Deserialize<ListRecentConversationsResponse>(json,
                Conversation.JsonSerializerOptions);

            result.Should().NotBeNull();
            result!.Conversations.Should().HaveCount(2);
            result.NextPageToken.Should().Be("next_token_abc");
            result.TotalSize.Should().Be(5);

            var first = result.Conversations![0];
            first.Conversation.Should().NotBeNull();
            first.Conversation!.Id.Should().Be("01W4FFL35P4NC4K35CONVERS001");
            first.Conversation.AppId.Should().Be("01W4FFL35P4NC4K35CONVAPP001");
            first.Conversation.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
            first.Conversation.Active.Should().BeTrue();
            first.Conversation.ActiveChannel.Should().Be(ConversationChannel.Messenger);
            first.Conversation.Metadata.Should().Be("e2e tests");
            first.Conversation.CorrelationId.Should().Be("my-correlator");

            first.LastMessage.Should().NotBeNull();
            first.LastMessage!.Direction.Should().Be(ConversationDirection.ToContact);
            first.LastMessage.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
            first.LastMessage.AppMessage.Should().NotBeNull();
            first.LastMessage.AppMessage!.TextMessage.Should().NotBeNull();
            first.LastMessage.AppMessage.TextMessage!.Text.Should().Be("Hello from recent");

            var second = result.Conversations[1];
            second.Conversation.Should().NotBeNull();
            second.Conversation!.Id.Should().Be("01W4FFL35P4NC4K35CONVERS002");
            second.Conversation.Active.Should().BeFalse();
            second.LastMessage.Should().NotBeNull();
            second.LastMessage!.Direction.Should().Be(ConversationDirection.ToApp);
        }

        #endregion

        #region Model Tests

        [Fact]
        public void UpdateMaskConversation()
        {
            var conversation = new ConversationModel
            {
                ActiveChannel = null,
                Active = true,
                AppId = "null",
                ContactId = "id",
                Id = "1",
                Metadata = "n",
                MetadataJson = new JsonObject(),
                CorrelationId = string.Empty
            };

            conversation.GetPropertiesMask().Should().BeEquivalentTo(
                "active_channel,active,app_id,contact_id,metadata,metadata_json,correlation_id");
        }

        [Fact]
        public void UpdateMaskConversationOnlyOneField()
        {
            var conversation = new ConversationModel
            {
                AppId = "AppId",
            };

            conversation.GetPropertiesMask().Should().BeEquivalentTo(
                "app_id");
        }

        [Fact]
        public void Validate_ThrowsWhenRegionNotSet()
        {
            var client = new SinchClient(new SinchClientConfiguration());
            var act = () => client.Conversation;
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Region*required*");
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenRegionIsSet()
        {
            var client = new SinchClient(new SinchClientConfiguration
            {
                ConversationConfiguration = new SinchConversationConfiguration { Region = ConversationRegion.Us }
            });
            var act = () => client.Conversation;
            act.Should().NotThrow<InvalidOperationException>();
        }

        #endregion
    }
}
