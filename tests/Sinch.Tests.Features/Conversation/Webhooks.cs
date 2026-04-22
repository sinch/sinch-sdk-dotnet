using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation.EventDestinations;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Webhooks
{
    private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
    private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
    private const string WebhookId001 = "01W4FFL35P4NC4K35WEBHOOK001";
    private const string WebhookId004 = "01W4FFL35P4NC4K35WEBHOOK004";

    private ISinchConversationWebhooks _webhooks;
    private Webhook _webhook;
    private List<Webhook> _webhooksList;
    private bool _deleteCompleted;

    [Given(@"the Conversation service ""Webhooks"" is available")]
    public void GivenTheConversationServiceWebhooksIsAvailable()
    {
        _webhooks = Utils.SinchConversationClient().Webhooks;
    }

    [When(@"I send a request to create a conversation webhook")]
    public async Task WhenISendARequestToCreateAConversationWebhook()
    {
        _webhook = await _webhooks.Create(new CreateWebhookRequest
        {
            AppId = AppId001,
            Target = "https://my-callback-server.com/capability",
            Triggers = [WebhookTrigger.Capability],
            Secret = "CactusKnight_SurfsWaves",
            TargetType = WebhookTargetType.Http
        });
    }

    [Then(@"the conversation webhook is created")]
    public void ThenTheConversationWebhookIsCreated()
    {
        _webhook.Should().NotBeNull();
        _webhook.Id.Should().Be(WebhookId004);
        _webhook.AppId.Should().Be(AppId001);
        _webhook.Target.Should().Be("https://my-callback-server.com/capability");
        _webhook.TargetType.Should().Be(WebhookTargetType.Http);
        _webhook.Secret.Should().Be("CactusKnight_SurfsWaves");
        _webhook.Triggers.Should().ContainSingle().Which.Should().Be(WebhookTrigger.Capability);
        _webhook.ClientCredentials.Should().BeNull();
    }

    [When(@"I send a request to list the conversation webhooks for an app")]
    public async Task WhenISendARequestToListTheConversationWebhooksForAnApp()
    {
        var response = await _webhooks.List(AppId001);
        _webhooksList = response.Webhooks?.ToList() ?? new List<Webhook>();
    }

    [Then(@"the response contains the list of conversation webhooks")]
    public void ThenTheResponseContainsTheListOfConversationWebhooks()
    {
        _webhooksList.Should().HaveCount(4);
        _webhooksList.Should().ContainSingle(x => x.Id == WebhookId001 && x.AppId == AppId001);
        _webhooksList.Should().ContainSingle(x => x.Id == WebhookId004 && x.Target == "https://my-callback-server.com/capability");
    }

    [When(@"I send a request to retrieve a conversation webhook")]
    public async Task WhenISendARequestToRetrieveAConversationWebhook()
    {
        _webhook = await _webhooks.Get(WebhookId001);
    }

    [Then(@"the response contains the conversation webhook details")]
    public void ThenTheResponseContainsTheConversationWebhookDetails()
    {
        _webhook.Should().NotBeNull();
        _webhook.Id.Should().Be(WebhookId001);
        _webhook.AppId.Should().Be(AppId001);
        _webhook.Target.Should().Be("https://my-callback-server.com/unsupported");
        _webhook.TargetType.Should().Be(WebhookTargetType.Http);
        _webhook.Secret.Should().Be("VeganVampire_SipsTea");
        _webhook.Triggers.Should().ContainSingle().Which.Should().Be(WebhookTrigger.Unsupported);
        _webhook.ClientCredentials.Should().NotBeNull();
        _webhook.ClientCredentials!.ClientId.Should().Be("webhook-username");
    }

    [When(@"I send a request to update a conversation webhook")]
    public async Task WhenISendARequestToUpdateAConversationWebhook()
    {
        _webhook = await _webhooks.Update(WebhookId004, new UpdateWebhookRequest
        {
            AppId = AppId002,
            Target = "https://my-callback-server.com/capability-optin-optout",
            Triggers = [WebhookTrigger.Capability, WebhookTrigger.OptIn, WebhookTrigger.OptOut],
            Secret = "SpacePanda_RidesUnicycle"
        });
    }

    [Then(@"the response contains the conversation webhook details with updated data")]
    public void ThenTheResponseContainsTheConversationWebhookDetailsWithUpdatedData()
    {
        _webhook.Should().NotBeNull();
        _webhook.Id.Should().Be(WebhookId004);
        _webhook.AppId.Should().Be(AppId002);
        _webhook.Target.Should().Be("https://my-callback-server.com/capability-optin-optout");
        _webhook.TargetType.Should().Be(WebhookTargetType.Http);
        _webhook.Secret.Should().Be("SpacePanda_RidesUnicycle");
        _webhook.Triggers.Should().BeEquivalentTo(
            [WebhookTrigger.Capability, WebhookTrigger.OptIn, WebhookTrigger.OptOut]);
    }

    [When(@"I send a request to delete a conversation webhook")]
    public async Task WhenISendARequestToDeleteAConversationWebhook()
    {
        await _webhooks.Delete(WebhookId004);
        _deleteCompleted = true;
    }

    [Then(@"the delete conversation webhook response contains no data")]
    public void ThenTheDeleteConversationWebhookResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }
}
