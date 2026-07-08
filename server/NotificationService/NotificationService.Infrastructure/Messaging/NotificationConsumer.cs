using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts;
using NotificationService.Application.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Infrastructure.Messaging
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationConsumer> _logger;
        private readonly IConfiguration _configuration; 
        private IConnection? _connection;
        private IChannel? _channel;
        private const string QueueName = "notification.orders";

        public NotificationConsumer(
            IServiceProvider serviceProvider,
            ILogger<NotificationConsumer> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create connection to RabbitMQ
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            var retryCount = 0;
            while(retryCount < 10)
            {
                try
                {
                    _connection = await factory.CreateConnectionAsync(cancellationToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                    // Declare queue - makes sure queue exists before consuming
                    await _channel.QueueDeclareAsync(
                        queue: QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        cancellationToken: cancellationToken
                        );

                    _logger.LogInformation("NotificationConsumer connected to RabbitMQ");
                    break;
                } 
                catch(Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning("RabbitMQ connection attempt {RetryCount} failed. Retrying in 5 seconds...", retryCount);
                    await Task.Delay(5000, cancellationToken);
                }
            }
            
            await base.StartAsync(cancellationToken);   
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Register callback - called automatically when message arrives
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                // Get raw bytes from queue
                var body = args.Body.ToArray();

                // Convert bytes to JSON string
                var json = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received message: {Json}", json);

                try
                {
                    // Check message type from header
                    var headers = args.BasicProperties.Headers;
                    var typeHeader = "";

if (headers != null && headers.ContainsKey("__TypeId__"))
{
    var typeBytes = headers["__TypeId__"] as byte[];
    typeHeader = typeBytes != null ? Encoding.UTF8.GetString(typeBytes) : "";
}

                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<INotificationHandler>();

                    if (typeHeader.Contains("OrderPlacedEvent"))
                    {
                        var @event = JsonSerializer.Deserialize<OrderPlacedEvent>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (@event != null)
                            await handler.HandleOrderPlacedAsync(@event);
                    }
                    else if (typeHeader.Contains("OrderCancelledEvent"))
                    {
                        var @event = JsonSerializer.Deserialize<OrderCancelledEvent>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (@event != null)
                            await handler.HandleOrderCancelledAsync(@event);
                    }

                    // Acknowledge message - tell RabbitMQ "I processed this successfully"
                    await _channel!.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    // Reject message - send back to queue to retry
                    await _channel!.BasicNackAsync(args.DeliveryTag, false, false, stoppingToken);
                }
            };

            // Start consuming
            await _channel!.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            // Keep running until application stops
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up connections when app stops
            if (_channel != null) await _channel.CloseAsync(cancellationToken);
            if (_connection != null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
