using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.TemplatesV2;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Templates
{
    private const string TemplateId001 = "01HVN010MG3B9N6X323JAFN59P";
    private const string TemplateId002 = "01W4FFL35P4NC4K35TEMPLATEV2";

    private ISinchConversationTemplates _templates;
    private Template _template;
    private ListTemplatesResponse _templatesList;
    private IEnumerable<TemplateTranslation> _translationsList;
    private bool _deleteCompleted;

    [Given(@"the Conversation service ""TemplatesV2"" is available")]
    public void GivenTheConversationServiceTemplatesV2IsAvailable()
    {
        _templates = Utils.SinchConversationClient().Templates;
    }

    [When(@"I send a request to create a conversation template with the V2 API")]
    public async Task WhenISendARequestToCreateAConversationTemplateWithTheV2Api()
    {
        _template = await _templates.Create(new CreateTemplateRequest
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
        });
    }

    [Then(@"the conversation template V2 is created")]
    public void ThenTheConversationTemplateV2IsCreated()
    {
        _template.Should().NotBeNull();
        _template.Id.Should().Be(TemplateId001);
        _template.Version.Should().Be(1);
        _template.Translations.Should().HaveCount(1);
        _template.Translations![0].Version.Should().Be("3");
    }

    [When(@"I send a request to list the conversation templates with the V2 API")]
    public async Task WhenISendARequestToListTheConversationTemplatesWithTheV2Api()
    {
        _templatesList = await _templates.List();
    }

    [Then(@"the response contains the list of conversation templates with the V2 structure")]
    public void ThenTheResponseContainsTheListOfConversationTemplatesWithTheV2Structure()
    {
        _templatesList.Should().NotBeNull();
        _templatesList.Templates.Should().HaveCount(2);
    }

    [Then(@"for each templateV2 in the templateV2 list response, it defines a translation with version ""latest"" on top of each current translation version")]
    public void ThenForEachTemplateV2InTheTemplateV2ListResponseItDefinesATranslationWithVersionLatestOnTopOfEachCurrentTranslationVersion()
    {
        foreach (var template in _templatesList.Templates!)
        {
            var translations = template.Translations!;
            var latestVersionCount = translations.Count(t => t.Version == "latest");
            var otherVersionCount = translations.Count(t => t.Version != "latest");
            latestVersionCount.Should().Be(otherVersionCount);
        }
    }

    [When(@"I send a request to list the translations for a template with the V2 API")]
    public async Task WhenISendARequestToListTheTranslationsForATemplateWithTheV2Api()
    {
        _translationsList = await _templates.ListTranslations(TemplateId002, string.Empty, string.Empty);
    }

    [Then(@"the response contains the list of translations for a template with the V2 structure")]
    public void ThenTheResponseContainsTheListOfTranslationsForATemplateWithTheV2Structure()
    {
        _translationsList.Should().NotBeNull();
        _translationsList.Should().HaveCount(2);
        _translationsList.Should().NotContain(t => t.Version == "latest");
    }

    [When(@"I send a request to retrieve a conversation template with the V2 API")]
    public async Task WhenISendARequestToRetrieveAConversationTemplateWithTheV2Api()
    {
        _template = await _templates.Get(TemplateId001);
    }

    [Then(@"the response contains the conversation template details with the V2 structure")]
    public void ThenTheResponseContainsTheConversationTemplateDetailsWithTheV2Structure()
    {
        _template.Should().NotBeNull();
        _template.Id.Should().Be(TemplateId001);
        _template.Description.Should().Be("Text template V2");
        _template.Version.Should().Be(1);
        _template.Translations.Should().HaveCount(2);
    }

    [When(@"I send a request to update a conversation template with the V2 API")]
    public async Task WhenISendARequestToUpdateAConversationTemplateWithTheV2Api()
    {
        _template = await _templates.Update(new UpdateTemplateRequest
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
        });
    }

    [Then(@"the response contains the conversation template details with updated data with the V2 structure")]
    public void ThenTheResponseContainsTheConversationTemplateDetailsWithUpdatedDataWithTheV2Structure()
    {
        _template.Should().NotBeNull();
        _template.Id.Should().Be(TemplateId001);
        _template.Description.Should().Be("Updated description v2");
        _template.Version.Should().Be(2);
    }

    [When(@"I send a request to delete a conversation template with the V2 API")]
    public async Task WhenISendARequestToDeleteAConversationTemplateWithTheV2Api()
    {
        await _templates.Delete(TemplateId002);
        _deleteCompleted = true;
    }

    [Then(@"the delete conversation template response V2 contains no data")]
    public void ThenTheDeleteConversationTemplateResponseV2ContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }
}
