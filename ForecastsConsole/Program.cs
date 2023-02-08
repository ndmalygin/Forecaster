using Config.Net;
using ForecastsCollector;
using ForecastsCommon;
using ForecastsRabbitMQProcessor;

var settings = new ConfigurationBuilder<ISettings>()
   .UseIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"forecaster.ini"))
   .Build();

var cities = settings.cities;

var mongoDBDispatcher = new MongoDBDispatcher(settings.mongodb_connection);
using var rabbitMQDispatcher = new RabbitMQDispatcher(settings.rabbitmq_connection);
using var forecastsCollector = new OpenWeatherAPIController(settings.api_key);

while (true)
{
    foreach (var city in cities)
    {
        var weatherData = forecastsCollector.GetWeatherAsync(city).Result;
        mongoDBDispatcher.WriteWeatherData(city, weatherData);
        var weatherExtractor = new WeatherExtractor();
        var weatherMandatory = weatherExtractor.ExtractMandatoryData(weatherData);
        rabbitMQDispatcher.PublishMessage(weatherMandatory);

        Console.WriteLine($"{DateTime.Now} {weatherData}");
    }

    Thread.Sleep(settings.interval);
}
