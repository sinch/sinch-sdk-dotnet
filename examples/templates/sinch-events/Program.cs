// Sinch Events Template
// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet
// See https://github.com/sinch/sinch-sdk-dotnet/tree/main/examples/templates/sinch-events/README.md for details

using Sinch;
using Sinch.Verification;
using SinchEvents.Template.Numbers;
using SinchEvents.Template.Sms;
using SinchEvents.Template.Verification;
using SinchEvents.Template.Voice;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<SmsServerBusinessLogic>();
builder.Services.AddSingleton<NumbersServerBusinessLogic>();
builder.Services.AddSingleton<VerificationServerBusinessLogic>();
builder.Services.AddSingleton<VoiceServerBusinessLogic>();

builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
       VerificationConfiguration = new SinchVerificationConfiguration
       {
              AppKey = builder.Configuration["Sinch:Verification:AppKey"] ?? string.Empty,
              AppSecret = builder.Configuration["Sinch:Verification:AppSecret"] ?? string.Empty
       }
});

builder.Services.AddSinchEventsHandlers(ServiceLifetime.Scoped);

var app = builder.Build();

app.MapControllers();

app.Run();

