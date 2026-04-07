using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Events.Send;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Events
{
    private ISinchConversationEvents _events;
    private SendEventResponse _sendResponse;
    private ListEventsResponse _listResponse;
    private List<ConversationEvent> _eventsList;
    private int _pagesIteration;
    private ConversationEvent _event;
    private bool _deleteCompleted;

    [Given(@"the Conversation service ""Events"" is available")]
    public void GivenTheConversationServiceEventsIsAvailable()
    {
        _events = Utils.SinchConversationClient().Events;
    }

    [When(@"I send a request to send a conversation event to a contact")]
    public async Task WhenISendARequestToSendAConversationEventToAContact()
    {
        _sendResponse = await _events.Send(new SendEventRequest
        {
            AppId = "01W4FFL35P4NC4K35CONVAPP001",
            Recipient = new ContactRecipient { ContactId = "01W4FFL35P4NC4K35CONTACT001" },
            Event = new AppEvent(new ComposingEvent())
        });
    }

    [Then(@"the response contains the id of the conversation event")]
    public void ThenTheResponseContainsTheIdOfTheConversationEvent()
    {
        _sendResponse.EventId.Should().Be("01W4FFL35P4NC4K35CONVEVENT1");
    }

    [When(@"I send a request to list the existing conversation events")]
    public async Task WhenISendARequestToListTheExistingConversationEvents()
    {
        _listResponse = await _events.List(new ListEventsRequest
        {
            PageSize = 2
        });
    }

    [Then(@"the response contains ""(.*)"" conversation events")]
    public void ThenTheResponseContainsConversationEvents(string expectedCount)
    {
        _listResponse.Events.Should().HaveCount(int.Parse(expectedCount));
    }

    [When(@"I send a request to list all the conversation events")]
    public async Task WhenISendARequestToListAllTheConversationEvents()
    {
        _eventsList = new List<ConversationEvent>();
        await foreach (var ev in _events.ListAuto(new ListEventsRequest { PageSize = 2 }))
        {
            _eventsList.Add(ev);
        }
    }

    [When(@"I iterate manually over the conversation events pages")]
    public async Task WhenIIterateManuallyOverTheConversationEventsPages()
    {
        _eventsList = new List<ConversationEvent>();
        _listResponse = await _events.List(new ListEventsRequest { PageSize = 2 });
        _eventsList.AddRange(_listResponse.Events ?? new List<ConversationEvent>());
        _pagesIteration = 1;

        while (!string.IsNullOrEmpty(_listResponse.NextPageToken))
        {
            _listResponse = await _events.List(new ListEventsRequest
            {
                PageSize = 2,
                PageToken = _listResponse.NextPageToken
            });
            _eventsList.AddRange(_listResponse.Events ?? new List<ConversationEvent>());
            _pagesIteration++;
        }
    }

    [Then(@"the conversation events list contains ""(.*)"" conversation events")]
    public void ThenTheConversationEventsListContainsConversationEvents(string expectedCount)
    {
        _eventsList.Should().HaveCount(int.Parse(expectedCount));
    }

    [Then(@"the conversation events iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheConversationEventsIterationResultContainsTheDataFromPages(string expectedPages)
    {
        _pagesIteration.Should().Be(int.Parse(expectedPages));
    }

    [When(@"I send a request to retrieve a conversation event")]
    public async Task WhenISendARequestToRetrieveAConversationEvent()
    {
        _event = await _events.Get("01W4FFL35P4NC4K35CONVEVENT1");
    }

    [Then(@"the response contains the conversation event details")]
    public void ThenTheResponseContainsTheConversationEventDetails()
    {
        _event.Id.Should().Be("01W4FFL35P4NC4K35CONVEVENT1");
        _event.Direction.Should().Be(ConversationDirection.ToContact);
        _event.ConversationId.Should().Be("01W4FFL35P4NC4K35CONVERSATI");
        _event.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
        var expectedChannelIdentity = new ChannelIdentity
        {
            Channel = ConversationChannel.Messenger,
            Identity = "7968425018576406",
            AppId = "01W4FFL35P4NC4K35CONVAPP001"
        };
        var expectedAppEvent = new AppEvent(new ComposingEvent());

        _event.ProcessingMode.Should().Be(ProcessingMode.Conversation);
        _event.ChannelIdentity.Should().BeEquivalentTo(expectedChannelIdentity);
        _event.AppEvent.Should().BeEquivalentTo(expectedAppEvent);
    }

    [When(@"I send a request to delete a conversation event")]
    public async Task WhenISendARequestToDeleteAConversationEvent()
    {
        await _events.Delete("01W4FFL35P4NC4K35CONVEVENT1");
        _deleteCompleted = true;
    }

    [Then(@"the delete conversation event response contains no data")]
    public void ThenTheDeleteConversationEventResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }
}
