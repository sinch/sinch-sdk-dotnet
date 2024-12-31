using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.TemplatesV1;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class TemplatesV1Tests : TestBase
    {
        private Template _template = new Template()
        {
            Id = "01W4FFL35P4NC4K35TEMPLATE01",
            Description = "Text template V1",
            DefaultTranslation = "en-US",
            Channel = TemplateChannel.Unspecified,
            CreateTime = Helpers.ParseUtc("2024-06-06T14:42:42Z"),
            UpdateTime = Helpers.ParseUtc("2024-06-06T14:42:42Z"),
            Translations = new List<TemplateTranslation>()
            {
                new TemplateTranslation()
                {
                    LanguageCode = "en-US",
                    Content =
                        "{\"text_message\":{\"text\":\"Hello ${name}. Text message template created with V1 API\"}}",
                    Version = "1",
                    CreateTime = Helpers.ParseUtc("2024-06-06T14:42:42Z"),
                    UpdateTime = Helpers.ParseUtc("2024-06-06T14:42:42Z"),
                    Variables = new List<TypeTemplateVariable>()
                    {
                        new TypeTemplateVariable()
                        {
                            Key = "name",
                            PreviewValue = "Professor Jones"
                        }
                    }
                }
            }
        };

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV1.Get("templateId");
            response.Should().BeEquivalentTo(_template);
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.TemplatesV1.Delete("templateId");
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task List()
        {
            var templates = await SinchClientMockServer.Conversation.TemplatesV1.List();
            templates.Should().BeEquivalentTo(new List<Template>
            {
                _template,
                new Template()
                {
                    Id = "01W4FFL35P4NC4K35TEMPLATE02",
                    Channel = TemplateChannel.Unspecified,
                    CreateTime = Helpers.ParseUtc("2024-06-06T15:50:00Z"),
                    UpdateTime = Helpers.ParseUtc("2024-06-06T15:52:52Z"),
                    DefaultTranslation = "en-US",
                    Description = "Temple of Doom location",
                    Translations = new List<TemplateTranslation>()
                    {
                        new TemplateTranslation()
                        {
                            LanguageCode = "en-US",
                            Content =
                                "{\"location_message\":{\"title\":\"Temple of Doom\",\"label\":\"The temple may be here\",\"coordinates\":{\"longitude\":78.8613,\"latitude\":30.2884}}}",
                            Version = "4",
                            CreateTime = Helpers.ParseUtc("2024-06-06T15:52:52Z"),
                            UpdateTime = Helpers.ParseUtc("2024-06-06T15:52:52Z"),
                            Variables = new List<TypeTemplateVariable>()
                        }
                    }
                }
            });
        }

        [Fact]
        public async Task Update()
        {
            var response = await SinchClientMockServer.Conversation.TemplatesV1.Update(new UpdateTemplateRequest()
            {
                Id = "01W4FFL35P4NC4K35TEMPLATE02",
                Description = "Updated text template V1",
                DefaultTranslation = "fr-FR",
                Channel = TemplateChannel.Sms,
                Translations = new List<TemplateTranslation>()
                {
                    new TemplateTranslation()
                    {
                        LanguageCode = "en-US",
                        Version = "2",
                        Content =
                            "{\"text_message\":{\"text\":\"Hello ${name}. This text message template has been created with V1 API\"}}",
                        Variables = new List<TypeTemplateVariable>()
                        {
                            new TypeTemplateVariable()
                            {
                                Key = "name",
                                PreviewValue = "Professor Jones"
                            }
                        }
                    },
                    new TemplateTranslation()
                    {
                        LanguageCode = "fr-FR",
                        Version = "1",
                        Content =
                            "{\"text_message\":{\"text\":\"Bonjour ${name}. Ce message texte provient d'un template V1\"}}",
                        Variables = new List<TypeTemplateVariable>()
                        {
                            new TypeTemplateVariable()
                            {
                                Key = "name",
                                PreviewValue = "Professeur Jones"
                            }
                        }
                    },
                }
            });

            response.Should().BeEquivalentTo(new Template()
            {
                Id = "01W4FFL35P4NC4K35TEMPLATE01",
                Description = "Updated text template V1",
                DefaultTranslation = "fr-FR",
                Channel = TemplateChannel.Unspecified,
                CreateTime = Helpers.ParseUtc("2024-06-06T14:42:42Z"),
                UpdateTime = Helpers.ParseUtc("2024-06-06T14:45:45Z"),
                Translations = new List<TemplateTranslation>()
                {
                    new TemplateTranslation()
                    {
                        LanguageCode = "en-US",
                        Version = "2",
                        Content =
                            "{\"text_message\":{\"text\":\"Hello ${name}. This text message template has been created with V1 API\"}}",
                        Variables = new List<TypeTemplateVariable>()
                        {
                            new TypeTemplateVariable()
                            {
                                Key = "name",
                                PreviewValue = "Professor Jones"
                            }
                        },
                        CreateTime = Helpers.ParseUtc("2024-06-06T14:45:45Z"),
                        UpdateTime = Helpers.ParseUtc("2024-06-06T14:45:45Z"),
                    },
                    new TemplateTranslation()
                    {
                        LanguageCode = "fr-FR",
                        Version = "1",
                        Content =
                            "{\"text_message\":{\"text\":\"Bonjour ${name}. Ce message texte provient d'un template V1\"}}",
                        Variables = new List<TypeTemplateVariable>()
                        {
                            new TypeTemplateVariable()
                            {
                                Key = "name",
                                PreviewValue = "Professeur Jones"
                            }
                        },
                        CreateTime = Helpers.ParseUtc("2024-06-06T14:45:45Z"),
                        UpdateTime = Helpers.ParseUtc("2024-06-06T14:45:45Z"),
                    },
                }
            });
        }

        [Fact]
        public async Task Create()
        {
            var template = await SinchClientMockServer.Conversation.TemplatesV1.Create(new CreateTemplateRequest()
            {
                DefaultTranslation = "en-US",
                Channel = TemplateChannel.Messenger,
                Description = "Text template V1",
                Translations = new List<TemplateTranslation>()
                {
                    new TemplateTranslation()
                    {
                        LanguageCode = "en-US",
                        Version = "1",
                        Content =
                            "{\"text_message\":{\"text\":\"Hello ${name}. Text message template created with V1 API\"}}",
                        Variables = new List<TypeTemplateVariable>()
                        {
                            new TypeTemplateVariable()
                            {
                                Key = "name",
                                PreviewValue = "Professor Jones"
                            }
                        }
                    }
                }
            });

            template.Should().BeEquivalentTo(_template);
        }
    }
}
