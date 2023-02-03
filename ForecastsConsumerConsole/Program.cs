using ForecastsRabbitMQProcessor;

var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");
rabbitMQDispatcher.Received += (_, message) => Console.WriteLine($" [x] {message}");
rabbitMQDispatcher.ConsumeMessage();