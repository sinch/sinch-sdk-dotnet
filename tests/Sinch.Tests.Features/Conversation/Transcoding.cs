using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Transcoding
{
    private ISinchConversationTranscoding _transcoding;
    private TranscodeResponse _transcodeResponse;

    [Given(@"the Conversation service ""Transcoding"" is available")]
    public void GivenTheConversationServiceTranscodingIsAvailable()
    {
        _transcoding = Utils.SinchConversationClient().Transcoding;
    }

    [When(@"I send a request to transcode a location message")]
    public async Task WhenISendARequestToTranscodeALocationMessage()
    {
        _transcodeResponse = await _transcoding.Transcode(new TranscodeRequest
        {
            AppId = "01W4FFL35P4NC4K35CONVAPP001",
            AppMessage = new AppMessage(new LocationMessage
            {
                Title = "Phare d'Eckmühl",
                Label = "Pointe de Penmarch",
                Coordinates = new Coordinates(47.7981899, -4.3727685)
            }),
            Channels = new List<ConversationChannel>
            {
                ConversationChannel.AppleBC,
                ConversationChannel.Instagram,
                ConversationChannel.KakaoTalk,
                ConversationChannel.KakaoTalkChat,
                ConversationChannel.Line,
                ConversationChannel.Messenger,
                ConversationChannel.Rcs,
                ConversationChannel.Sms,
                ConversationChannel.Telegram,
                ConversationChannel.Viber,
                ConversationChannel.WeChat,
                ConversationChannel.WhatsApp
            }
        });
    }

    [Then(@"the location message is transcoded for all the channels")]
    public void ThenTheLocationMessageIsTranscodedForAllTheChannels()
    {
        _transcodeResponse.TranscodedMessage.Should().NotBeNull();
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.AppleBC);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Instagram);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.KakaoTalk);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.KakaoTalkChat);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Line);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Messenger);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Rcs);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Sms);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Telegram);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.Viber);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.WeChat);
        _transcodeResponse.TranscodedMessage.Should().ContainKey(ConversationChannel.WhatsApp);

        var whatsAppJson = _transcodeResponse.TranscodedMessage![ConversationChannel.WhatsApp];
        var whatsAppMessage = JsonSerializer.Deserialize<JsonElement>(whatsAppJson);
        whatsAppMessage.GetProperty("to").GetString().Should().Be("{{to}}");
        whatsAppMessage.GetProperty("type").GetString().Should().Be("location");
        whatsAppMessage.GetProperty("recipient_type").GetString().Should().Be("individual");
        whatsAppMessage.GetProperty("messaging_product").GetString().Should().Be("whatsapp");

        var location = whatsAppMessage.GetProperty("location");
        location.GetProperty("name").GetString().Should().Be("Phare d'Eckmühl");
        location.GetProperty("address").GetString().Should().Be("Pointe de Penmarch");
        location.GetProperty("longitude").GetString().Should().Be("-4.3727684");
        location.GetProperty("latitude").GetString().Should().Be("47.79819");
    }
}
