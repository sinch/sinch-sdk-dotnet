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
    new SinchClientConfiguration()
    {
        SinchCommonCredentials = new SinchCommonCredentials()
        {
            KeySecret = builder.Configuration["Sinch:KeySecret"]!,
            KeyId = builder.Configuration["Sinch:KeyId"]!,
            ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        },
        SinchOptions = new SinchOptions()
        {
            LoggerFactory = LoggerFactory.Create(config => { config.AddConsole(); }),
            HttpClient = new HttpClient()
        }
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
