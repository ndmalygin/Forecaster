using System.Text;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ForecastsRabbitMQDispatcher;

public class RabbitMQDispatcher : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private IChannel _channel;
    private readonly string _rabbitUri;
    public EventHandler<string>? Received;

    public RabbitMQDispatcher(string rabbitUri)
    {
        _rabbitUri = rabbitUri;
    }

    public async Task StartAsync()
    {
        try
        {
            var factory = new ConnectionFactory { HostName = _rabbitUri };
            var connection = await factory.CreateConnectionAsync();
            _channel = await connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync("forecasts", ExchangeType.Fanout);
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

    public async Task PublishMessageAsync(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        var props = new RabbitMQ.Client.BasicProperties();
        try
        {
            await _channel.BasicPublishAsync(
                exchange: "forecasts",
                routingKey: string.Empty,
                mandatory: true,
                basicProperties: props, // Вместо null
                body: new ReadOnlyMemory<byte>(body),
                cancellationToken: CancellationToken.None // Если токен не используется
            );
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public async Task ConsumeMessage()
    {
        var queueName = _channel.QueueDeclareAsync().Result.QueueName;
        await _channel.QueueBindAsync(queueName,
            "forecasts",
            string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Received?.Invoke(this, message);
            return Task.CompletedTask;
        };
        await _channel.BasicConsumeAsync(queueName,
            true,
            consumer);
    }
}