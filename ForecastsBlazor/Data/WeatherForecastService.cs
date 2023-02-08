using ForecastsCommon.JsonEntities;
using ForecastsCollector;
namespace ForecastsBlazor.Data;
public class WeatherForecastService
{
    public Task<IQueryable<Weather>> GetForecastAsync()
    {
        var mongoDBDispatcher = new MongoDBDispatcher("mongodb://localhost:27017");
         return Task.FromResult(mongoDBDispatcher.GetWeathers("Antalya"));
    }
}
