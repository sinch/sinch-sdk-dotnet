using Sinch;
using Sinch.SMS.Hooks;
using Webhook.Template.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<ServerBusinessLogic>();

builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
});

builder.Services.AddSingleton<ISmsWebhooks>(sp => sp.GetRequiredService<ISinchClient>().Sms.Webhooks);

var app = builder.Build();

app.MapControllers();

app.Run();

