using Sinch;
using Sinch.SMS.Webhooks;
using SmsWebhookTemplate.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

// Add controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".xml");
    if (System.IO.File.Exists(xmlFile))
    {
        options.IncludeXmlComments(xmlFile);
    }
});

builder.Services.AddSingleton<ServerBusinessLogic>();

// Configure Sinch client (placeholder - real credentials in appsettings)
var sinchKeyId = builder.Configuration["Sinch:KeyId"]; // optional
var sinchKeySecret = builder.Configuration["Sinch:KeySecret"]; // optional
var projectId = builder.Configuration["Sinch:ProjectId"]; // optional

if (!string.IsNullOrEmpty(projectId) && !string.IsNullOrEmpty(sinchKeyId) && !string.IsNullOrEmpty(sinchKeySecret))
{
    // Use the recommended DI helper which configures IHttpClientFactory and proper options
    builder.Services.AddSinchClient(() => new SinchClientConfiguration
    {
        SinchUnifiedCredentials = new SinchUnifiedCredentials
        {
            ProjectId = projectId,
            KeyId = sinchKeyId,
            KeySecret = sinchKeySecret
        }
    });

    // Expose ISmsWebhooks directly for convenience in the webhook template
    builder.Services.AddSingleton<ISmsWebhooks>(sp => sp.GetRequiredService<ISinchClient>().Sms.Webhooks);
}
else
{
    // Provide a manual registration of a local SmsWebhooks implementation if the Sinch package is not present
    builder.Services.AddSingleton<ISmsWebhooks>(_ => new LocalSmsWebhooks());
}

var app = builder.Build();

// Enable swagger unconditionally (always available for the example)
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

// LocalSmsWebhooks has been moved to Sms/LocalSmsWebhooks.cs
