using FlightBooking.Dtos.CheckInDtos;
using FlightBooking.Entities;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.Services.CheckInServices
{
    public class CheckInService : ICheckInService
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<CheckIn> _checkInCollection;

        public CheckInService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _bookingCollection = database.GetCollection<Booking>(settings.BookingCollectionName);
            _checkInCollection = database.GetCollection<CheckIn>(settings.CheckInCollectionName);
        }
        public async Task CompleteCheckInAsync(CompleteCheckInDto dto)
        {
            var booking = await _bookingCollection
                .Find(x => x.Passengers.Any(p => p.PassengerId == dto.PassengerId))
                .FirstOrDefaultAsync();

            if (booking == null)
                throw new Exception("Booking bulunamadı");

            var passenger = booking.Passengers
                .FirstOrDefault(p => p.PassengerId == dto.PassengerId);

            if (passenger == null)
                throw new Exception("Yolcu bulunamadı");

            var boardingPass = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            var gates = new[] { "A1", "A2", "B5", "C3", "D7" };
            var randomGate = gates[new Random().Next(gates.Length)];

            var filter = Builders<Booking>.Filter.ElemMatch(x => x.Passengers, p => p.PassengerId == dto.PassengerId);

            var update = Builders<Booking>.Update
                .Set("Passengers.$.IsCheckedIn", true)
                .Set("Passengers.$.CheckInDate", DateTime.Now)
                .Set("Passengers.$.SeatNumber", dto.SeatNumber)
                .Set("Passengers.$.BaggageKg", dto.BaggageKg)
                .Set("Passengers.$.MealType", dto.MealType)
                .Set("Passengers.$.ExtraServices", dto.ExtraServices)
                .Set("Passengers.$.BoardingPassNumber", boardingPass)
                //.Set("Passengers.$.Gate", randomGate)
                .Set("Passengers.$.BoardingTime", DateTime.Now.AddMinutes(30));

            await _bookingCollection.UpdateOneAsync(filter, update);

            var checkIn = new CheckIn
            {
                CheckInId = Guid.NewGuid().ToString(),
                PassengerId = dto.PassengerId,
                FlightId = booking.FlightId,
                PnrNumber = booking.PnrNumber,

                CheckInDate = DateTime.Now,
                IsCheckedIn = true,

                SeatNumber = dto.SeatNumber,
                ExtraTotalPrice = dto.ExtraTotalPrice
            };

            await _checkInCollection.InsertOneAsync(checkIn);
        }
    }
}