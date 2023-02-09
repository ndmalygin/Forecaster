using Config.Net;
using ForecastsRabbitMQDispatcher;

var settings = new ConfigurationBuilder<ISettings>()
   .UseIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"forecaster.ini"))
   .Build();

var rabbitMQDispatcher = new RabbitMQDispatcher(settings.rabbitmq_connection);
rabbitMQDispatcher.Received += (_, message) => Console.WriteLine($" [x] {message}");
rabbitMQDispatcher.ConsumeMessage();

Console.Read();