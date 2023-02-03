using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ForecastsRabbitMQProcessor
{
    public class RabbitMQDispatcher : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _rabbitUri;
        private readonly IModel _channel;
        public EventHandler<string> Received;

        public RabbitMQDispatcher(string rabbitUri)
        {
            _rabbitUri = rabbitUri;

            try
            {
                var factory = new ConnectionFactory { HostName = _rabbitUri };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "forecasts", type: ExchangeType.Fanout);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            try
            {
                _channel.BasicPublish(exchange: "forecasts",
                                    routingKey: string.Empty,
                                    mandatory: true,
                                    basicProperties: null,
                                    body: body);
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
            _channel.QueueBind(queue: queueName,
                              exchange: "forecasts",
                              routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Received?.Invoke(this, message);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}