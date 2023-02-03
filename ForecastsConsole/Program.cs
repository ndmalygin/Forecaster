using ForecastsCollector;
using ForecastsCommon;
using ForecastsRabbitMQProcessor;

var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
using var forecastsCollector = new OpenWeatherAPIController("");
using var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");

while (true)
{
    var weatherData = forecastsCollector.GetWeather("Belek").Result;
    mongoDBDispatcher.WriteWeatherData("Belek", weatherData);
    var weatherExtractor = new WeatherExtractor();
    var weatherMandatory = weatherExtractor.ExtractMandatoryData(weatherData);
    rabbitMQDispatcher.PublishMessage(weatherMandatory);

    Console.WriteLine(weatherData);
    Thread.Sleep(30000);
}
