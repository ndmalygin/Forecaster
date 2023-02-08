using ForecastsCommon.JsonEntities;
using ForecastsCollector;
namespace ForecastsBlazor.Data;
public class WeatherForecastService
{
    public Task<Weather[]> GetForecastAsync(string city)
    {
        var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
         return Task.FromResult(mongoDBDispatcher.GetWeathers(city).ToArray());
    }

    public Task<string[]> GetCitiesNamesAsync()
    {
        var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
         return Task.FromResult(mongoDBDispatcher.GetCities().ToArray());
    }
}
