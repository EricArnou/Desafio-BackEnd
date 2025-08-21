using moto_rent.Features.Motors.DTOs;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class MotoEventPublisher
{
    private const string QueueName = "moto.cadastrada";

    public void PublishMotoCadastrada(MotorDto moto)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };

        // Cria conexão e canal somente aqui
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declara fila
        channel.QueueDeclare(queue: QueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        // Serializa e envia mensagem
        var message = JsonSerializer.Serialize(moto);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: QueueName,
                             basicProperties: null,
                             body: body);
    }
}
