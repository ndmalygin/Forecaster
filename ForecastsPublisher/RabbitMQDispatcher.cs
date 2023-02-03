using System.Text;
using RabbitMQ.Client;

namespace ForecastsPublisher
{
    public class RabbitMQDispatcher : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _rabbitUri;
        private readonly IModel _channel;

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
            Logger.Info("Publishing message");
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
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}