using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation.EventDestinations;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class EventDestinations
{
    private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
    private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
    private const string EventDestinationId001 = "01W4FFL35P4NC4K35WEBHOOK001";
    private const string EventDestinationId004 = "01W4FFL35P4NC4K35WEBHOOK004";

    private ISinchConversationEventDestinations _eventDestinations;
    private EventDestination _eventDestination;
    private List<EventDestination> _eventDestinationsList;
    private bool _deleteCompleted;

    [Given(@"the Conversation service ""Webhooks"" is available")]
    public void GivenTheConversationServiceEventDestinationsIsAvailable()
    {
        _eventDestinations = Utils.SinchConversationClient().EventDestinations;
    }

    [When(@"I send a request to create a conversation webhook")]
    public async Task WhenISendARequestToCreateAConversationEventDestination()
    {
        _eventDestination = await _eventDestinations.Create(new CreateEventDestinationRequest
        {
            AppId = AppId001,
            Target = "https://my-callback-server.com/capability",
            Triggers = [EventDestinationTrigger.Capability],
            Secret = "CactusKnight_SurfsWaves",
            TargetType = EventDestinationTargetType.Http
        });
    }

    [Then(@"the conversation webhook is created")]
    public void ThenTheConversationEventDestinationIsCreated()
    {
        _eventDestination.Should().NotBeNull();
        _eventDestination.Id.Should().Be(EventDestinationId004);
        _eventDestination.AppId.Should().Be(AppId001);
        _eventDestination.Target.Should().Be("https://my-callback-server.com/capability");
        _eventDestination.TargetType.Should().Be(EventDestinationTargetType.Http);
        _eventDestination.Secret.Should().Be("CactusKnight_SurfsWaves");
        _eventDestination.Triggers.Should().ContainSingle().Which.Should().Be(EventDestinationTrigger.Capability);
        _eventDestination.ClientCredentials.Should().BeNull();
    }

    [When(@"I send a request to list the conversation webhooks for an app")]
    public async Task WhenISendARequestToListTheConversationEventDestinationForAnApp()
    {
        var response = await _eventDestinations.List(AppId001);
        _eventDestinationsList = response.EventDestinations?.ToList() ?? new List<EventDestination>();
    }

    [Then(@"the response contains the list of conversation webhooks")]
    public void ThenTheResponseContainsTheListOfConversationEventDestinations()
    {
        _eventDestinationsList.Should().HaveCount(4);
        _eventDestinationsList.Should().ContainSingle(x => x.Id == EventDestinationId001 && x.AppId == AppId001);
        _eventDestinationsList.Should().ContainSingle(x => x.Id == EventDestinationId004 && x.Target == "https://my-callback-server.com/capability");
    }

    [When(@"I send a request to retrieve a conversation webhook")]
    public async Task WhenISendARequestToRetrieveAConversationEventDestination()
    {
        _eventDestination = await _eventDestinations.Get(EventDestinationId001);
    }

    [Then(@"the response contains the conversation webhook details")]
    public void ThenTheResponseContainsTheConversationEventDestinationDetails()
    {
        _eventDestination.Should().NotBeNull();
        _eventDestination.Id.Should().Be(EventDestinationId001);
        _eventDestination.AppId.Should().Be(AppId001);
        _eventDestination.Target.Should().Be("https://my-callback-server.com/unsupported");
        _eventDestination.TargetType.Should().Be(EventDestinationTargetType.Http);
        _eventDestination.Secret.Should().Be("VeganVampire_SipsTea");
        _eventDestination.Triggers.Should().ContainSingle().Which.Should().Be(EventDestinationTrigger.Unsupported);
        _eventDestination.ClientCredentials.Should().NotBeNull();
        _eventDestination.ClientCredentials!.ClientId.Should().Be("webhook-username");
    }

    [When(@"I send a request to update a conversation webhook")]
    public async Task WhenISendARequestToUpdateAConversationEventDestination()
    {
        _eventDestination = await _eventDestinations.Update(EventDestinationId004, new UpdateEventDestinationRequest
        {
            AppId = AppId002,
            Target = "https://my-callback-server.com/capability-optin-optout",
            Triggers = [EventDestinationTrigger.Capability, EventDestinationTrigger.OptIn, EventDestinationTrigger.OptOut],
            Secret = "SpacePanda_RidesUnicycle"
        });
    }

    [Then(@"the response contains the conversation webhook details with updated data")]
    public void ThenTheResponseContainsTheConversationEventDestinationDetailsWithUpdatedData()
    {
        _eventDestination.Should().NotBeNull();
        _eventDestination.Id.Should().Be(EventDestinationId004);
        _eventDestination.AppId.Should().Be(AppId002);
        _eventDestination.Target.Should().Be("https://my-callback-server.com/capability-optin-optout");
        _eventDestination.TargetType.Should().Be(EventDestinationTargetType.Http);
        _eventDestination.Secret.Should().Be("SpacePanda_RidesUnicycle");
        _eventDestination.Triggers.Should().BeEquivalentTo(
            [EventDestinationTrigger.Capability, EventDestinationTrigger.OptIn, EventDestinationTrigger.OptOut]);
    }

    [When(@"I send a request to delete a conversation webhook")]
    public async Task WhenISendARequestToDeleteAConversationEventDestination()
    {
        await _eventDestinations.Delete(EventDestinationId004);
        _deleteCompleted = true;
    }

    [Then(@"the delete conversation webhook response contains no data")]
    public void ThenTheDeleteConversationEventDestinationResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }
}
