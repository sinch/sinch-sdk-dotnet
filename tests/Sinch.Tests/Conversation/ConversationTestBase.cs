using System;
using System.Text.Json;
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
                null, HttpSnakeCase);
        }

        /// <summary>
        ///     Deserialize Json string using default plain JsonSerializer without options
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        ///      Deserialize Json string using the same rules as Conversation json serializer uses
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T DeserializeAsConversationClient<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Conversation.JsonSerializerOptions);
        }
    }
}
