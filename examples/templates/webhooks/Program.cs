using Sinch;
using Sinch.SMS.Hooks;
using SmsWebhookTemplate.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<ServerBusinessLogic>();

var sinchKeyId = builder.Configuration["Sinch:KeyId"];
var sinchKeySecret = builder.Configuration["Sinch:KeySecret"];
var projectId = builder.Configuration["Sinch:ProjectId"];

builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = sinchKeyId,
        KeySecret = sinchKeySecret
    }
});

builder.Services.AddSingleton<ISmsWebhooks>(sp => sp.GetRequiredService<ISinchClient>().Sms.Webhooks);

var app = builder.Build();

app.MapControllers();

app.Run();

