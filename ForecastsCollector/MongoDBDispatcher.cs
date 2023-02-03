using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ForecastsCollector
{
    public class MongoDBDispatcher
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private readonly string _dbName;
        private readonly MongoClient _client;

        public MongoDBDispatcher(string connectionString)
        {
            _connectionString = connectionString;
            _dbName = "forecaster";
            _client = new MongoClient(_connectionString);
        }

        public void WriteWeatherData(string city, string weatherData)
        {
            try
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(weatherData);
                var collection = _client.GetDatabase(_dbName).GetCollection<BsonDocument>(city);
                collection.InsertOneAsync(document);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
    }
}