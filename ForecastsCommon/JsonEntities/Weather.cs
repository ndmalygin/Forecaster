using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ForecastsCommon.JsonEntities;

[BsonIgnoreExtraElements]
public class Main
{
    [BsonElement("temp")]
    public double Temperature { get; set; }

    [BsonElement("pressure")]
    public double Pressure { get; set; }

    [BsonElement("humidity")]
    public double Humidity { get; set; }
}

[BsonIgnoreExtraElements]
public class Wind
{
    [BsonElement("speed")]
    public double Speed { get; set; }
}

[BsonIgnoreExtraElements]
public class Weather
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("main")]
    public Main? Main { get; set; }

    [BsonElement("wind")]
    public Wind? Wind { get; set; }

    [BsonElement("name")]
    public string? City { get; set; }

    [BsonElement("dt")]
    public int date { get; set; }

    public DateTime Date { get { return UnixTimeStampToDateTime(date); } }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    }
}
