using ForecastsCommon.JsonEntities;
using Newtonsoft.Json;

namespace ForecastsCommon;

public class WeatherExtractor
{
    public string ExtractMandatoryData(string json)
    {
        var obj = JsonConvert.DeserializeObject<dynamic>(json);
        var weather = new Weather
        {
            Temperature = obj.main.temp,
            Humidity = obj.main.humidity,
            Pressure = obj.main.pressure,
            Wind = obj.wind.speed
        };

        return JsonConvert.SerializeObject(weather);
    }
}
