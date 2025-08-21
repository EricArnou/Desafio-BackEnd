using moto_rent.Features.Motors.DTOs;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class MotoEventPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MotoEventPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "moto.cadastrada",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublishMotoCadastrada(MotorDto moto)
    {
        var message = JsonSerializer.Serialize(moto);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: "moto.cadastrada",
                             basicProperties: null,
                             body: body);
    }
}
