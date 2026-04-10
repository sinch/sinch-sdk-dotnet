using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Conversations;
using Sinch.Conversation.Conversations.Create;
using Sinch.Conversation.Conversations.InjectEvent;
using Sinch.Conversation.Conversations.InjectMessage;
using Sinch.Conversation.Conversations.List;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Messages.Message;
using ConversationEntity = Sinch.Conversation.Conversations.Conversation;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Conversations
{
    private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
    private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
    private const string ContactId001 = "01W4FFL35P4NC4K35CONTACT001";
    private const string ContactId002 = "01W4FFL35P4NC4K35CONTACT002";
    private const string ConversationId001 = "01W4FFL35P4NC4K35CONVERS001";
    private const string EventId001 = "01W4FFL35P4NC4K35CONVEVENT1";

    private ISinchConversationConversations _conversations;
    private ConversationEntity _conversation;
    private ListConversationsResponse _listResponse;
    private List<ConversationEntity> _allConversations;
    private int _totalConversationPages;
    private ListRecentConversationsResponse _listRecentResponse;
    private List<ConversationRecentMessage> _allRecentConversations;
    private int _totalRecentConversationPages;
    private InjectEventResponse _injectEventResponse;
    private bool _deleteCompleted;
    private bool _injectMessageCompleted;
    private bool _stopCompleted;

    [Given(@"the Conversation service ""Conversations"" is available")]
    public void GivenTheConversationServiceConversationsIsAvailable()
    {
        _conversations = Utils.SinchConversationClient().Conversations;
    }

    [When(@"I send a request to create a conversation")]
    public async Task WhenISendARequestToCreateAConversation()
    {
        _conversation = await _conversations.Create(new CreateConversationRequest
        {
            AppId = AppId001,
            ContactId = ContactId001,
            Active = true,
            ActiveChannel = ConversationChannel.Messenger,
            MetadataJson = new System.Text.Json.Nodes.JsonObject
            {
                ["prop1"] = "value1",
                ["prop2"] = "value2"
            }
        });
    }

    [Then(@"the conversation is created")]
    public void ThenTheConversationIsCreated()
    {
        _conversation.Should().NotBeNull();
        _conversation.Id.Should().Be(ConversationId001);
        _conversation.AppId.Should().Be(AppId001);
        _conversation.ContactId.Should().Be(ContactId001);
        _conversation.Active.Should().BeTrue();
    }

    [When(@"I send a request to list the existing conversations")]
    public async Task WhenISendARequestToListTheExistingConversations()
    {
        _listResponse = await _conversations.List(new ListConversationsRequest
        {
            OnlyActive = false,
            AppId = AppId001,
            PageSize = 2
        });
    }

    [Then(@"the response contains ""(.*)"" conversations")]
    public void ThenTheResponseContainsConversations(int count)
    {
        _listResponse.Conversations.Should().HaveCount(count);
        _listResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the conversations")]
    public async Task WhenISendARequestToListAllTheConversations()
    {
        _allConversations = new List<ConversationEntity>();
        await foreach (var conv in _conversations.ListAuto(new ListConversationsRequest
        {
            OnlyActive = false,
            AppId = AppId001,
            PageSize = 2
        }))
        {
            _allConversations.Add(conv);
        }
    }

    [Then(@"the conversations list contains ""(.*)"" conversations")]
    public void ThenTheConversationsListContainsConversations(int count)
    {
        _allConversations.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the conversations pages")]
    public async Task WhenIIterateManuallyOverTheConversationsPages()
    {
        _allConversations = new List<ConversationEntity>();
        _totalConversationPages = 0;
        ListConversationsResponse response = null;
        do
        {
            response = await _conversations.List(new ListConversationsRequest
            {
                OnlyActive = false,
                AppId = AppId001,
                PageSize = 2,
                PageToken = response?.NextPageToken
            });
            if (response.Conversations != null)
                _allConversations.AddRange(response.Conversations);
            _totalConversationPages++;
        } while (!string.IsNullOrEmpty(response.NextPageToken));
    }

    [Then(@"the conversations iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheConversationsIterationResultContainsTheDataFromPages(int count)
    {
        _totalConversationPages.Should().Be(count);
    }

    [When(@"I send a request to list the recent conversations")]
    public async Task WhenISendARequestToListTheRecentConversations()
    {
        _listRecentResponse = await _conversations.ListRecent(new ListRecentConversationsRequest
        {
            AppId = AppId001,
            PageSize = 2
        });
    }

    [Then(@"the response contains ""(.*)"" recent conversations")]
    public void ThenTheResponseContainsRecentConversations(int count)
    {
        _listRecentResponse.Conversations.Should().HaveCount(count);
        _listRecentResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the recent conversations")]
    public async Task WhenISendARequestToListAllTheRecentConversations()
    {
        _allRecentConversations = new List<ConversationRecentMessage>();
        await foreach (var item in _conversations.ListRecentAuto(new ListRecentConversationsRequest
        {
            AppId = AppId001,
            PageSize = 2
        }))
        {
            _allRecentConversations.Add(item);
        }
    }

    [Then(@"the recent conversations list contains ""(.*)"" recent conversations")]
    public void ThenTheRecentConversationsListContainsRecentConversations(int count)
    {
        _allRecentConversations.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the recent conversations pages")]
    public async Task WhenIIterateManuallyOverTheRecentConversationsPages()
    {
        _allRecentConversations = new List<ConversationRecentMessage>();
        _totalRecentConversationPages = 0;
        ListRecentConversationsResponse response = null;
        do
        {
            response = await _conversations.ListRecent(new ListRecentConversationsRequest
            {
                AppId = AppId001,
                PageSize = 2,
                PageToken = response?.NextPageToken
            });
            if (response.Conversations != null)
                _allRecentConversations.AddRange(response.Conversations);
            _totalRecentConversationPages++;
        } while (!string.IsNullOrEmpty(response.NextPageToken));
    }

    [Then(@"the recent conversations iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheRecentConversationsIterationResultContainsTheDataFromPages(int count)
    {
        _totalRecentConversationPages.Should().Be(count);
    }

    [When(@"I send a request to retrieve a conversation")]
    public async Task WhenISendARequestToRetrieveAConversation()
    {
        _conversation = await _conversations.Get(ConversationId001);
    }

    [Then(@"the response contains the conversation details")]
    public void ThenTheResponseContainsTheConversationDetails()
    {
        _conversation.Should().NotBeNull();
        _conversation.Id.Should().Be(ConversationId001);
        _conversation.AppId.Should().Be(AppId001);
        _conversation.ContactId.Should().Be(ContactId002);
        _conversation.ActiveChannel.Should().Be(ConversationChannel.Messenger);
        _conversation.Active.Should().BeTrue();
        _conversation.Metadata.Should().Be("e2e tests");
    }

    [When(@"I send a request to update a conversation")]
    public async Task WhenISendARequestToUpdateAConversation()
    {
        _conversation = await _conversations.Update(new ConversationEntity
        {
            Id = ConversationId001,
            Active = false,
            AppId = AppId002,
            CorrelationId = "my-correlator"
        });
    }

    [Then(@"the response contains the conversation details with updated data")]
    public void ThenTheResponseContainsTheConversationDetailsWithUpdatedData()
    {
        _conversation.Should().NotBeNull();
        _conversation.Id.Should().Be(ConversationId001);
        _conversation.AppId.Should().Be(AppId002);
        _conversation.Active.Should().BeFalse();
        _conversation.Metadata.Should().Be("Transferred conversation");
        _conversation.CorrelationId.Should().Be("my-correlator");
    }

    [When(@"I send a request to delete a conversation")]
    public async Task WhenISendARequestToDeleteAConversation()
    {
        await _conversations.Delete(ConversationId001);
        _deleteCompleted = true;
    }

    [Then(@"the delete conversation response contains no data")]
    public void ThenTheDeleteConversationResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }

    [When(@"I send a request to inject an event into a conversation")]
    public async Task WhenISendARequestToInjectAnEventIntoAConversation()
    {
        _injectEventResponse = await _conversations.InjectEvent(new InjectEventRequest(new AppEvent(new ComposingEvent()))
        {
            ConversationId = ConversationId001,
            AcceptTime = DateTime.UtcNow
        });
    }

    [Then(@"the event is created and injected in the conversation")]
    public void ThenTheEventIsCreatedAndInjectedInTheConversation()
    {
        _injectEventResponse.Should().NotBeNull();
        _injectEventResponse.EventId.Should().Be(EventId001);
    }

    [When(@"I send a request to inject a message into a conversation")]
    public async Task WhenISendARequestToInjectAMessageIntoAConversation()
    {
        var appMessage = new AppMessage(new TextMessage("Injected text message"));

        await _conversations.InjectMessage(new InjectMessageRequest(appMessage)
        {
            ConversationId = ConversationId001,
            ContactId = ContactId002,
            ChannelIdentity = new ChannelIdentity
            {
                Channel = ConversationChannel.Messenger,
                Identity = "7968425018576406",
                AppId = AppId001
            },
            Direction = ConversationDirection.ToContact,
            AcceptTime = DateTime.UtcNow
        });
        _injectMessageCompleted = true;
    }

    [Then(@"the message is created and injected in the conversation")]
    public void ThenTheMessageIsCreatedAndInjectedInTheConversation()
    {
        _injectMessageCompleted.Should().BeTrue();
    }

    [When(@"I send a request to stop a conversation")]
    public async Task WhenISendARequestToStopAConversation()
    {
        await _conversations.Stop(ConversationId001);
        _stopCompleted = true;
    }

    [Then(@"the stop conversation response contains no data")]
    public void ThenTheStopConversationResponseContainsNoData()
    {
        _stopCompleted.Should().BeTrue();
    }
}
