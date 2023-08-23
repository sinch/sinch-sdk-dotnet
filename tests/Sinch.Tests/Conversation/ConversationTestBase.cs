using System;
using Sinch.Conversation;

namespace Sinch.Tests.Conversation
{
    public class ConversationTestBase : TestBase
    {
        internal readonly IConversation Conversation;

        protected ConversationTestBase()
        {
            Conversation = new Sinch.Conversation.Conversation(ProjectId, new Uri("https://us.conversation.api.sinch.com"),
                default, HttpSnakeCase);
        }
    }
}
