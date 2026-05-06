// Sinch Events Template
// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet
// See https://github.com/sinch/sinch-sdk-dotnet/tree/main/examples/templates/sinch-events/README.md for details


using Microsoft.Extensions.DependencyInjection;
using Sinch;
using SinchEvents.Template.Numbers;
using SinchEvents.Template.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<SmsServerBusinessLogic>();
builder.Services.AddSingleton<NumbersServerBusinessLogic>();

builder.Services.AddSinchClient(() => new SinchClientConfiguration());

builder.Services.AddSinchEventHandlers(ServiceLifetime.Scoped);

var app = builder.Build();

app.MapControllers();

app.Run();

