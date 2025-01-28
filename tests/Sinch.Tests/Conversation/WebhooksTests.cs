using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Hooks;
using Sinch.Conversation.Hooks.Models;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class WebhooksTests : ConversationTestBase
    {
        [Fact]
        public void ValidateRequest()
        {
            const string str =
                "{\"app_id\":\"\",\"accepted_time\":\"2021-10-18T17:49:13.813615Z\",\"project_id\":\"e2df3a34-a71b-4448-9db5-a8d2baad28e4\",\"contact_create_notification\":{\"contact\":{\"id\":\"01FJA8B466Y0R2GNXD78MD9SM1\",\"channel_identities\":[{\"channel\":\"SMS\",\"identity\":\"48123456789\",\"app_id\":\"\"}],\"display_name\":\"New Test Contact\",\"email\":\"new.contact@email.com\",\"external_id\":\"\",\"metadata\":\"\",\"language\":\"EN_US\"}},\"message_metadata\":\"\"}";

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, StringValues>()
            {
                { "x-sinch-webhook-signature-nonce", new[] { "01FJA8B4A7BM43YGWSG9GBV067" } },
                { "x-sinch-webhook-signature-timestamp", new[] { "1634579353" } },
                { "x-sinch-webhook-signature", new[] { "6bpJoRmFoXVjfJIVglMoJzYXxnoxRujzR4k2GOXewOE=" } },
            }, str, "foo_secret1234");
            isValid.Should().BeTrue();
        }

        [Fact]
        public void DeserializeCapabilityEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/CapabilityEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<CapabilityEvent>();

            result.Should().BeEquivalentTo(new CapabilityEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",
                CapabilityNotification = new CapabilityNotification()
                {
                    ContactId = "contact id value",
                    Identity = "12345678910",
                    Channel = ConversationChannel.WhatsApp,
                    CapabilityStatus = CapabilityStatus.CapabilityPartial,
                    RequestId = "request id value",
                    ChannelCapabilities = new List<string>() { "capability value" },
                    Reason = new Reason()
                    {
                        Code = "RECIPIENT_NOT_OPTED_IN",
                        Description = "reason description",
                        SubCode = "UNSPECIFIED_SUB_CODE"
                    }
                }
            });
        }

        [Fact]
        public void DeserializeChannelEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/ChannelEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ChannelEvent>();

            result.ChannelEventNotification?.ChannelEvent?.AdditionalData?["quality_rating"]?.GetValue<string>()
                .Should()
                .BeEquivalentTo("quality rating value");
            result.Should().BeEquivalentTo(new ChannelEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",
                ChannelEventNotification = new ChannelEventNotification()
                {
                    ChannelEvent = new EventNotification()
                    {
                        Channel = ConversationChannel.WhatsApp,
                        EventType = "WHATS_APP_QUALITY_RATING_CHANGED",
                        AdditionalData = new JsonObject()
                        {
                            ["quality_rating"] = "quality rating value"
                        }
                    }
                }
            },
                options => options.Excluding(x =>
                    x.MessageMetadata).Excluding(x => x.ChannelEventNotification.ChannelEvent.AdditionalData));
        }

        [Fact]
        public void DeserializeContactCreateEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/ContactCreateEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ContactCreateEvent>();

            result.Should().BeEquivalentTo(new ContactCreateEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ContactCreateNotification = new ContactNotification()
                {
                    Contact = new Contact()
                    {
                        Id = "my created contact ID",
                        ChannelIdentities = new List<ChannelIdentity>()
                        {
                            new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Mms,
                                Identity = "33987654321",
                            },
                            new ChannelIdentity()
                            {
                                AppId = "my MESSENGER app id",
                                Channel = ConversationChannel.Messenger,
                                Identity = "+33987654321"
                            }
                        },
                        ChannelPriority = new List<ConversationChannel>()
                        {
                            ConversationChannel.Mms,
                            ConversationChannel.Messenger
                        },
                        DisplayName = "created from Dotnet SDK",
                        Email = "foo@foo.com",
                        ExternalId = "external id value",
                        Metadata = "metadata value",
                        Language = ConversationLanguage.Arabic
                    }
                }
            });
        }

        [Fact]
        public void DeserializeContactDeleteEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/ContactDeleteEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ContactDeleteEvent>();

            result.Should().BeEquivalentTo(new ContactDeleteEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ContactDeleteNotification = new ContactNotification()
                {
                    Contact = new Contact()
                    {
                        Id = "my created contact ID",
                        ChannelIdentities = new List<ChannelIdentity>()
                        {
                            new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Mms,
                                Identity = "33987654321",
                            },
                            new ChannelIdentity()
                            {
                                AppId = "my MESSENGER app id",
                                Channel = ConversationChannel.Messenger,
                                Identity = "+33987654321"
                            }
                        },
                        ChannelPriority = new List<ConversationChannel>()
                        {
                            ConversationChannel.Mms,
                            ConversationChannel.Messenger
                        },
                        DisplayName = "created from Dotnet SDK",
                        Email = "foo@foo.com",
                        ExternalId = "external id value",
                        Metadata = "metadata value",
                        Language = ConversationLanguage.Arabic
                    }
                }
            });
        }


        [Fact]
        public void DeserializeContactIdentitiesDuplicationEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/ContactIdentitiesDuplicationEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ContactIdentitiesDuplicationEvent>();

            result.Should().BeEquivalentTo(new ContactIdentitiesDuplicationEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                DuplicatedContactIdentitiesNotification = new DuplicatedIdentitiesEvent()
                {
                    DuplicatedIdentities = new List<DuplicatedIdentitiesEventDuplicatedIdentitiesInner>()
                    {
                        new DuplicatedIdentitiesEventDuplicatedIdentitiesInner()
                        {
                            Channel = ConversationChannel.Messenger,
                            ContactIds = new List<string>()
                            {
                                "01EKA07N79THJ20ZSN6AS30TMW",
                                "01EKA07N79THJ20ZSN6AS30TTT"
                            }
                        }
                    }
                }
            });
        }

        [Fact]
        public void DeserializeContactMergeEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ContactMergeEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ContactMergeEvent>();

            result.Should().BeEquivalentTo(new ContactMergeEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ContactMergeNotification = new ContactMergeNotification()
                {
                    DeletedContact = new Contact()
                    {
                        Id = "my created contact ID",
                        ChannelIdentities = new List<ChannelIdentity>()
                        {
                            new ChannelIdentity() { Channel = ConversationChannel.Mms, Identity = "33987654321" },
                            new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Messenger, Identity = "+33987654321",
                                AppId = "my MESSENGER app id"
                            }
                        },
                        ChannelPriority = new List<ConversationChannel>()
                            { ConversationChannel.Mms, ConversationChannel.Messenger },
                        DisplayName = "created from Dotnet SDK",
                        Email = "foo@foo.com",
                        ExternalId = "external id value",
                        Metadata = "metadata value",
                        Language = ConversationLanguage.Arabic
                    },
                    PreservedContact = new Contact()
                    {
                        Id = "a contact id",
                        ChannelIdentities = new List<ChannelIdentity>()
                        {
                            new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Sms, Identity = "a channel identity", AppId = "an app id"
                            }
                        },
                        ChannelPriority = new List<ConversationChannel>() { ConversationChannel.Sms },
                        DisplayName = "a display name",
                        Email = "an email",
                        ExternalId = "an external id",
                        Metadata = "metadata value",
                        Language = new ConversationLanguage("UNSPECIFIED")
                    }
                }
            });
        }

        [Fact]
        public void DeserializeContactUpdateEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ContactUpdateEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ContactUpdateEvent>();

            result.Should().BeEquivalentTo(new ContactUpdateEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",
                ContactUpdateNotification = new ContactNotification()
                {
                    Contact = new Contact()
                    {
                        Id = "my created contact ID",
                        ChannelIdentities = new List<ChannelIdentity>()
                        {
                            new ChannelIdentity() { Channel = ConversationChannel.Mms, Identity = "33987654321" },
                            new ChannelIdentity()
                            {
                                Channel = ConversationChannel.Messenger, Identity = "+33987654321",
                                AppId = "my MESSENGER app id"
                            }
                        },
                        ChannelPriority = new List<ConversationChannel>()
                            { ConversationChannel.Mms, ConversationChannel.Messenger },
                        DisplayName = "created from Dotnet SDK",
                        Email = "foo@foo.com",
                        ExternalId = "external id value",
                        Metadata = "metadata value",
                        Language = ConversationLanguage.Arabic
                    }
                }
            });
        }

        [Fact]
        public void DeserializeConversationDeleteEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ConversationDeleteEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ConversationDeleteEvent>();

            result.ConversationDeleteNotification?.Conversation?.MetadataJson?["metadata_json_key"]?
                .GetValue<string>().Should().Be("metadata json value");
            result.Should().BeEquivalentTo(new ConversationDeleteEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ConversationDeleteNotification = new ConversationNotification()
                {
                    Conversation = new Sinch.Conversation.Conversations.Conversation()
                    {
                        Active = true,
                        ActiveChannel = ConversationChannel.WhatsApp,
                        AppId = "conversation app Id",
                        ContactId = "contact ID",
                        Id = "a conversation id",
                        LastReceived = Helpers.ParseUtc("2020-11-17T15:00:00Z"),
                        Metadata = "metadata value",
                        MetadataJson = new JsonObject()
                            {
                                { "metadata_json_key", "metadata json value" }
                            },
                        CorrelationId = "correlation id value"
                    }
                }
            },
                options => options.Excluding(x => x.MessageMetadata)
                    .Excluding(x => x.ConversationDeleteNotification.Conversation.MetadataJson));
        }

        [Fact]
        public void DeserializeConversationStartEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ConversationStartEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ConversationStartEvent>();

            result.ConversationStartNotification?.Conversation?.MetadataJson?["metadata_json_key"]?
                .GetValue<string>().Should().Be("metadata json value");
            result.Should().BeEquivalentTo(new ConversationStartEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ConversationStartNotification = new ConversationNotification()
                {
                    Conversation = new Sinch.Conversation.Conversations.Conversation()
                    {
                        Active = true,
                        ActiveChannel = ConversationChannel.WhatsApp,
                        AppId = "conversation app Id",
                        ContactId = "contact ID",
                        Id = "a conversation id",
                        LastReceived = Helpers.ParseUtc("2020-11-17T15:00:00Z"),
                        Metadata = "metadata value",
                        MetadataJson = new JsonObject()
                            {
                                { "metadata_json_key", "metadata json value" }
                            },
                        CorrelationId = "correlation id value"
                    }
                }
            },
                options => options.Excluding(x => x.MessageMetadata)
                    .Excluding(x => x.ConversationStartNotification.Conversation.MetadataJson));
        }

        [Fact]
        public void DeserializeConversationStopEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ConversationStopEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ConversationStopEvent>();

            result.ConversationStopNotification?.Conversation?.MetadataJson?["metadata_json_key"]?
                .GetValue<string>().Should().Be("metadata json value");
            result.Should().BeEquivalentTo(new ConversationStopEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                ConversationStopNotification = new ConversationNotification()
                {
                    Conversation = new Sinch.Conversation.Conversations.Conversation()
                    {
                        Active = true,
                        ActiveChannel = ConversationChannel.WhatsApp,
                        AppId = "conversation app Id",
                        ContactId = "contact ID",
                        Id = "a conversation id",
                        LastReceived = Helpers.ParseUtc("2020-11-17T15:00:00Z"),
                        Metadata = "metadata value",
                        MetadataJson = new JsonObject()
                            {
                                { "metadata_json_key", "metadata json value" }
                            },
                        CorrelationId = "correlation id value"
                    }
                }
            },
                options => options.Excluding(x => x.MessageMetadata)
                    .Excluding(x => x.ConversationStopNotification.Conversation.MetadataJson));
        }

        [Fact]
        public void DeserializeEventDeliveryReportEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/EventDeliveryReportEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<DeliveryEvent>();

            result.Should().BeEquivalentTo(new DeliveryEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                EventDeliveryReport = new EventDeliveryAllOfEventDeliveryReport()
                {
                    EventId = "event id",
                    Status = DeliveryStatus.Delivered,
                    ChannelIdentity = new ChannelIdentity()
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    Reason = new Reason()
                    {
                        Code = "RECIPIENT_NOT_OPTED_IN",
                        Description = "reason description",
                        SubCode = "UNSPECIFIED_SUB_CODE"
                    },
                    Metadata = "metadata value",
                    ProcessingMode = ProcessingMode.Dispatch
                }
            },
                options => options.Excluding(x => x.MessageMetadata));
        }

        [Fact]
        public void DeserializeMessageDeliveryReportEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/MessageDeliveryReceiptEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<MessageDeliveryReceiptEvent>();

            result.Should().BeEquivalentTo(new MessageDeliveryReceiptEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                MessageDeliveryReport = new MessageDeliveryReport()
                {
                    MessageId = "message id",
                    ConversationId = "conversation id",
                    Status = DeliveryStatus.Delivered,
                    ChannelIdentity = new ChannelIdentity()
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    Reason = new Reason()
                    {
                        Code = "RECIPIENT_NOT_OPTED_IN",
                        Description = "reason description",
                        SubCode = "UNSPECIFIED_SUB_CODE"
                    },
                    Metadata = "metadata value",
                    ProcessingMode = ProcessingMode.Dispatch
                }
            },
                options => options.Excluding(x => x.MessageMetadata));
        }

        [Fact]
        public void DeserializeInboundContactEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/InboundContactEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<InboundEvent>();

            result.Should().BeEquivalentTo(new InboundEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                Event = new EventInboundAllOfEvent()
                {
                    Id = "event id",
                    Direction = ConversationDirection.ToApp,
                    ChannelIdentity = new ChannelIdentity()
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    ConversationId = "conversation id",
                    AcceptTime = Helpers.ParseUtc("2020-11-17T16:07:15Z"),
                    ProcessingMode = ProcessingMode.Dispatch,
                    ContactEvent = new ContactEvent()
                    {
                        ComposingEvent = new object() // Empty object
                    }
                }
            },
                options => options.Excluding(x => x.MessageMetadata));
        }

        [Fact]
        public void DeserializeInboundContactMessageEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/InboundContactMessageEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<InboundEvent>();

            result.Should().BeEquivalentTo(new InboundEvent()
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",

                Event = new EventInboundAllOfEvent()
                {
                    Id = "event id",
                    Direction = ConversationDirection.ToApp,
                    ChannelIdentity = new ChannelIdentity()
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    ConversationId = "conversation id",
                    AcceptTime = Helpers.ParseUtc("2020-11-17T16:07:15Z"),
                    ProcessingMode = ProcessingMode.Dispatch,
                    ContactMessageEvent = new ContactMessageEvent()
                    {
                        ReactionEvent = new ReactionEvent()
                        {
                            Emoji = "\uD83D\uDD25 Dotnet SDK",
                            Action = ReactionAction.React,
                            MessageId = "message id value",
                            ReactionCategory = "reaction category"
                        }
                    }
                }
            },
                options => options.Excluding(x => x.MessageMetadata));
        }

        [Fact]
        public void DeserializeMessageInboundEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<MessageInboundEvent>();

            result.Should().BeEquivalentTo(new MessageInboundEvent
            {
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                ProjectId = "project id value",
                AppId = "app id value",
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                MessageMetadata = "metadata value",
                CorrelationId = "correlation id value",
                Message = new MessageInboundEventItem()
                {
                    Id = "event id",
                    Direction = ConversationDirection.ToApp,
                    ChannelIdentity = new ChannelIdentity
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    ConversationId = "conversation id",
                    Metadata = "metadata value",
                    AcceptTime = Helpers.ParseUtc("2020-11-17T16:07:15Z"),
                    SenderId = "sender id value",
                    ProcessingMode = ProcessingMode.Dispatch,
                    Injected = true,
                    ContactMessage = new ContactMessage(new TextMessage("This is a text message."))
                    {
                        ReplyTo = new ReplyTo("message id value")
                    }
                }
            });
        }

        [Fact]
        public void DeserializeMessageSubmitNotificationEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/MessageSubmitEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<MessageSubmitEvent>();

            result.MessageSubmitNotification!.SubmittedMessage!.ExplicitChannelMessage![ConversationChannel.KakaoTalk]
                .GetValue<string>().Should().BeEquivalentTo("foo value");
            result.Should().BeEquivalentTo(new MessageSubmitEvent
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                MessageSubmitNotification = new MessageSubmitNotification
                {
                    MessageId = "message id",
                    ConversationId = "conversation id",
                    ChannelIdentity = new ChannelIdentity
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ContactId = "contact ID",
                    SubmittedMessage = new AppMessage(new TextMessage("This is a text message."))
                    {
                        ExplicitChannelMessage = new Dictionary<ConversationChannel, JsonValue>
                            {
                                { ConversationChannel.KakaoTalk, JsonValue.Create("foo value") }
                            },
                        ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                            {
                                {
                                    ChannelSpecificTemplate.KakaoTalk,
                                    new ChoiceMessage
                                    {
                                        TextMessage = new TextMessage("This is a text message."),
                                        Choices = new List<Choice>
                                        {
                                            new Choice
                                            {
                                                CallMessage = new CallMessage("phone number value", "title value"),
                                                PostbackData = "postback call_message data value"
                                            },
                                            new Choice
                                            {
                                                LocationMessage = new LocationMessage
                                                {
                                                    Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                                    Title = "title value",
                                                    Label = "label value"
                                                },
                                                PostbackData = "postback location_message data value"
                                            },
                                            new Choice
                                            {
                                                TextMessage = new TextMessage("This is a text message."),
                                                PostbackData = "postback text_message data value"
                                            },
                                            new Choice
                                            {
                                                UrlMessage = new UrlMessage
                                                {
                                                    Title = "title value",
                                                    Url = "an url value"
                                                },
                                                PostbackData = "postback url_message data value"
                                            }
                                        }
                                    }
                                }
                            },
                        ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>
                            {
                                {
                                    ConversationChannel.Messenger,
                                    new FlowMessage()
                                    {
                                        Message = new FlowChannelSpecificMessage()
                                        {
                                            FlowId = "1",
                                            FlowCta = "Book!",
                                            Header = new WhatsAppInteractiveTextHeader()
                                            {
                                                Text = "text header value",
                                            },
                                            Body = new()
                                            {
                                                Text = "text body value",
                                            },
                                            Footer = new WhatsAppInteractiveFooter()
                                            {
                                                Text = "Flow message footer"
                                            },
                                            FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                            FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                            FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                            FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                            {
                                                Screen = "<SCREEN_NAME>",
                                                Data = new JsonObject()
                                                {
                                                    ["product_name"] = "name",
                                                    ["product_description"] = "description",
                                                    ["product_price"] = 100
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                        Agent = new Agent
                        {
                            DisplayName = "display_name value",
                            Type = AgentType.Bot,
                            PictureUrl = "picture_url value"
                        }
                    },
                    Metadata = "metadata value",
                    ProcessingMode = ProcessingMode.Dispatch
                }
            },
                options => options.Excluding(x => x.MessageMetadata)
                    .Excluding(x => x.MessageSubmitNotification.SubmittedMessage.ExplicitChannelMessage));
        }

        [Fact]
        public void DeserializeMessageRedactionEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/MessageInboundSmartConversationRedactionEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json)
                .As<MessageInboundSmartConversationRedactionEvent>();

            result.Should().BeEquivalentTo(new MessageInboundSmartConversationRedactionEvent
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                MessageRedaction = new MessageInboundEventItem()
                {
                    Id = "id",
                    Direction = ConversationDirection.ToApp,
                    ContactMessage = new ContactMessage(new MediaMessage
                    {
                        Url = "an url value",
                        ThumbnailUrl = "another url",
                        FilenameOverride = "filename override value"
                    })
                    {
                        ReplyTo = new ReplyTo("message id value")
                    },
                    ChannelIdentity = new ChannelIdentity
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    },
                    ConversationId = "conversation id",
                    ContactId = "contact id",
                    Metadata = "metadata value",
                    AcceptTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                    SenderId = "sender id",
                    ProcessingMode = ProcessingMode.Dispatch,
                    Injected = true
                }
            });
        }

        [Fact]
        public void DeserializeSmartConversationNotificationEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/SmartConversationsEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<SmartConversationsEvent>();

            result.Should().BeEquivalentTo(new SmartConversationsEvent
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                SmartConversationNotification = new SmartConversationNotification
                {
                    ContactId = "contact id",
                    ChannelIdentity = "channel identity",
                    Channel = ConversationChannel.Messenger,
                    MessageId = "message id",
                    ConversationId = "conversation id",
                    AnalysisResults = new AnalysisResult
                    {
                        MlSentimentResult = new List<MachineLearningSentimentResult>
                        {
                            new MachineLearningSentimentResult
                            {
                                Message = "message result",
                                Results = new List<SentimentResult>
                                {
                                    new SentimentResult
                                    {
                                        Sentiment = Sentiment.Positive,
                                        Score = 0.4f
                                    }
                                },
                                Sentiment = Sentiment.Neutral,
                                Score = 0.6f
                            }
                        },
                        MlNluResult = new List<MachineLearningNLUResult>
                        {
                            new MachineLearningNLUResult
                            {
                                Message = "message ml_nlu_result",
                                Results = new List<IntentResult>
                                {
                                    new IntentResult
                                    {
                                        Intent = "chitchat.greeting",
                                        Score = 0.3f
                                    }
                                },
                                Intent = "chitchat.compliment",
                                Score = 0.2f
                            }
                        },
                        MlImageRecognitionResult = new List<MachineLearningImageRecognitionResult>
                        {
                            new MachineLearningImageRecognitionResult
                            {
                                Url = "url value",
                                DocumentImageClassification = new DocumentImageClassification
                                {
                                    DocType = "an image classification",
                                    Confidence = 0.12f
                                },
                                OpticalCharacterRecognition = new OpticalCharacterRecognition
                                {
                                    Result = new List<OpticalCharacterRecognitionData>
                                    {
                                        new OpticalCharacterRecognitionData
                                        {
                                            Data = new List<string> { "string 1", "string 2" }
                                        }
                                    }
                                },
                                DocumentFieldClassification = new DocumentFieldClassification
                                {
                                    Result = new List<Dictionary<string, DocumentFieldData>>
                                    {
                                        new()
                                        {
                                            {
                                                "John",
                                                new DocumentFieldData
                                                {
                                                    Data = new List<string> { "a string" }
                                                }
                                            },
                                            {
                                                "Doe",
                                                new DocumentFieldData
                                                {
                                                    Data = new List<string> { "another string" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        MlPiiResult = new List<MachineLearningPIIResult>
                        {
                            new MachineLearningPIIResult
                            {
                                Message = "analyzed message",
                                Masked = "masked analyzed message"
                            }
                        },
                        MlOffensiveAnalysisResult = new List<OffensiveAnalysis>
                        {
                            new OffensiveAnalysis
                            {
                                Message = "message value",
                                Url = "URL value",
                                Evaluation = Evaluation.Unsafe,
                                Score = 0.456f
                            }
                        }
                    }
                }
            });
        }

        [Fact]
        public void DeserializeUnsupportedCallbackEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/UnsupportedCallbackEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<UnsupportedCallbackEvent>();

            result.Should().BeEquivalentTo(new UnsupportedCallbackEvent
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                UnsupportedCallback = new UnsupportedCallback
                {
                    Channel = ConversationChannel.Messenger,
                    Payload = "payload value",
                    ProcessingMode = ProcessingMode.Dispatch,
                    Id = "id value",
                    ContactId = "contact id",
                    ConversationId = "conversation id",
                    ChannelIdentity = new ChannelIdentity
                    {
                        AppId = "an app id",
                        Channel = ConversationChannel.Messenger,
                        Identity = "an identity"
                    }
                }
            });
        }

        [Fact]
        public void DeserializeOptInNotificationEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/OptInEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<OptInEvent>();

            result.Should().BeEquivalentTo(new OptInEvent()
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                OptInNotification = new OptInEventAllOfOptInNotification()
                {
                    RequestId = "request id",
                    ContactId = "contact id",
                    Channel = ConversationChannel.WhatsApp,
                    Status = OptInStatus.OptInFailed,
                    ErrorDetails = new OptInNotificationErrorDetails()
                    {
                        Description = "Error description value"
                    },
                    ProcessingMode = ProcessingMode.Dispatch
                }
            });
        }

        [Fact]
        public void DeserializeOptOutNotificationEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/OptOutEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<OptOutEvent>();

            result.Should().BeEquivalentTo(new OptOutEvent()
            {
                AppId = "app id value",
                AcceptedTime = Helpers.ParseUtc("2020-11-17T16:05:51.724083Z"),
                EventTime = Helpers.ParseUtc("2020-11-17T16:05:45Z"),
                ProjectId = "project id value",
                CorrelationId = "correlation id value",
                MessageMetadata = "metadata value",
                OptOutNotification = new OptOutNotification
                {
                    RequestId = "request id",
                    ContactId = "contact id",
                    Channel = ConversationChannel.WhatsApp,
                    Status = OptOutStatus.OptOutFailed,
                    ErrorDetails = new OptOutNotificationErrorDetails()
                    {
                        Description = "Error description value"
                    },
                    ProcessingMode = ProcessingMode.Dispatch
                }
            });
        }

        [Fact]
        public void DeserializeCommentEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/CommentEvent.json");

            var result = JsonSerializer.Deserialize<ContactEvent>(json);

            result.CommentEvent.Should().BeEquivalentTo(new CommentEvent()
            {
                Id = "comment id",
                Text = "text value",
                CommentType = CommentType.Live,
                CommentedOn = "https/my.link.com",
                User = "user name"
            });
        }

        [Fact]
        public void DeserializePaymentStatusUpdateEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/PaymentStatusUpdateEvent.json");

            var result = JsonSerializer.Deserialize<ContactMessageEvent>(json);

            result.PaymentStatusUpdateEvent.Should().BeEquivalentTo(new PaymentStatusUpdateEvent()
            {
                ReferenceId = "a reference id",
                PaymentStatus = PaymentStatus.PaymentStatusCaptured,
                PaymentTransactionStatus = PaymentTransactionStatus.PaymentStatusTransactionPending,
                PaymentTransactionId = "a payment transaction id",
            });
        }

        [Fact]
        public void DeserializeShortlinkActivatedEvent()
        {
            string json = Helpers.LoadResources("Conversation/Hooks/ShortlinkActivatedEvent.json");

            var result = JsonSerializer.Deserialize<ContactMessageEvent>(json);

            result.ShortlinkActivatedEvent.Should().BeEquivalentTo(new ShortlinkActivatedEvent()
            {
                Payload = "payload value",
                Title = "title value",
                Ref = "ref value",
                Source = "SHORTLINK",
                Type = "OPEN_THREAD"
            });
        }
    }
}
