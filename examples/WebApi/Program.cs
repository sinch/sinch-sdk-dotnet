using Sinch;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddSingleton<ISinchClient>(_ => new SinchClient(
    builder.Configuration["Sinch:ProjectId"],
    builder.Configuration["Sinch:KeyId"]!,
    builder.Configuration["Sinch:KeySecret"]!,
    options =>
    {
        options.LoggerFactory = LoggerFactory.Create(config => { config.AddConsole(); });
        options.HttpClient = new HttpClient();
    }));

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
