using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.TemplatesV2;
using Xunit;

namespace Sinch.Tests.Conversation.TemplatesV2
{
    public class TemplateTranslationTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeTemplateTranslationWithTextMessage()
        {
            var json = Helpers.LoadResources("Conversation/TemplatesV2/TemplateTranslationWithTextMessage.json");

            var result = JsonSerializer.Deserialize<TemplateTranslation>(json, Conversation.JsonSerializerOptions);

            result.Should().BeEquivalentTo(new TemplateTranslation(new TextMessage("Hello, this is a template text message."))
            {
                LanguageCode = "en-US",
                Version = "1.0",
                Variables = new List<TypeTemplateVariable>
                {
                    new TypeTemplateVariable { Key = "name", PreviewValue = "John Doe" },
                    new TypeTemplateVariable { Key = "order_id", PreviewValue = "12345" }
                },
                CreateTime = DateTime.Parse("2024-01-15T10:30:00Z").ToUniversalTime(),
                UpdateTime = DateTime.Parse("2024-01-16T14:45:00Z").ToUniversalTime()
            });
        }

        [Fact]
        public void SerializeTemplateTranslationWithTextMessage()
        {
            var templateTranslation = new TemplateTranslation(new TextMessage("Hello, this is a template text message."))
            {
                LanguageCode = "en-US",
                Version = "1.0",
                Variables = new List<TypeTemplateVariable>
                {
                    new TypeTemplateVariable { Key = "name", PreviewValue = "John Doe" },
                    new TypeTemplateVariable { Key = "order_id", PreviewValue = "12345" }
                },
                CreateTime = DateTime.Parse("2024-01-15T10:30:00Z").ToUniversalTime(),
                UpdateTime = DateTime.Parse("2024-01-16T14:45:00Z").ToUniversalTime()
            };

            var actual = JsonSerializer.Serialize(templateTranslation, Conversation.JsonSerializerOptions);

            var expected = Helpers.LoadResources("Conversation/TemplatesV2/TemplateTranslationWithTextMessage.json");

            Helpers.AssertJsonEqual(expected, actual);
        }
    }
}
