using Sinch;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();

// Sinch SDK with IHttpClientFactory
// This demonstrates the recommended configuration for ASP.NET Core applications
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
