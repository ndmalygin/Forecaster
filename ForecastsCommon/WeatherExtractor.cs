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
            Main = new Main
            {
                Temperature = obj?.main.temp,
                Humidity = obj?.main.humidity,
                Pressure = obj?.main.pressure,
            },
            Wind = new Wind
            {
                Speed = obj?.wind.speed
            },
            City = obj?.name,
            date = obj?.dt
        };

        return JsonConvert.SerializeObject(weather);
    }

    public Weather? ExtractMandatoryDataObject(string json)
    {
        return JsonConvert.DeserializeObject<Weather>(json);
    }
}
