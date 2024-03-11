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
        private readonly DateTime _defaultTime = Helpers.ParseUtc("1970-01-01T00:00:00Z");
        
        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV2.Get("template_id");
            response.Should().BeEquivalentTo(new Template()
            {
                Id = "template_id",
                Description = "template",
                CreateTime = _defaultTime,
                DefaultTranslation = "en-US",
                UpdateTime = _defaultTime,
                Version = 3,
                Translations = new List<TemplateTranslation>()
                {
                    new TemplateTranslation(new TextMessage("hello"))
                    {
                        Version = "1", // is this really a string?
                        CreateTime = _defaultTime,
                        UpdateTime = _defaultTime,
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
            });
        }
        
        
    }
}
