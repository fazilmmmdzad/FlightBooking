namespace FlightBooking.Dtos.PassengerDtos
{
    public class PassengerListItemDto
    {
        public string PassengerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public string PnrNumber { get; set; }

        public string PassengerType { get; set; }

        public string SeatNumber { get; set; }

        public string CheckInStatus { get; set; }

        public string PaymentStatus { get; set; }


        public string TicketStatus { get; set; }

        public string Phone { get; set; }
    }
}