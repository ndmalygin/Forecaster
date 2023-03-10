using System.Diagnostics;
using ForecastsCollector.Enums;
using ForecastsCollector.JsonEntities;
using Newtonsoft.Json;
using NLog;

namespace ForecastsCollector;

public class OpenWeatherAPIController : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string _apiKey;
    private readonly HttpClient _client = new();

    public OpenWeatherAPIController(string apiKey)
    {
        _apiKey = apiKey;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<string> GetWeatherAsync(string cityName)
    {
        try
        {
            var geoUri = GetGeocodeCallUri(cityName, string.Empty, string.Empty, 1);
            var responseBody = await _client.GetStringAsync(geoUri);
            var geoCodes = JsonConvert.DeserializeObject<List<GeoCodes>>(responseBody);
            Debug.Assert(geoCodes?.Count == 1);

            var weatherUri = GetWeatherCallUri(geoCodes[0].lat, geoCodes[0].lon, ForecastUnits.metric);
            responseBody = await _client.GetStringAsync(weatherUri);

            return await Task.FromResult(responseBody);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    private string GetGeocodeCallUri(string cityName, string stateCode, string countryCode, int limit)
    {
        return
            $"http://api.openweathermap.org/geo/1.0/direct?q={cityName},{stateCode},{countryCode}&limit={limit}&appid={_apiKey}";
    }

    private string GetWeatherCallUri(double lat, double lon, ForecastUnits units)
    {
        return
            $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units={Enum.GetName(units)}&lang=en&appid={_apiKey}";
    }
}