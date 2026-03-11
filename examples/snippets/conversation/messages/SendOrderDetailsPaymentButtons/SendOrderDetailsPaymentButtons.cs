using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Sinch.Conversation.Messages.Send;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// The ID of the Conversation Application to send the message from
const string conversationAppId = "CONVERSATION_APP_ID";
// The recipient's WhatsApp phone number
const string whatsAppPhoneNumber = "RECIPIENT_CONTACT_ID";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        ConversationRegion = new ConversationRegion(conversationRegion)
    }
});

var appMessage = new AppMessage(new TextMessage("Please complete your payment."))
{
    ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>
    {
        [ConversationChannel.WhatsApp] = new OrderDetailsPaymentMessage
        {
            Message = new OrderDetails
            {
                Payment = new OrderDetailsPayment
                {
                    Type = OrderDetailsPayment.TypeEnum.Br,
                    ReferenceId = "order-ref-001",
                    TypeOfGoods = TypeOfGoods.DigitalGoods,
                    TotalAmountValue = 1200,
                    PaymentButtons =
                    [
                        new WhatsAppPaymentSettingsButtonPaymentLink
                        {
                            Uri = "https://www.my-payment-link.com"
                        }
                    ],
                    Order = new OrderDetailsPaymentOrder
                    {
                        SubtotalValue = 1000,
                        TaxValue = 200,
                        Items =
                        [
                            new OrderDetailsPaymentOrderItems
                            {
                                RetailerId = "sku-001",
                                Name = "My Product",
                                AmountValue = 1000,
                                Quantity = 1
                            }
                        ]
                    }
                }
            }
        }
    }
};

var request = new SendMessageRequest
{
    AppId = conversationAppId,
    Recipient = new Identified
    {
        IdentifiedBy = new IdentifiedBy
        {
            ChannelIdentities =
            [
                new ChannelIdentity
                {
                    Channel = ConversationChannel.WhatsApp,
                    Identity = whatsAppPhoneNumber
                }
            ]
        }
    },
    Message = appMessage
};

Console.WriteLine($"Sending payment message to '{whatsAppPhoneNumber}'");

var response = await client.Conversation.Messages.Send(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
