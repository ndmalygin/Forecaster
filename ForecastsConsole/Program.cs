using ForecastsCollector;
using ForecastsRabbitMQProcessor;

var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
using var forecastsCollector = new OpenWeatherAPIController("");
using var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");

while (true)
{
    var weatherData = forecastsCollector.GetWeather("Belek").Result;
    mongoDBDispatcher.WriteWeatherData("Belek", weatherData);
    rabbitMQDispatcher.PublishMessage(weatherData);

    Console.WriteLine(weatherData);
    Thread.Sleep(1000);
}
