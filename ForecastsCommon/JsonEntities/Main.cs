using MongoDB.Bson.Serialization.Attributes;

namespace ForecastsCommon.JsonEntities;

[BsonIgnoreExtraElements]
public class Main
{
    [BsonElement("temp")] public double Temperature { get; set; }

    [BsonElement("pressure")] public double Pressure { get; set; }

    [BsonElement("humidity")] public double Humidity { get; set; }
}