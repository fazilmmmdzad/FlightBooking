namespace FlightBooking.Entities
{
    public class CheckIn
    {
        public string CheckInId { get; set; }
        public string PassengerId { get; set; }
        public string FlightId { get; set; }
        public string PnrNumber { get; set; }

        public DateTime CheckInDate { get; set; }
        public bool IsCheckedIn { get; set; }

        public string SeatNumber { get; set; }

        public decimal ExtraTotalPrice { get; set; }
    }
}
