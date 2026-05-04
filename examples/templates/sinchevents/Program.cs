// Sinch Events Template
// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet
// See https://github.com/sinch/sinch-sdk-dotnet/tree/main/examples/templates/sinchevents/README.md for details


using Sinch;
using Sinch.Numbers.SinchEvents;
using Sinch.SMS.Hooks;
using SinchEvents.Template.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSinchClient(() => new SinchClientConfiguration());

builder.Services.AddSingleton<SmsServerBusinessLogic>();
builder.Services.AddSingleton<ISmsWebhooks>(sp => sp.GetRequiredService<ISinchClient>().Sms.Webhooks);
builder.Services.AddSingleton<INumbersSinchEvents>(sp => sp.GetRequiredService<ISinchClient>().Numbers.SinchEvents);

var app = builder.Build();

app.MapControllers();

app.Run();

