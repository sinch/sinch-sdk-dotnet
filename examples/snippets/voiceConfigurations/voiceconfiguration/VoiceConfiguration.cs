using System.Text.Json;
using Sinch.Numbers.VoiceConfigurations;

var rtcConfiguration = new VoiceRtcConfiguration
{
    AppId = "your-voice-app-id"
};

var estConfiguration = new VoiceEstConfiguration
{
    TrunkId = "your-trunk-id"
};

var faxConfiguration = new VoiceFaxConfiguration
{
    ServiceId = "your-fax-service-id"
};

Console.WriteLine("RTC Configuration:");
Console.WriteLine(JsonSerializer.Serialize(rtcConfiguration, new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine("\nEST Configuration:");
Console.WriteLine(JsonSerializer.Serialize(estConfiguration, new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine("\nFAX Configuration:");
Console.WriteLine(JsonSerializer.Serialize(faxConfiguration, new JsonSerializerOptions
{
    WriteIndented = true
}));
