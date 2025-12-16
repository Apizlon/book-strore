using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Services;

public class ClickHouseSender : IClickHouseSender
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<ClickHouseSender> _logger;
    private readonly string _topic;

    public ClickHouseSender(ILogger<ClickHouseSender> logger, IConfiguration config)
    {
        _logger = logger;
        _topic = config["Kafka:Topic"] ?? "book-metrics";

        var bootstrapServers = config["Kafka:BootstrapServers"] ?? "kafka:9092";

        _logger.LogInformation("Initializing Kafka producer: {BootstrapServers}, Topic: {Topic}", bootstrapServers, _topic);

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            CompressionType = CompressionType.Snappy,
            RequestTimeoutMs = 5000,
            TransactionTimeoutMs = 5000
        };

        _producer = new ProducerBuilder<string, string>(producerConfig)
            .SetErrorHandler((producer, error) =>
            {
                _logger.LogError("Kafka error: {Error}", error.Reason);
            })
            .SetLogHandler((producer, logMessage) =>
            {
                _logger.LogInformation("Kafka: {Message}", logMessage.Message);
            })
            .Build();

        _logger.LogInformation("Kafka producer initialized successfully");
    }

    public Task SendEventAsync(string eventType, string userName, string status, string errorMessage = "")
    {
        // ✅ Запускаем асинхронно БЕЗ await - не блокируем
        _ = Task.Run(async () =>
        {
            try
            {
                var clickHouseEvent = new ClickHouseEvent
                {
                    EventDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    Event = eventType,
                    UserName = userName,
                    Status = status,
                    Name = errorMessage
                };

                var json = JsonSerializer.Serialize(clickHouseEvent);

                _logger.LogInformation("Sending to Kafka: {Json}", json);

                var message = new Message<string, string>
                {
                    Key = userName,
                    Value = json
                };

                var result = await _producer.ProduceAsync(_topic, message);
                _logger.LogInformation("✅ Event sent to Kafka: {Event}/{UserName} (Offset: {Offset})", 
                    eventType, userName, result.Offset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send event to Kafka: {Event} for user {UserName}", 
                    eventType, userName);
            }
        });

        return Task.CompletedTask;
    }
}
