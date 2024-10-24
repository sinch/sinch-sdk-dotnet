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

        [Fact]
        public async Task MessageInboundSmartConversationRedaction()
        {
            var json = await _httpClient.GetStringAsync("message-inbound/smart-conversation-redaction");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageInboundSmartConversationRedactionEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    EventTime = DateTime.Parse("2024-06-06T14:42:41.293Z").ToUniversalTime(),
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.240093543Z").ToUniversalTime(),
                    CorrelationId = "correlatorId",
                    MessageRedaction = new MessageInboundEventItem()
                    {
                        Id = "01W4FFL35P4NC4K35MESSAGE02",
                        Direction = ConversationDirection.ToApp,
                        ContactMessage = new ContactMessage(new TextMessage(
                                "Hi, my real name is {PERSON} and I live in {LOCATION}. My credit card number is 4242 4242 4242 4242. What a beautiful day!")),
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",
                        ChannelIdentity = new ChannelIdentity()
                        {
                            Channel = ConversationChannel.Messenger,
                            Identity = "7968425018576406",
                            AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        },
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Metadata = "",
                        ProcessingMode = ProcessingMode.Conversation,
                        Injected = false,
                        SenderId = "",
                        AcceptTime = DateTime.Parse("2024-06-06T14:42:42.165Z").ToUniversalTime(),
                    }
                },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<MessageInboundSmartConversationRedactionEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task MessageSubmitMedia()
        {
            var json = await _httpClient.GetStringAsync("message-submit/media");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageSubmitEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    EventTime = DateTime.Parse("2024-06-06T14:42:42.475Z").ToUniversalTime(),
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.475Z").ToUniversalTime(),
                    CorrelationId = "",
                    MessageSubmitNotification = new MessageSubmitNotification()
                    {
                        MessageId = "01W4FFL35P4NC4K35MESSAGE04",
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",

                        ChannelIdentity = new ChannelIdentity()
                        {
                            Channel = ConversationChannel.Messenger,
                            Identity = "7968425018576406",
                            AppId = "01W4FFL35P4NC4K35CONVAPP01",
                        },
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Metadata = "",
                        ProcessingMode = ProcessingMode.Conversation,
                        SubmittedMessage = new ContactMessage(new MediaMessage()
                        {
                            FilenameOverride = "",
                            ThumbnailUrl = "",
                            Url =
                                    "https://scontent.xx.fbcdn.net/v/t1.15752-9/450470563_473474858617216_4192328888545460366_n.png?_nc_cat=102&ccb=1-7&_nc_sid=fc17b8&_nc_ohc=48P1Kdk4UiwQ7kNvgE60fDt&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&oh=03_Q7cD1QEkgERuI-tu8rt1GGpOEcNU2-0bFkmG4mQkzbciZss10g&oe=66C0A0E0",
                        })
                    }
                },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<MessageSubmitEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task MessageSubmitText()
        {
            var json = await _httpClient.GetStringAsync("message-submit/text");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new MessageSubmitEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    EventTime = DateTime.Parse("2024-06-06T14:42:42.721Z").ToUniversalTime(),
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.721Z").ToUniversalTime(),
                    CorrelationId = "correlatorId",
                    MessageSubmitNotification = new MessageSubmitNotification()
                    {
                        MessageId = "01W4FFL35P4NC4K35MESSAGE03",
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",

                        ChannelIdentity = new ChannelIdentity()
                        {
                            Channel = ConversationChannel.Rcs,
                            Identity = "12015555555",
                            AppId = "",
                        },
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Metadata = "",
                        ProcessingMode = ProcessingMode.Conversation,
                        SubmittedMessage = new ContactMessage(new TextMessage("I \u2764\ufe0f Sinch")
                        {
                        })
                    }
                },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<MessageSubmitEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task SmartConversationMedia()
        {
            var json = await _httpClient.GetStringAsync("smart-conversations/media");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new SmartConversationsEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    EventTime = DateTime.Parse("2024-06-06T14:42:42.094Z").ToUniversalTime(),
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:44.2069826Z").ToUniversalTime(),
                    CorrelationId = "",
                    SmartConversationNotification = new SmartConversationNotification()
                    {
                        MessageId = "01W4FFL35P4NC4K35MESSAGE04",
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",

                        ChannelIdentity = "7968425018576406",
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Channel = ConversationChannel.Messenger,
                        AnalysisResults = new AnalysisResult()
                        {
                            MlImageRecognitionResult = new List<MachineLearningImageRecognitionResult>()
                                {
                                    new MachineLearningImageRecognitionResult()
                                    {
                                        Url =
                                            "https://scontent.xx.fbcdn.net/v/t1.15752-9/450470563_473474858617216_4192328888545460366_n.png?_nc_cat=102&ccb=1-7&_nc_sid=fc17b8&_nc_ohc=48P1Kdk4UiwQ7kNvgE60fDt&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&oh=03_Q7cD1QEkgERuI-tu8rt1GGpOEcNU2-0bFkmG4mQkzbciZss10g&oe=66C0A0E0"
                                    }
                                },
                            MlOffensiveAnalysisResult = new List<OffensiveAnalysis>()
                                {
                                    new OffensiveAnalysis()
                                    {
                                        Message = "",
                                        Url =
                                            "https://scontent.xx.fbcdn.net/v/t1.15752-9/450470563_473474858617216_4192328888545460366_n.png?_nc_cat=102&ccb=1-7&_nc_sid=fc17b8&_nc_ohc=48P1Kdk4UiwQ7kNvgE60fDt&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&oh=03_Q7cD1QEkgERuI-tu8rt1GGpOEcNU2-0bFkmG4mQkzbciZss10g&oe=66C0A0E0",
                                        Evaluation = Evaluation.Safe,
                                        Score = 0.3069f
                                    }
                                }
                        }
                    },
                },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<SmartConversationsEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task SmartConversationText()
        {
            var json = await _httpClient.GetStringAsync("smart-conversations/text");

            var result = _sinchConversationWebhooks.DeserializeCallbackEvent(json);

            AssertEvent(result);

            var resultPlain = JsonSerializer.Deserialize<ICallbackEvent>(json);

            AssertEvent(resultPlain);

            void AssertEvent(ICallbackEvent callbackEvent)
            {
                callbackEvent.Should().BeEquivalentTo(new SmartConversationsEvent()
                {
                    AppId = "01W4FFL35P4NC4K35CONVAPP01",
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    EventTime = DateTime.Parse("2024-06-06T14:42:42.1492634Z").ToUniversalTime(),
                    AcceptedTime = DateTime.Parse("2024-06-06T14:42:42.2198899Z").ToUniversalTime(),
                    CorrelationId = "",
                    SmartConversationNotification = new SmartConversationNotification()
                    {
                        MessageId = "01W4FFL35P4NC4K35MESSAGE03",
                        ConversationId = "01W4FFL35P4NC4K35CONVERS01",

                        ChannelIdentity = "12015555555",
                        ContactId = "01W4FFL35P4NC4K35CONTACT01",
                        Channel = ConversationChannel.Rcs,
                        AnalysisResults = new AnalysisResult()
                        {
                            MlSentimentResult = new List<MachineLearningSentimentResult>()
                                {
                                    new MachineLearningSentimentResult()
                                    {
                                        Message = "I \u2764\ufe0f Sinch",
                                        Sentiment = Sentiment.Positive,
                                        Score = 0.9041176f,
                                        Results = new List<SentimentResult>()
                                        {
                                            new SentimentResult()
                                            {
                                                Sentiment = Sentiment.Negative,
                                                Score = 0.0028852955f
                                            },
                                            new SentimentResult()
                                            {
                                                Sentiment = Sentiment.Neutral,
                                                Score = 0.09299716f
                                            },

                                            new SentimentResult()
                                            {
                                                Sentiment = Sentiment.Positive,
                                                Score = 0.9041176f
                                            },
                                        }
                                    }
                                },
                            MlNluResult = new List<MachineLearningNLUResult>()
                                {
                                    new MachineLearningNLUResult()
                                    {
                                        Message = "I \u2764\ufe0f Sinch",
                                        Intent = "chitchat.thank_you",
                                        Score = 0.99831617f,
                                        Results = new List<IntentResult>()
                                        {
                                            new IntentResult()
                                            {
                                                Intent = "chitchat.thank_you",
                                                Score = 0.99831617f
                                            },
                                            new IntentResult()
                                            {
                                                Intent = "chitchat.one_moment_please",
                                                Score =  0.00027679664f
                                            },
                                            new IntentResult()
                                            {
                                                Intent = "chitchat.bye",
                                                Score =  0.0002178006f
                                            }
                                        }
                                    },

                                },
                            MlPiiResult = new List<MachineLearningPIIResult>()
                                {
                                    new MachineLearningPIIResult()
                                    {
                                        Message = "I \u2764\ufe0f Sinch",
                                        Masked = "{PERSON} {PERSON} {PERSON}"
                                    }
                                },
                            MlOffensiveAnalysisResult = new List<OffensiveAnalysis>()
                                {
                                    new OffensiveAnalysis()
                                    {
                                        Message = "I \u2764\ufe0f Sinch",
                                        Url = "",
                                        Evaluation = Evaluation.Safe,
                                        Score = 0.9826318f
                                    }
                                }
                        }
                    },
                },
                    x => x.Excluding(m =>
                        m.MessageMetadata));
                var convEvent = callbackEvent.As<SmartConversationsEvent>();
                convEvent.MessageMetadata!.GetValue<string>().Should().BeEmpty();
            }
        }
    }
}
