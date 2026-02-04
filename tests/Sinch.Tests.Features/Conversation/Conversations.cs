using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Nodes;
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

namespace Sinch.Tests.Features.Conversation
{
    [Binding]
    public class Conversations
    {
        private const string ConversationId = "01W4FFL35P4NC4K35CONVERS001";
        private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
        private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
        private const string ContactId001 = "01W4FFL35P4NC4K35CONTACT001";
        private const string ContactId002 = "01W4FFL35P4NC4K35CONTACT002";
        private const string InjectEventId = "01W4FFL35P4NC4K35CONVEVENT1";

        private ISinchConversationConversations _conversations;
        private Sinch.Conversation.Conversations.Conversation _conversation;
        private IEnumerable<Sinch.Conversation.Conversations.Conversation> _conversationsPage;
        private List<Sinch.Conversation.Conversations.Conversation> _conversationsList;
        private int _pagesIteration;
        private bool _deleteCompleted;
        private bool _injectMessageCompleted;
        private bool _stopConversationCompleted;
        private InjectEventResponse _injectEventResponse;

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
                Metadata = "e2e tests",
                MetadataJson = new JsonObject
                {
                    ["prop1"] = "value1",
                    ["prop2"] = "value2"
                }
            });
        }

        [Then(@"the conversation is created")]
        public void ThenTheConversationIsCreated()
        {
            _conversation.Id.Should().Be(ConversationId);
        }

        [When(@"I send a request to list the existing conversations")]
        public async Task WhenISendARequestToListTheExistingConversations()
        {
            var response = await _conversations.List(new ListConversationsRequest
            {
                AppId = AppId001,
                PageSize = 2,
                OnlyActive = false
            });
            _conversationsPage = response.Conversations ?? [];
        }

        [Then(@"the response contains ""{int}"" conversations")]
        public void ThenTheResponseContainsConversations(int expectedCount)
        {
            _conversationsPage.Count().Should().Be(expectedCount);
        }

        [When(@"I send a request to list all the conversations")]
        public async Task WhenISendARequestToListAllTheConversations()
        {
            _conversationsList = [];
            await foreach (var conversation in _conversations.ListAuto(new ListConversationsRequest
            {
                AppId = AppId001,
                PageSize = 2,
                OnlyActive = false
            }))
            {
                _conversationsList.Add(conversation);
            }
        }

        [When(@"I iterate manually over the conversations pages")]
        public async Task WhenIIterateManuallyOverTheConversationsPages()
        {
            _conversationsList = [];
            var response = await _conversations.List(new ListConversationsRequest
            {
                AppId = AppId001,
                PageSize = 2,
                OnlyActive = false
            });
            _conversationsList.AddRange(response.Conversations ?? []);
            _pagesIteration = 1;

            while (!string.IsNullOrEmpty(response.NextPageToken))
            {
                response = await _conversations.List(new ListConversationsRequest
                {
                    AppId = AppId001,
                    PageSize = 2,
                    OnlyActive = false,
                    PageToken = response.NextPageToken
                });
                _conversationsList.AddRange(response.Conversations ?? []);
                _pagesIteration++;
            }
        }

        [Then(@"the conversations list contains ""{int}"" conversations")]
        public void ThenTheConversationsListContainsConversations(int expectedCount)
        {
            _conversationsList.Count().Should().Be(expectedCount);
        }

        [Then(@"the conversations iteration result contains the data from ""{int}"" pages")]
        public void ThenTheConversationsIterationResultContainsDataFromPages(int expectedPages)
        {
            _pagesIteration.Should().Be(expectedPages);
        }

        [When(@"I send a request to list the recent conversations")]
        public void WhenISendARequestToListTheRecentConversations()
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [Then(@"the response contains ""{int}"" recent conversations")]
        public void ThenTheResponseContainsRecentConversations(int expectedCount)
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [When(@"I send a request to list all the recent conversations")]
        public void WhenISendARequestToListAllTheRecentConversations()
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [When(@"I iterate manually over the recent conversations pages")]
        public void WhenIIterateManuallyOverTheRecentConversationsPages()
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [Then(@"the recent conversations list contains ""{int}"" recent conversations")]
        public void ThenTheRecentConversationsListContainsRecentConversations(int expectedCount)
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [Then(@"the recent conversations iteration result contains the data from ""{int}"" pages")]
        public void ThenTheRecentConversationsIterationResultContainsDataFromPages(int expectedPages)
        {
            // TODO: ListRecent is not supported in the .NET SDK.
        }

        [When(@"I send a request to retrieve a conversation")]
        public async Task WhenISendARequestToRetrieveAConversation()
        {
            _conversation = await _conversations.Get(ConversationId);
        }

        [Then(@"the response contains the conversation details")]
        public void ThenTheResponseContainsTheConversationDetails()
        {
            _conversation.Id.Should().Be(ConversationId);
            _conversation.AppId.Should().Be(AppId001);
            _conversation.ContactId.Should().Be(ContactId002);
            _conversation.LastReceived.Should().Be(LastReceivedTime);
            _conversation.ActiveChannel.Should().Be(ConversationChannel.Messenger);
            _conversation.Active.Should().BeTrue();
            _conversation.Metadata.Should().Be("e2e tests");
            var expectedMetadata = JsonNode.Parse("{\"prop1\":\"value1\",\"prop2\":\"value2\"}");
            JsonNode.DeepEquals(_conversation.MetadataJson, expectedMetadata).Should().BeTrue();
            _conversation.CorrelationId.Should().BeEmpty();
        }

        [When(@"I send a request to update a conversation")]
        public async Task WhenISendARequestToUpdateAConversation()
        {
            _conversation = await _conversations.Update(new Sinch.Conversation.Conversations.Conversation
            {
                Id = ConversationId,
                Active = false,
                AppId = AppId002,
                Metadata = "Transferred conversation",
                CorrelationId = "my-correlator"
            }, MetadataUpdateStrategy.Replace);
        }

        [Then(@"the response contains the conversation details with updated data")]
        public void ThenTheResponseContainsTheConversationDetailsWithUpdatedData()
        {
            _conversation.Id.Should().Be(ConversationId);
            _conversation.AppId.Should().Be(AppId002);
            _conversation.Active.Should().BeFalse();
            _conversation.Metadata.Should().Be("Transferred conversation");
            _conversation.CorrelationId.Should().Be("my-correlator");
        }

        [When(@"I send a request to delete a conversation")]
        public async Task WhenISendARequestToDeleteAConversation()
        {
            await _conversations.Delete(ConversationId);
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
            _injectEventResponse = await _conversations.InjectEvent(new InjectEventRequest(
                new AppEvent(new ComposingEvent()))
                {
                    ConversationId = ConversationId,
                    AcceptTime = InjectEventAcceptedTime
                });
        }

        [Then(@"the event is created and injected in the conversation")]
        public void ThenTheEventIsCreatedAndInjectedInTheConversation()
        {
            _injectEventResponse.EventId.Should().Be(InjectEventId);
            _injectEventResponse.AcceptedTime.Should().Be(InjectEventAcceptedTime);
        }

        [When(@"I send a request to inject a message into a conversation")]
        public async Task WhenISendARequestToInjectAMessageIntoAConversation()
        {
            await _conversations.InjectMessage(new InjectMessageRequest
            {
                ConversationId = ConversationId,
                AcceptTime = LastReceivedTime,
                Direction = ConversationDirection.ToContact,
                ContactId = ContactId002,
                ChannelIdentity = new ChannelIdentity
                {
                    Channel = ConversationChannel.Messenger,
                    Identity = "7968425018576406",
                    AppId = AppId001
                },
                AppMessage = new AppMessage(new TextMessage("Injected text message"))
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
            await _conversations.Stop(ConversationId);
            _stopConversationCompleted = true;
        }

        [Then(@"the stop conversation response contains no data")]
        public void ThenTheStopConversationResponseContainsNoData()
        {
            _stopConversationCompleted.Should().BeTrue();
        }

        private static readonly DateTime LastReceivedTime = DateTime.Parse(
            "2024-06-06T14:42:42Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private static readonly DateTime InjectEventAcceptedTime = DateTime.Parse(
            "2024-06-06T15:15:15Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }
}
