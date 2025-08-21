using moto_rent_consumer.Features.Motors.DTOs;
using moto_rent_consumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Consumer : IHostedService
{
    private readonly ILogger<Consumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IModel? _channel;

    private const int MaxRetry = 20;
    private const int RetryDelaySeconds = 10;

    public Consumer(IServiceProvider serviceProvider, ILogger<Consumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        int attempt = 0;
        while (attempt < MaxRetry && _connection == null)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "rabbitmq" };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: "moto.cadastrada",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                _logger.LogInformation("Consumer initialized and queue declared.");
                StartListening();
                break;
            }
            catch (Exception ex)
            {
                attempt++;
                _logger.LogWarning(ex, "Attempt {Attempt}/{MaxRetry} - Failed to connect to RabbitMQ. Retrying in {Delay}s...", attempt, MaxRetry, RetryDelaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(RetryDelaySeconds), cancellationToken);
            }
        }

        if (_connection == null)
        {
            _logger.LogError("Could not connect to RabbitMQ after {MaxRetry} attempts.", MaxRetry);
        }
    }

    private void StartListening()
    {
        if (_channel == null) return;

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Message received: {Message}", message);

                var moto = JsonSerializer.Deserialize<MotorDto>(message);

                if (moto != null && moto.ano == 2024)
                {
                    _logger.LogInformation("Processing moto: {id} {ano}", moto.identificador, moto.ano);

                    using var scope = _serviceProvider.CreateScope();
                    var motorService = scope.ServiceProvider.GetRequiredService<MotorService>();
                    await motorService.CreateMotorAsync(moto);
                }
                else
                {
                    _logger.LogWarning("Received message could not be deserialized or year is not 2024.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue.");
            }
        };

        _channel.BasicConsume(queue: "moto.cadastrada",
                             autoAck: true,
                             consumer: consumer);

        _logger.LogInformation("Consumer started and listening to queue 'moto.cadastrada'.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping consumer...");

        _channel?.Close();
        _connection?.Close();

        return Task.CompletedTask;
    }
}
