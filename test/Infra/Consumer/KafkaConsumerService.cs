using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using test.Application.Interfaces;
using test.Domain;

namespace test.Infra.Consumer
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly ConsumerConfig _config;
        private readonly IEventStore _store;
        private const string _topicName = "event-topic";
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter(allowIntegerValues: true) }
        };

        public KafkaConsumerService(ILogger<KafkaConsumerService> logger, ConsumerConfig config, IEventStore store)
        {
            _logger = logger;
            _config = config;
            _store = store;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    var builder = new ConsumerBuilder<Ignore, string>(_config)
                        .SetErrorHandler((_, e) => _logger.LogWarning("Kafka consumer error: {Reason}", e.Reason));
                    using var consumer = builder.Build();
                    consumer.Subscribe(_topicName);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = consumer.Consume(stoppingToken);
                            if (cr?.Message?.Value is string json)
                            {
                                try
                                {
                                    var evt = JsonSerializer.Deserialize<Event>(json, _jsonOptions);
                                    if (evt != null)
                                    {
                                        _store.Add(evt);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Failed to deserialize event JSON");
                                }
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Kafka consume loop error");
                        }
                    }

                    consumer.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kafka consumer failed to start");
                }
            }, stoppingToken);
        }
    }
}