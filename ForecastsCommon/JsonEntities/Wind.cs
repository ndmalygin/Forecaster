using MongoDB.Bson.Serialization.Attributes;

namespace ForecastsCommon.JsonEntities;

[BsonIgnoreExtraElements]
public class Wind
{
    [BsonElement("speed")]
    public double Speed { get; set; }
}