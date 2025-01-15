using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Sinch.Conversation;
using Sinch.Conversation.Hooks;
using Sinch.Conversation.Hooks.Models;
using Sinch.Conversation.Messages.Message;
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
            }, JsonNode.Parse(str)!.AsObject(), "foo_secret1234");
            isValid.Should().BeTrue();
        }

        [Fact]
        public void DeserializeCapabilityEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/CapabilityEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<CapabilityEvent>();

            result.MessageMetadata?.GetValue<string>().Should().Be("metadata value");
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
            }, options => options.Excluding(x => x.MessageMetadata));
        }

        [Fact]
        public void DeserializeChannelEvent()
        {
            string json =
                Helpers.LoadResources("Conversation/Hooks/ChannelEvent.json");

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json).As<ChannelEvent>();

            result.MessageMetadata?.GetValue<string>().Should().Be("metadata value");
            result.ChannelEventNotification?.ChannelEvent?.AdditionalData?["quality_rating"]?.GetValue<string>().Should()
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
    }
}
