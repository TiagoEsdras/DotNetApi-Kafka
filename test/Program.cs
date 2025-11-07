using System.Text.Json.Serialization;
using Confluent.Kafka;
using test.Application.Interfaces;
using test.Application.Services;
using test.Infra.Consumer;
using test.Infra.Producer;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
var consumerGroupId = Environment.GetEnvironmentVariable("KAFKA_CONSUMER_GROUP_ID");
builder.Services.AddSingleton(new ProducerConfig
{
    BootstrapServers = bootstrapServers
});
builder.Services.AddSingleton(new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = consumerGroupId,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = true
});
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IEventStore>(new InMemoryEventStore(1000));
builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();