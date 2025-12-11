using System.Text.Json;
using Sinch.Numbers;
using Sinch.Numbers.VoiceConfigurations;

var rtcProvisioning = new ScheduledVoiceRtcProvisioning
{
    AppId = "your-voice-app-id",
    Status = ProvisioningStatus.Waiting,
    LastUpdatedTime = DateTime.UtcNow
};

var estProvisioning = new ScheduledVoiceEstProvisioning
{
    TrunkId = "your-trunk-id"
    Status = ProvisioningStatus.InProgress,
    LastUpdatedTime = DateTime.UtcNow
};

var faxProvisioning = new ScheduledVoiceFaxProvisioning
{
    ServiceId = "your-fax-service-id",
    Status = ProvisioningStatus.Waiting,
    LastUpdatedTime = DateTime.UtcNow
};

Console.WriteLine("RTC Scheduled Provisioning:");
Console.WriteLine(JsonSerializer.Serialize(rtcProvisioning, new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine("\nEST Scheduled Provisioning:");
Console.WriteLine(JsonSerializer.Serialize(estProvisioning, new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine("\nFAX Scheduled Provisioning:");
Console.WriteLine(JsonSerializer.Serialize(faxProvisioning, new JsonSerializerOptions
{
    WriteIndented = true
}));
