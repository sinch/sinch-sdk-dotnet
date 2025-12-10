using System.Text.Json;
using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.VoiceConfigurations;

// Example 1: VoiceRtcConfiguration - Real-Time Communication
var rtcConfiguration = new VoiceRtcConfiguration
{
    AppId = "your-voice-app-id"
};

// Example 2: VoiceEstConfiguration - Elastic SIP Trunking
var estConfiguration = new VoiceEstConfiguration
{
    TrunkId = "your-trunk-id"
};

// Example 3: VoiceFaxConfiguration - Fax Service
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
