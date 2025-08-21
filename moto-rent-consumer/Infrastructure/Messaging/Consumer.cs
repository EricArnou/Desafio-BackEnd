using moto_rent_consumer.Features.Motors;
using moto_rent_consumer.Features.Motors.DTOs;
using moto_rent_consumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class Consumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly MotorService _motorService;

    public Consumer(MotorService motorService)
    {
        _motorService = motorService;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "moto.cadastrada",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var moto = JsonSerializer.Deserialize<MotorDto>(message);

            if (moto != null && moto.ano == 2024)
            {
               await _motorService.CreateMotorAsync(moto);
            }
        };

        _channel.BasicConsume(queue: "moto.cadastrada",
                             autoAck: true,
                             consumer: consumer);
    }
}
