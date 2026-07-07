namespace FlightBooking.Dtos.FlightDtos
{
    public class ResultFlightDto
    {
        public string FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string AirlineCode { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }
}
