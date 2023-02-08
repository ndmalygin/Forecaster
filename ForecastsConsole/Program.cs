using ForecastsCollector;
using ForecastsCommon;
using ForecastsRabbitMQProcessor;

var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
using var forecastsCollector = new OpenWeatherAPIController("");
using var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");

var cities = new[] { "Antalya", "Krasnoyarsk", "Saint-Petersburg", "Phuket" };

while (true)
{
    foreach (var city in cities)
    {
        var weatherData = forecastsCollector.GetWeatherAsync(city).Result;
        mongoDBDispatcher.WriteWeatherData(city, weatherData);
        var weatherExtractor = new WeatherExtractor();
        var weatherMandatory = weatherExtractor.ExtractMandatoryData(weatherData);
        rabbitMQDispatcher.PublishMessage(weatherMandatory);

        Console.WriteLine(weatherData);
    }

    Thread.Sleep(120000);
}
