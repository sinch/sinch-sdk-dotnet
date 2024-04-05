using System.Collections.Generic;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class WebhooksTests : ConversationTestBase
    {
        [Fact]
        public void ValidateRequest()
        {
            const string str =
                "{\"app_id\":\"\",\"accepted_time\":\"2021-10-18T17:49:13.813615Z\",\"project_id\":\"e2df3a34-a71b-4448-9db5-a8d2baad28e4\",\"contact_create_notification\":{\"contact\":{\"id\":\"01FJA8B466Y0R2GNXD78MD9SM1\",\"channel_identities\":[{\"channel\":\"SMS\",\"identity\":\"48123456789\",\"app_id\":\"\"}],\"display_name\":\"New Test Contact\",\"email\":\"new.contact@email.com\",\"external_id\":\"\",\"metadata\":\"\",\"language\":\"EN_US\"}},\"message_metadata\":\"\"}";

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, StringValues>()
            {
                { "x-sinch-webhook-signature-nonce", new[] { "01FJA8B4A7BM43YGWSG9GBV067" } },
                { "x-sinch-webhook-signature-timestamp", new[] { "1634579353" } },
                { "x-sinch-webhook-signature", new[] { "6bpJoRmFoXVjfJIVglMoJzYXxnoxRujzR4k2GOXewOE=" } },
            }, JsonNode.Parse(str)!.AsObject(), "foo_secret1234");
            isValid.Should().BeTrue();
        }
    }
}
