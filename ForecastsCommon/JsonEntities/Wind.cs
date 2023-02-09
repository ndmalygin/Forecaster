using MongoDB.Bson.Serialization.Attributes;

namespace ForecastsCommon.JsonEntities;

[BsonIgnoreExtraElements]
public class Wind
{
    [BsonElement("speed")] public double Speed { get; set; }

    public override bool Equals(object? obj)
    {
        return Speed == ((Wind)obj).Speed;
    }
}