using Sinch;
using Sinch.Conversation.Messages.Message;

namespace Examples
{
    public class AccessingPolymorphicType
    {
        public static void Example()
        {
            var sinchClient = new SinchClient("KEY_ID", "KEY_SECRET", "PROJECT_ID");
            var message = sinchClient.Conversation.Messages.Get("1").Result;
            var messageType = message.AppMessage.Message switch
            {
                CardMessage cardMessage => "card",
                CarouselMessage carouselMessage => "carousel",
                ChoiceMessage choiceMessage => "choice",
                ListMessage listMessage => "list",
                LocationMessage locationMessage => "location",
                MediaMessage mediaMessage => "media",
                TemplateMessage templateMessage => "template",
                TextMessage textMessage => "text",
                _ => "none"
            };
            Console.WriteLine(messageType);
        }
    }
}
