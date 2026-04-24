// Sinch SMS Sinch Events Template
// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet
// See https://github.com/sinch/sinch-sdk-dotnet/tree/main/examples/templates/webhooks/README.md for details

using Sinch;
using Sinch.SMS.SinchEvents;
using SinchEvents.Template.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<ServerBusinessLogic>();

// Note: Unified credentials are not required for sinch event validation (only Sinch:Sms:SinchEventSecret is).
// They are included here so you can respond to incoming messages using the SMS API if needed.
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
});

builder.Services.AddSingleton<ISinchSmsSinchEvents>(sp => sp.GetRequiredService<ISinchClient>().Sms.SinchEvents);

var app = builder.Build();

app.MapControllers();

app.Run();

