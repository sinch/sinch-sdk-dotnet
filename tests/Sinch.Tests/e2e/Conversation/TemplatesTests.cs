using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.TemplatesV2;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class TemplatesTests : TestBase
    {
        private static readonly DateTime DefaultTime = Helpers.ParseUtc("1970-01-01T00:00:00Z");

        private readonly Template _template = new Template()
        {
            Id = "template_id",
            Description = "template",
            CreateTime = DefaultTime,
            DefaultTranslation = "en-US",
            UpdateTime = DefaultTime,
            Version = 3,
            Translations = new List<TemplateTranslation>()
            {
                new TemplateTranslation(new TextMessage("hello"))
                {
                    Version = "1", // is this really a string?
                    CreateTime = DefaultTime,
                    UpdateTime = DefaultTime,
                    LanguageCode = "en_US",
                    ChannelTemplateOverrides = null,
                    Variables = new List<TypeTemplateVariable>()
                    {
                        new TypeTemplateVariable()
                        {
                            Key = "a",
                            PreviewValue = "c"
                        }
                    }
                }
            },
        };

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV2.Get("template_id");
            response.Should().BeEquivalentTo(_template);
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.TemplatesV2.Delete(_template.Id);
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV2.List();
            response.Should().BeEquivalentTo(new List<Template>() { _template, _template });
        }

        [Fact]
        public async Task Create()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV2.Create(_template);
            response.Should().BeEquivalentTo(_template);
        }

        [Fact]
        public async Task ListTranslations()
        {
            var response =
                await SinchClientMockServer.Conversation.TemplatesV2.ListTranslations(_template.Id, "en_US", "3");
            response.Should().BeEquivalentTo(_template.Translations);
        }

        [Fact]
        public async Task Update()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV2.Update(_template);
            response.Should().BeEquivalentTo(_template);
        }
    }
}
