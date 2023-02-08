using Config.Net;
using ForecastsCommon.JsonEntities;
using ForecastsCollector;
namespace ForecastsBlazor.Data;
public class WeatherForecastService
{
    private string _mongoUri;
    public WeatherForecastService()
    {
        var settings = new ConfigurationBuilder<ISettings>()
           .UseIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forecaster.ini"))
           .Build();

        _mongoUri = settings.mongodb_connection;
    }

    public Task<Weather[]> GetForecastAsync(string city)
    {
        var mongoDBDispatcher = new MongoDBDispatcher(_mongoUri);
        return Task.FromResult(mongoDBDispatcher.GetWeathers(city).ToArray());
    }

    public Task<string[]> GetCitiesNamesAsync()
    {
        var mongoDBDispatcher = new MongoDBDispatcher(_mongoUri);
        return Task.FromResult(mongoDBDispatcher.GetCities().ToArray());
    }
}
