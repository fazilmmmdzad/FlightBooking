using AutoMapper;
using FlightBooking.Dtos.FlightDtos;
using FlightBooking.Entities;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.Services.FlightServices
{
    public class FlightService : IFlightService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Flight> _flightCollection;
        public FlightService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _flightCollection = database.GetCollection<Flight>(_databaseSettings.FlightCollectionName);

            _mapper = mapper;
        }
        public async Task CreateFlightAsync(CreateFlightDto createFlightDto)
        {
            var values = _mapper.Map<Flight>(createFlightDto);
            await _flightCollection.InsertOneAsync(values);
        }

        public async Task DeleteFlightAsync(string id)
        {
            await _flightCollection.DeleteOneAsync(x => x.FlightId == id);
        }

        public async Task<List<ResultFlightDto>> GetAllFlightsAsync()
        {
            var values = await _flightCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultFlightDto>>(values);
        }

        public async Task<GetFlightByIdDto> GetFlightByIdAsync(string id)
        {
            var value = await _flightCollection.Find(x => x.FlightId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetFlightByIdDto>(value);
        }

        public async Task UpdateFlightAsync(UpdateFlightDto updateFlightDto)
        {
            var values = _mapper.Map<Flight>(updateFlightDto);
            await _flightCollection.FindOneAndReplaceAsync(x => x.FlightId == updateFlightDto.FlightId, values);
        }
    }
}
