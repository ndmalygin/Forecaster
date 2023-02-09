using Config.Net;
using ForecastsCollector;
using ForecastsCollector.Enums;
using ForecastsCommon;
using ForecastsCommon.JsonEntities;

namespace ForecastsWebApp.Data;

public class WeatherForecastService
{
    private readonly string _mongoUri;

    public WeatherForecastService()
    {
        var settings = new ConfigurationBuilder<ISettings>()
            .UseIniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forecaster.ini"))
            .Build();

        _mongoUri = settings.mongodb_connection;
    }

    public Task<Weather[]> GetForecastAsync(string city, string period, bool allValuesMode)
    {
        var mongoDBDispatcher = new MongoDBDispatcher(_mongoUri);
        var periodEnum = (Periods)Enum.Parse(typeof(Periods), period);

        switch (periodEnum)
        {
            case Periods.Day:
                return Task.FromResult(mongoDBDispatcher.GetWeathers(city, allValuesMode).AsEnumerable().Where(v => v.Date > DateTime.Now.AddDays(-1)).ToArray());
            case Periods.Week:
                return Task.FromResult(mongoDBDispatcher.GetWeathers(city, allValuesMode).AsEnumerable().Where(v => v.Date > DateTime.Now.AddDays(-7)).ToArray());
            case Periods.Month:
                return Task.FromResult(mongoDBDispatcher.GetWeathers(city, allValuesMode).AsEnumerable().Where(v => v.Date > DateTime.Now.AddDays(-30)).ToArray());
            default:
                return Task.FromResult(mongoDBDispatcher.GetWeathers(city, allValuesMode).ToArray());
        }
    }

    public Task<string[]> GetCitiesNamesAsync()
    {
        var mongoDBDispatcher = new MongoDBDispatcher(_mongoUri);
        return Task.FromResult(mongoDBDispatcher.GetCities().ToArray());
    }
}