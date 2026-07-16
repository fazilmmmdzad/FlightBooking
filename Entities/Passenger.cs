namespace FlightBooking.Entities
{
    public class Passenger
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PassengerType { get; set; }
        public string? SeatNumber { get; set; }
        public string? TicketStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? CheckInStatus { get; set; }
    }
}