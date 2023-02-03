using ForecastsCollector;
using ForecastsPublisher;

internal class Program
{
    private static void Main(string[] args)
    {
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
    }
}