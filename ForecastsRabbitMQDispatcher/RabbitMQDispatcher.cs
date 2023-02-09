using System.Text;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ForecastsRabbitMQDispatcher;

public class RabbitMQDispatcher : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IModel _channel;
    private readonly string _rabbitUri;
    public EventHandler<string>? Received;

    public RabbitMQDispatcher(string rabbitUri)
    {
        _rabbitUri = rabbitUri;

        try
        {
            var factory = new ConnectionFactory { HostName = _rabbitUri };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare("forecasts", ExchangeType.Fanout);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
    }

    public void PublishMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        try
        {
            _channel.BasicPublish("forecasts",
                string.Empty,
                true,
                null,
                body);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public void ConsumeMessage()
    {
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queueName,
            "forecasts",
            string.Empty);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Received?.Invoke(this, message);
        };
        _channel.BasicConsume(queueName,
            true,
            consumer);
    }
}