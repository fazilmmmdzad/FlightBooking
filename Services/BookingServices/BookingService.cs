using FlightBooking.Dtos.BookingDtos;
using FlightBooking.Entities;
using FlightBooking.Services.BookingServices;
using FlightBooking.Settings;
using MongoDB.Driver;

public class BookingService : IBookingService
{
    private readonly IMongoCollection<Booking> _bookingCollection;
    private readonly IMongoCollection<Flight> _flightCollection;

    public BookingService(IDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _bookingCollection = database.GetCollection<Booking>(settings.BookingCollectionName);
        _flightCollection = database.GetCollection<Flight>(settings.FlightCollectionName);
    }

    public async Task CreateBookingAsync(CreateBookingDto dto)
    {
        var flight = await _flightCollection
            .Find(x => x.FlightId == dto.FlightId)
            .FirstOrDefaultAsync();

        //if (flight == null)
        //    throw new Exception("Uçuş bulunamadı");

        var passengerCount = dto.Passengers.Count;

        //if (flight.AvailableSeats < passengerCount)
        //    throw new Exception("Yeterli koltuk yok");

        var passengers = dto.Passengers.Select(x => new Passenger
        {
            Name = x.Name,
            Surname = x.Surname,
            BirthDate = x.BirthDate,
            Gender = x.Gender,
            PassengerType = x.PassengerType
        }).ToList();

        var totalPrice = passengerCount * flight.BasePrice;

        var booking = new Booking
        {
            FlightId = dto.FlightId,
            Passengers = passengers,

            ContactName = dto.ContactName,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,

            TotalPrice = totalPrice,
            BookingDate = DateTime.Now,
            Status = "Confirmed"
        };

        await _bookingCollection.InsertOneAsync(booking);

        //var update = Builders<Flight>.Update
        //    .Inc(x => x.AvailableSeats, -passengerCount);

        //await _flightCollection.UpdateOneAsync(
        //    x => x.FlightId == dto.FlightId,
        //    update
        //);
    }
}