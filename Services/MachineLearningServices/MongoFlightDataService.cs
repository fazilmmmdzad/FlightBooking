using FlightBooking.MachineLearningModels;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.Services.MachineLearningServices
{
    public class MongoFlightDataService
    {
        private readonly IMongoCollection<FlightRawData> _collection;
        public MongoFlightDataService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<FlightRawData>("FligtForecastDatas");
        }
        public async Task<List<FlightRawData>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
