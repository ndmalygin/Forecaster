using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ForecastsCollector
{
    public class MongoDBDispatcher
    {
        private readonly string _connectionString;
        private readonly string _dbName;
        private readonly MongoClient _client;
        public MongoDBDispatcher(string connectionString)
        {
            _connectionString = connectionString;
            _client = new MongoClient(_connectionString);
            _dbName = "Forecaster";
        }

        public void WriteWeatherData(string city, string weatherData)
        {
            var document = BsonSerializer.Deserialize<BsonDocument>(weatherData);
            var collection = _client.GetDatabase(_dbName).GetCollection<BsonDocument>(city);
            collection.InsertOneAsync(document);
        }
    }
}