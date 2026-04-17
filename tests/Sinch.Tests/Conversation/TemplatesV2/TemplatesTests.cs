using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.TemplatesV2;
using Xunit;

namespace Sinch.Tests.Conversation.TemplatesV2
{
    public class TemplatesTests : ConversationTestBase
    {
        private const string TemplateId001 = "01HVN010MG3B9N6X323JAFN59P";
        private const string TemplateId002 = "01W4FFL35P4NC4K35TEMPLATEV2";

        private readonly string _templatesUrl =
            $"https://us.template.api.sinch.com/v2/projects/{ProjectId}/templates";

        #region Create Tests

        [Fact]
        public async Task Create_WithValidRequest_ReturnsTemplate()
        {
            var expectedRequest = new CreateTemplateRequest
            {
                Id = TemplateId001,
                DefaultTranslation = "en-US",
                Description = "Text template V2",
                Translations = new List<TemplateTranslation>
                {
                    new TemplateTranslation(new TextMessage("Hello ${name}. Text message template created with V2 API"))
                    {
                        LanguageCode = "en-US",
                        Version = "3",
                        Variables = new List<TypeTemplateVariable>
                        {
                            new TypeTemplateVariable { Key = "name", PreviewValue = "Professor Jones" }
                        }
                    }
                }
            };

            var expectedResponse = new Template { Id = TemplateId001, Version = 1 };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, _templatesUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Templates.Create(expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(TemplateId001);
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithoutParams_ReturnsTemplates()
        {
            var templates = new List<Template>
            {
                new Template { Id = TemplateId001, Version = 1 },
                new Template { Id = TemplateId002, Version = 2 }
            };
            var responseWrapper = new { templates };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, _templatesUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    responseWrapper,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Templates.List();

            response.Should().NotBeNull();
            response.Templates.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListAuto_WithTemplates_YieldsAllTemplates()
        {
            var templates = new List<Template>
            {
                new Template { Id = TemplateId001, Version = 1 },
                new Template { Id = TemplateId002, Version = 2 }
            };
            var responseWrapper = new { templates };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, _templatesUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    responseWrapper,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var result = new List<Template>();
            await foreach (var template in Conversation.Templates.ListAuto())
                result.Add(template);

            result.Should().HaveCount(2);
        }

        #endregion

        #region ListTranslations Tests

        [Fact]
        public async Task ListTranslations_WithTemplateId_ReturnsTranslations()
        {
            var translations = new List<TemplateTranslation>
            {
                new TemplateTranslation(new TextMessage("Message from a template v2.")) { LanguageCode = "en-US", Version = "1" },
                new TemplateTranslation(new TextMessage("Message fr")) { LanguageCode = "fr-FR", Version = "1" }
            };
            var responseWrapper = new { translations };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_templatesUrl}/{TemplateId002}/translations")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    responseWrapper,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Templates.ListTranslations(TemplateId002, string.Empty, string.Empty);

            response.Should().NotBeNull();
            response.Should().HaveCount(2);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidTemplateId_ReturnsTemplate()
        {
            var expectedResponse = new Template { Id = TemplateId001, Version = 1 };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_templatesUrl}/{TemplateId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Templates.Get(TemplateId001);

            response.Should().NotBeNull();
            response.Id.Should().Be(TemplateId001);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidRequest_ReturnsUpdatedTemplate()
        {
            var expectedRequest = new UpdateTemplateRequest
            {
                Id = TemplateId001,
                Version = 1,
                Description = "Updated description v2",
                DefaultTranslation = "en-US",
                Translations = new List<TemplateTranslation>
                {
                    new TemplateTranslation(new ListMessage
                    {
                        Title = "Choose your icecream flavor",
                        Description = "The best icecream in town!",
                        Sections = new List<ListSection>
                        {
                            new ListSection
                            {
                                Title = "Fruit flavors",
                                Items = new List<IListItem>
                                {
                                    new ChoiceItem { Title = "Strawberry", PostbackData = "Strawberry postback" },
                                    new ChoiceItem { Title = "Blueberry", PostbackData = "Blueberry postback" }
                                }
                            },
                            new ListSection
                            {
                                Title = "Other flavors",
                                Items = new List<IListItem>
                                {
                                    new ChoiceItem { Title = "Chocolate", PostbackData = "Chocolate postback" },
                                    new ChoiceItem { Title = "Vanilla", PostbackData = "Vanilla postback" }
                                }
                            }
                        }
                    })
                    {
                        LanguageCode = "en-US",
                        Version = "1"
                    }
                }
            };

            var expectedResponse = new Template { Id = TemplateId001, Version = 2 };

            HttpMessageHandlerMock
                .When(HttpMethod.Put, $"{_templatesUrl}/{TemplateId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Templates.Update(expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(TemplateId001);
            response.Version.Should().Be(2);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidTemplateId_DeletesSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{_templatesUrl}/{TemplateId002}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, new StringContent(string.Empty));

            await Conversation.Templates.Delete(TemplateId002);
        }

        #endregion
    }
}
