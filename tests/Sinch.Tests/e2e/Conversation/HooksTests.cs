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
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Hooks;
using Sinch.Conversation.Hooks.Models;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Webhooks;
using Sinch.Numbers.Hooks;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    // utilizes mocks initialized from sinch-mock-server json files
    // the models are from a custom endpoint, meaning no sinch client endpoint matches them
    // json is fetched with http requests, testing parsing here
    // for two types: plain deser with JsonSerializer, and provided Deserialize from ISinchConversationWebhooks
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

        [Fact]
        public async Task ContactUpdate()
        {
            var json = await _httpClient.GetStringAsync("contact-update");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ContactUpdateEvent()
                {
                    AppId = "",
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.1646148Z").ToUniversalTime(),
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    CorrelationId = "",
                    ContactUpdateNotification = new ContactNotification()
                    {
                        Contact = new Contact()
                        {
                            Id = "01W4FFL35P4NC4K35CONTACT01",
                            ChannelIdentities = new List<ChannelIdentity>()
                            {
                                new ChannelIdentity()
                                {
                                    Channel = ConversationChannel.Rcs,
                                    Identity = "12015556666",
                                    AppId = ""
                                }
                            },
                            ChannelPriority = new List<ConversationChannel>()
                            {
                                ConversationChannel.Rcs
                            },
                            DisplayName = "Updated name with the Sinch SDK",
                            Email = "",
                            ExternalId = "",
                            Metadata = "",
                            Language = ConversationLanguage.French,
                        }
                    }
                }, x => x.Excluding(m => m.MessageMetadata));
                callbackEvent.As<ContactUpdateEvent>().MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task ContactMerge()
        {
            var json = await _httpClient.GetStringAsync("contact-merge");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ContactMergeEvent()
                {
                    AppId = "",
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.722089838Z").ToUniversalTime(),
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    CorrelationId = "",
                    ContactMergeNotification = new ContactMergeNotification()
                    {
                        PreservedContact = new Contact()
                        {
                            Id = "01W4FFL35P4NC4K35CONTACT02",
                            ChannelIdentities = new List<ChannelIdentity>()
                            {
                                new ChannelIdentity()
                                {
                                    Channel = ConversationChannel.Mms,
                                    Identity = "12016666666",
                                    AppId = ""
                                }
                            },
                            ChannelPriority = new List<ConversationChannel>()
                            {
                                ConversationChannel.Mms
                            },
                            DisplayName = "Destination Contact",
                            Email = "",
                            ExternalId = "",
                            Metadata = "",
                            Language = ConversationLanguage.EnglishUS,
                        },
                        DeletedContact = new Contact()
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
                callbackEvent.As<ContactMergeEvent>().MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task ConversationDelete()
        {
            var json = await _httpClient.GetStringAsync("conversation-delete");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ConversationDeleteEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        CorrelationId = "",
                        ConversationDeleteNotification = new ConversationNotification()
                        {
                            Conversation = new Sinch.Conversation.Conversations.Conversation()
                            {
                                Id = "01W4FFL35P4NC4K35CONVERS01",
                                AppId = "01W4FFL35P4NC4K35CONVAPP01",
                                ContactId = "01W4FFL35P4NC4K35CONTACT01",
                                LastReceived = DateTime.Parse("2024-06-06T14:42:42Z").ToUniversalTime(),
                                ActiveChannel = ConversationChannel.Rcs,
                                Active = false,
                                Metadata = "",
                                CorrelationId = "correlatorId"
                            }
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata).Excluding(m => m.ConversationDeleteNotification.Conversation.MetadataJson));
                var deleteEvent = callbackEvent.As<ConversationDeleteEvent>();
                deleteEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
                deleteEvent.ConversationDeleteNotification!.Conversation!.MetadataJson!.ToJsonString().Should()
                    .Be("{}");
            }
        }

        [Fact]
        public async Task ConversationStart()
        {
            var json = await _httpClient.GetStringAsync("conversation-start");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ConversationStartEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        CorrelationId = "",
                        ConversationStartNotification = new ConversationNotification()
                        {
                            Conversation = new Sinch.Conversation.Conversations.Conversation()
                            {
                                Id = "01W4FFL35P4NC4K35CONVERS01",
                                AppId = "01W4FFL35P4NC4K35CONVAPP01",
                                ContactId = "01W4FFL35P4NC4K35CONTACT01",
                                LastReceived = new DateTime(),
                                ActiveChannel = ConversationChannel.Unspecified,
                                Active = true,
                                Metadata = "",
                                CorrelationId = "correlatorId",
                            }
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata).Excluding(m => m.ConversationStartNotification.Conversation.MetadataJson));
                var convEvent = callbackEvent.As<ConversationStartEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
                convEvent.ConversationStartNotification!.Conversation!.MetadataJson!.ToJsonString().Should()
                    .Be("{}");
            }
        }

        [Fact]
        public async Task ConversationStop()
        {
            var json = await _httpClient.GetStringAsync("conversation-stop");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new ConversationStopEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        CorrelationId = "",
                        ConversationStopNotification = new ConversationNotification()
                        {
                            Conversation = new Sinch.Conversation.Conversations.Conversation()
                            {
                                Id = "01W4FFL35P4NC4K35CONVERS01",
                                AppId = "01W4FFL35P4NC4K35CONVAPP01",
                                ContactId = "01W4FFL35P4NC4K35CONTACT01",
                                LastReceived = DateTime.Parse("2024-06-06T14:42:42Z").ToUniversalTime(),
                                ActiveChannel = ConversationChannel.Rcs,
                                Active = false,
                                Metadata = "",
                                CorrelationId = "correlatorId",
                            }
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata).Excluding(m => m.ConversationStopNotification.Conversation.MetadataJson));
                var convEvent = callbackEvent.As<ConversationStopEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
                convEvent.ConversationStopNotification!.Conversation!.MetadataJson!.ToJsonString().Should()
                    .Be("{}");
            }
        }

        [Fact]
        public async Task EventDeliveryReportFailed()
        {
            var json = await _httpClient.GetStringAsync("event-delivery-report/failed");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new DeliveryEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        CorrelationId = "",
                        AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.208Z").ToUniversalTime(),
                        EventTime = DateTime.Parse("2024-06-06T14:42:42.251277147Z").ToUniversalTime(),
                        EventDeliveryReport = new EventDeliveryAllOfEventDeliveryReport()
                        {
                            EventId = "01W4FFL35P4NC4K35EVENT0003",
                            Status = DeliveryStatus.Failed,
                            ChannelIdentity = new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Messenger,
                                Identity = "7968425018576406",
                                AppId = "01W4FFL35P4NC4K35CONVAPP01"
                            },
                            ContactId = "",
                            Reason = new Reason()
                            {
                                Code = "BAD_REQUEST",
                                Description =
                                    "The underlying channel reported: Message type [MESSAGE_NOT_SET] not supported on Messenger",
                                SubCode = "UNSPECIFIED_SUB_CODE"
                            },
                            Metadata = "",
                            ProcessingMode = ProcessingMode.Conversation
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<DeliveryEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task EventDeliveryReportSucceeded()
        {
            var json = await _httpClient.GetStringAsync("event-delivery-report/succeeded");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new DeliveryEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        CorrelationId = "",
                        AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.132Z").ToUniversalTime(),
                        EventTime = DateTime.Parse("2024-06-06T14:42:42.891Z").ToUniversalTime(),
                        EventDeliveryReport = new EventDeliveryAllOfEventDeliveryReport()
                        {
                            EventId = "01W4FFL35P4NC4K35EVENT0002",
                            Status = DeliveryStatus.Delivered,
                            ChannelIdentity = new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Messenger,
                                Identity = "7968425018576406",
                                AppId = "01W4FFL35P4NC4K35CONVAPP01"
                            },
                            ContactId = "",
                            Metadata = "",
                            ProcessingMode = ProcessingMode.Conversation
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<DeliveryEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task EventInbound()
        {
            var json = await _httpClient.GetStringAsync("event-inbound");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new InboundEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        EventTime = DateTime.Parse("2024-06-06T14:42:42.379863404Z").ToUniversalTime(),
                        CorrelationId = "",
                        Event = new EventInboundAllOfEvent()
                        {
                            Direction = ConversationDirection.ToApp,
                            ContactEvent = new ContactEvent()
                            {
                                ComposingEvent = new object()
                            },
                            Id = "01W4FFL35P4NC4K35EVENT0001",
                            ConversationId = "01W4FFL35P4NC4K35CONVERS01",
                            ContactId = "01W4FFL35P4NC4K35CONTACT01",
                            ChannelIdentity = new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Rcs,
                                Identity = "12015555555",
                                AppId = ""
                            },
                            AcceptTime = DateTime.Parse("2024-06-06T14:42:42.429455346Z").ToUniversalTime(),
                            ProcessingMode = ProcessingMode.Conversation,
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<InboundEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task MessageDeliveryReportFailed()
        {
            var json = await _httpClient.GetStringAsync("message-delivery-report/failed");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageDeliveryReceiptEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        EventTime = DateTime.Parse("2024-06-06T14:42:43Z").ToUniversalTime(),
                        AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.721Z").ToUniversalTime(),
                        CorrelationId = "correlatorId",
                        MessageDeliveryReport = new MessageDeliveryReport()
                        {
                            MessageId = "01W4FFL35P4NC4K35MESSAGE05",
                            ConversationId = "01W4FFL35P4NC4K35CONVERS01",
                            Status = DeliveryStatus.Failed,
                            ChannelIdentity = new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Rcs,
                                Identity = "12016666666",
                                AppId = "",
                            },
                            ContactId = "01W4FFL35P4NC4K35CONTACT02",
                            Reason = new Reason()
                            {
                                Code = "RECIPIENT_NOT_REACHABLE",
                                Description =
                                    "The underlying channel reported: Unable to find rcs support for the given recipient",
                                SubCode = "UNSPECIFIED_SUB_CODE",
                            },
                            Metadata = "",
                            ProcessingMode = ProcessingMode.Conversation
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<MessageDeliveryReceiptEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }
        
         [Fact]
        public async Task MessageDeliveryReportQueued()
        {
            var json = await _httpClient.GetStringAsync("message-delivery-report/succeeded");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageDeliveryReceiptEvent()
                    {
                        AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        EventTime = DateTime.Parse("2024-06-06T14:42:43.0093518Z").ToUniversalTime(),
                        AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.721Z").ToUniversalTime(),
                        CorrelationId = "correlatorId",
                        MessageDeliveryReport = new MessageDeliveryReport()
                        {
                            MessageId = "01W4FFL35P4NC4K35MESSAGE01",
                            ConversationId = "01W4FFL35P4NC4K35CONVERS01",
                            Status = DeliveryStatus.QueuedOnChannel,
                            ChannelIdentity = new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Rcs,
                                Identity = "12015555555",
                                AppId = "",
                            },
                            ContactId = "01W4FFL35P4NC4K35CONTACT01",
                            Metadata = "",
                            ProcessingMode = ProcessingMode.Conversation
                        }
                    },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<MessageDeliveryReceiptEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }
    }
}
