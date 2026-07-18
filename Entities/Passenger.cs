namespace FlightBooking.Entities
{
    public class Passenger
    {
        public string PassengerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PassengerType { get; set; }
        public string? SeatNumber { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string? TicketStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? CheckInStatus { get; set; }
        public int BaggageKg { get; set; }
        public string? MealType { get; set; }
        public List<string>? ExtraServices { get; set; }

        public string? BoardingPassNumber { get; set; }
        public string? Gate { get; set; }
        public DateTime? BoardingTime { get; set; }

    }
}