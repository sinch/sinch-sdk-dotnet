using Sinch;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    // Optional: Add more configurations as needed
    // VerificationConfiguration = new SinchVerificationConfiguration { ... },
    // VoiceConfiguration = new SinchVoiceConfiguration { ... }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  // DNS refresh every 5 minutes
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),   // DNS refresh interval
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2), // Close idle connections after 2 minutes
    MaxConnectionsPerServer = 10  // HTTP/1.1 best practice (6-10 connections)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
