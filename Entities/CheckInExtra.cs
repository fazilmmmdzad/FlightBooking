namespace FlightBooking.Entities
{
    public class CheckInExtra
    {
        public string CheckInExtraId { get; set; }
        public string CheckInId { get; set; }

        public string ExtraType { get; set; } 
        public string ExtraName { get; set; }

        public decimal Price { get; set; }
    }
}
