using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Hooks;
using Sinch.Conversation.Hooks.Models;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Webhooks;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    // utilizes mocks initialized from sinch-mock-server json files
    // the models are from a custom endpoint, meaning no sinch client endpoint matches them
    // json is fetched with http requests, testing parsing here
    public class HooksTests : TestBase
    {
        private readonly HttpClient _httpClient;
        private readonly ISinchConversationWebhooks _sinchConversationWebhooks;

        public HooksTests()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(WebhooksEventsBaseAddress)
            };
            _sinchConversationWebhooks = SinchClientMockServer.Conversation.Webhooks;
        }

        [Fact]
        public async Task MessageInbound()
        {
            var json = await _httpClient.GetStringAsync("message-inbound");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageInboundEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.151Z").ToUniversalTime(),
                    EventTime = DateTime.Parse("2024-06-06T14:42:42.1492634Z").ToUniversalTime(),
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    CorrelationId = "correlatorId",
                    Message = new MessageInboundEventItem()
                    {
                        Id = "01W4FFL35P4NC4K35MESSAGE01",
                        Direction = ConversationDirection.ToApp,
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Metadata = "",
                        AcceptTime = DateTime.Parse("2024-06-06T14:42:42.151Z").ToUniversalTime(),
                        SenderId = "",
                        ProcessingMode = ProcessingMode.Conversation,
                        Injected = false,
                        ChannelIdentity = new ChannelIdentity()
                        {
                            Channel = ConversationChannel.Rcs,
                            Identity = "12015555555",
                            AppId = ""
                        },
                        ContactMessage = new ContactMessage(new TextMessage("Hello")),
                    },
                }, x => x.Excluding(m => m.MessageMetadata));
                callbackEvent.As<MessageInboundEvent>().MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }
        
        [Fact]
        public async Task ContactDelete()
        {
            var json = await _httpClient.GetStringAsync("contact-delete");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ContactDeleteEvent()
                {
                    AppId = "",
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.726873733Z").ToUniversalTime(),
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    CorrelationId = "",
                    ContactDeleteNotification = new ContactNotification()
                    {
                        Contact = new Contact()
                        {
                            Id = "01W4FFL35P4NC4K35CONTACT01",
                            ChannelIdentities = new List<ChannelIdentity>()
                            {
                                new ChannelIdentity()
                                {
                                    Channel = ConversationChannel.Sms,
                                    Identity = "12015555555",
                                    AppId = ""
                                }
                            },
                            DisplayName = "Source Contact",
                            Email = "source@mail.com",
                            ExternalId = "",
                            Metadata = "Some metadata belonging to the source contact",
                            Language = ConversationLanguage.French,
                        }
                    }
                }, x => x.Excluding(m => m.MessageMetadata));
                callbackEvent.As<ContactDeleteEvent>().MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }
    }
}
