using ForecastsCommon.JsonEntities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NLog;

namespace ForecastsCollector;

public class MongoDBDispatcher : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly MongoClient _client;
    private readonly string _connectionString;
    private readonly string _dbName;

    public MongoDBDispatcher(string connectionString)
    {
        _connectionString = connectionString;
        _dbName = "forecaster";
        _client = new MongoClient(_connectionString);
    }

    public async Task WriteWeatherData(string city, string weatherData)
    {
        try
        {
            var document = BsonSerializer.Deserialize<BsonDocument>(weatherData);
            var collection = _client.GetDatabase(_dbName).GetCollection<BsonDocument>(city);
            await collection.InsertOneAsync(document);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public IQueryable<Weather> GetWeathers(string city, bool all = true)
    {
        if (!all)
        {
            var data = _client.GetDatabase(_dbName).GetCollection<Weather>(city).AsQueryable()
                .GroupBy(v => v.date, g => g)
                .Select(v => v.First()).OrderBy(d => d.date).ToArray();

            var prepared = new List<Weather>();
            for (var i = 1; i < data.Length; i++)
            {
                if (i == 1)
                    prepared.Add(data[0]);
                if (!data[i].Equals(data[i - 1]))
                    prepared.Add(data[i]);
            }

            return prepared.AsQueryable();
        }

        return _client.GetDatabase(_dbName).GetCollection<Weather>(city).AsQueryable().GroupBy(v => v.date, g => g)
            .Select(v => v.First()).OrderBy(d => d.date);
    }

    public async Task<IAsyncCursor<string>> GetCitiesAsync()
    {
        return await _client.GetDatabase(_dbName).ListCollectionNamesAsync();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}