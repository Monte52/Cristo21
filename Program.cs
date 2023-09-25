using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMqGenericFramwork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Added services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitMQSettings = new RabbitMQSettings();
builder.Configuration.GetSection("RabbitMQ").Bind(rabbitMQSettings);


builder.Services.AddSingleton(rabbitMQSettings);

builder.Services.AddSingleton<IConnectionFactoryWrapper, ConnectionFactoryWrapper>(_ =>
    new ConnectionFactoryWrapper(rabbitMQSettings.ConnectionString ?? "localhost"));

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factoryWrapper = sp.GetRequiredService<IConnectionFactoryWrapper>();
    return factoryWrapper.GetConnection();
});
builder.Services.AddSingleton<IChannelWrapper, ChannelWrapper>();
builder.Services.AddSingleton<IRabbitMQConsumerService, RabbitMQConsumerService>();

var app = builder.Build();

// Watch for changes in the configuration file.
//app.Configuration.GetReloadToken().RegisterChangeCallback(async (_) =>
//{
//    // Reload the RabbitMQ settings
//    var newSettings = new RabbitMQSettings();
//    app.Configuration.GetSection("RabbitMQ").Bind(newSettings);

//    // Start new consumers based on the new settings
//    var rabbitMQConsumerService = app.Services.GetRequiredService<IRabbitMQConsumerService>();
//    rabbitMQConsumerService.StartNewConsumersBasedOnNewSettings(newSettings);

//}, null);


// Fetch Queue Names and Start Consumers
var rabbitMQConsumerService = app.Services.GetRequiredService<IRabbitMQConsumerService>();
rabbitMQConsumerService.StartConsumers();


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


