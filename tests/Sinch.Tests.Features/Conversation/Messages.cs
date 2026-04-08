using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Sinch.Conversation.Messages.Send;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Messages
{
    private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";
    private const string ContactId = "01W4FFL35P4NC4K35CONTACT001";
    private const string MessageId001 = "01W4FFL35P4NC4K35MESSAGE001";

    private ISinchConversationMessages _messages;
    private SendMessageResponse _sendResponse;
    private ListMessagesResponse _listResponse;
    private List<ConversationMessage> _allMessages;
    private int _totalPages;
    private ConversationMessage _message;
    private bool _deleteCompleted;

    [Given(@"the Conversation service ""Messages"" is available")]
    public void GivenTheConversationServiceMessagesIsAvailable()
    {
        _messages = Utils.SinchConversationClient().Messages;
    }

    [When(@"I send a request to send a message to a contact")]
    public async Task WhenISendARequestToSendAMessageToAContact()
    {
        _sendResponse = await _messages.Send(new SendMessageRequest
        {
            AppId = AppId,
            Recipient = new ContactRecipient { ContactId = ContactId },
            Message = new AppMessage(new TextMessage("Hello"))
        });
    }

    [Then(@"the response contains the id of the message")]
    public void ThenTheResponseContainsTheIdOfTheMessage()
    {
        _sendResponse.Should().NotBeNull();
        _sendResponse.MessageId.Should().Be(MessageId001);
    }

    [When(@"I send a request to list the existing messages")]
    public async Task WhenISendARequestToListTheExistingMessages()
    {
        _listResponse = await _messages.List(new ListMessagesRequest { PageSize = 2 });
    }

    [Then(@"the response contains ""(.*)"" messages")]
    public void ThenTheResponseContainsMessages(int count)
    {
        _listResponse.Messages.Should().HaveCount(count);
        _listResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the messages")]
    public async Task WhenISendARequestToListAllTheMessages()
    {
        _allMessages = new List<ConversationMessage>();
        await foreach (var message in _messages.ListAuto(new ListMessagesRequest { PageSize = 2 }))
        {
            _allMessages.Add(message);
        }
    }

    [Then(@"the messages list contains ""(.*)"" messages")]
    public void ThenTheMessagesListContainsMessages(int count)
    {
        _allMessages.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the messages pages")]
    public async Task WhenIIterateManuallyOverTheMessagesPages()
    {
        _allMessages = new List<ConversationMessage>();
        _totalPages = 0;
        ListMessagesResponse response = null;
        do
        {
            response = await _messages.List(new ListMessagesRequest
            {
                PageSize = 2,
                PageToken = response?.NextPageToken
            });
            if (response.Messages != null)
                _allMessages.AddRange(response.Messages);
            _totalPages++;
        } while (!string.IsNullOrEmpty(response.NextPageToken));
    }

    [Then(@"the result contains the data from ""(.*)"" pages")]
    public void ThenTheResultContainsTheDataFromPages(int count)
    {
        _totalPages.Should().Be(count);
    }

    [When(@"I send a request to retrieve a message")]
    public async Task WhenISendARequestToRetrieveAMessage()
    {
        _message = await _messages.Get(MessageId001);
    }

    [Then(@"the response contains the message details")]
    public void ThenTheResponseContainsTheMessageDetails()
    {
        _message.Should().NotBeNull();
        _message.Id.Should().Be(MessageId001);
        _message.Direction.Should().Be(ConversationDirection.ToContact);
        _message.AppMessage.Should().NotBeNull();
        _message.AppMessage!.TextMessage!.Text.Should().Be("Hello");
        _message.ChannelIdentity!.Channel.Should().Be(ConversationChannel.Sms);
        _message.ChannelIdentity.Identity.Should().Be("12015555555");
        _message.ContactId.Should().Be(ContactId);
    }

    [When(@"I send a request to update a message")]
    public async Task WhenISendARequestToUpdateAMessage()
    {
        _message = await _messages.Update(MessageId001, "Updated metadata");
    }

    [Then(@"the response contains the message details with updated metadata")]
    public void ThenTheResponseContainsTheMessageDetailsWithUpdatedMetadata()
    {
        _message.Should().NotBeNull();
        _message.Id.Should().Be(MessageId001);
        _message.Metadata.Should().Be("Updated metadata");
    }

    [When(@"I send a request to delete a message")]
    public async Task WhenISendARequestToDeleteAMessage()
    {
        await _messages.Delete(MessageId001);
        _deleteCompleted = true;
    }

    [Then(@"the delete message response contains no data")]
    public void ThenTheDeleteMessageResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }

    [When(@"I send a request to list the last messages sent to specified channel identities")]
    public async Task WhenISendARequestToListTheLastMessagesSentToSpecifiedChannelIdentities()
    {
        _listResponse = await _messages.ListLastMessagesByChannelIdentity(
            new ListMessagesByChannelIdentityRequest
            {
                ChannelIdentities = new List<string> { "12015555555", "12017777777", "7504610123456789" },
                MessagesSource = MessageSource.ConversationSource,
                PageSize = 2
            });
    }

    [Then(@"the response contains ""(.*)"" last messages sent to specified channel identities")]
    public void ThenTheResponseContainsLastMessages(int count)
    {
        _listResponse.Messages.Should().HaveCount(count);
        _listResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the last messages sent to specified channel identities")]
    public async Task WhenISendARequestToListAllTheLastMessagesSentToSpecifiedChannelIdentities()
    {
        _allMessages = new List<ConversationMessage>();
        await foreach (var message in _messages.ListLastMessagesByChannelIdentityAuto(
                           new ListMessagesByChannelIdentityRequest
                           {
                               ChannelIdentities = new List<string> { "12015555555", "12017777777", "7504610123456789" },
                               MessagesSource = MessageSource.ConversationSource,
                               PageSize = 2
                           }))
        {
            _allMessages.Add(message);
        }
    }

    [Then(@"the response list contains ""(.*)"" last messages sent to specified channel identities")]
    public void ThenTheResponseListContainsLastMessages(int count)
    {
        _allMessages.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the last messages sent to specified channel identities pages")]
    public async Task WhenIIterateManuallyOverTheLastMessagesSentToSpecifiedChannelIdentitiesPages()
    {
        _allMessages = new List<ConversationMessage>();
        _totalPages = 0;
        ListMessagesResponse response = null;
        var request = new ListMessagesByChannelIdentityRequest
        {
            ChannelIdentities = new List<string> { "12015555555", "12017777777", "7504610123456789" },
            MessagesSource = MessageSource.ConversationSource,
            PageSize = 2
        };
        do
        {
            response = await _messages.ListLastMessagesByChannelIdentity(request);
            request.PageToken = response.NextPageToken;
            if (response.Messages != null)
                _allMessages.AddRange(response.Messages);
            _totalPages++;
        } while (!string.IsNullOrEmpty(response.NextPageToken));
    }

    [Then(@"the result contains the data from ""(.*)"" pages of last messages sent to specified channel identities")]
    public void ThenTheResultContainsTheDataFromPagesOfLastMessages(int count)
    {
        _totalPages.Should().Be(count);
    }

    [When(@"I send a request to send an order details payment buttons message")]
    public async Task WhenISendARequestToSendAnOrderDetailsPaymentButtonsMessage()
    {
        _sendResponse = await _messages.Send(new SendMessageRequest
        {
            AppId = AppId,
            Recipient = new ContactRecipient { ContactId = ContactId },
            Message = new AppMessage(new TextMessage("Please complete your payment."))
            {
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>
                {
                    [ConversationChannel.WhatsApp] = new OrderDetailsPaymentMessage
                    {
                        Message = new OrderDetails
                        {
                            Body = new WhatsAppInteractiveBody { Text = "Here is your order summary." },
                            Payment = new OrderDetailsPayment
                            {
                                Type = OrderDetailsPayment.TypeEnum.Br,
                                TypeOfGoods = TypeOfGoods.DigitalGoods,
                                ReferenceId = "order-ref-001",
                                TotalAmountValue = 1200,
                                PaymentButtons = new List<IWhatsAppPaymentButton>
                                {
                                    new WhatsAppPaymentSettingsButtonPix
                                    {
                                        Code = "MY_PIX_DYNAMIC_CODE",
                                        MerchantName = "My Store",
                                        Key = "MY_PIX_KEY",
                                        KeyType = WhatsAppPaymentSettingsButtonPix.PixKeyType.Cpf
                                    }
                                },
                                Order = new OrderDetailsPaymentOrder
                                {
                                    SubtotalValue = 1200,
                                    TaxValue = 0,
                                    Items = new List<OrderDetailsPaymentOrderItems>
                                    {
                                        new()
                                        {
                                            RetailerId = "sku-001",
                                            Name = "My Product",
                                            AmountValue = 1200,
                                            Quantity = 1
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    [Then(@"the response contains the id of the payment buttons message")]
    public void ThenTheResponseContainsTheIdOfThePaymentButtonsMessage()
    {
        _sendResponse.Should().NotBeNull();
        _sendResponse.MessageId.Should().Be("01W4FFL35P4NC4K35PAYBTN001");
    }
}
