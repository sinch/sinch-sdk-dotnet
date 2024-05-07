using System;
using Sinch.Conversation;

namespace Sinch.Tests.Conversation
{
    public class ConversationTestBase : TestBase
    {
        internal readonly ISinchConversation Conversation;

        protected ConversationTestBase()
        {
            Conversation = new SinchConversationClient(ProjectId,
                new Uri("https://us.conversation.api.sinch.com"), new Uri("https://us.template.api.sinch.com"),
                default, HttpSnakeCase);
        }
    }
}
