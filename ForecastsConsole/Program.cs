using ForecastsCollector;

internal class Program
{
    private static void Main(string[] args)
    {
        var forecastsCollector = new OpenWeatherAPIController("");
        var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");

        while (true)
        {
            var weatherData = forecastsCollector.GetWeather("Belek").Result;
            Console.WriteLine(weatherData);

            mongoDBDispatcher.WriteWeatherData("Belek", weatherData);

            Thread.Sleep(1000);
        }
    }
}