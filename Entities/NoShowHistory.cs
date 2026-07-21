using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlightBooking.Entities
{
    public class NoShowHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("route")]
        public string Route { get; set; }

        [BsonElement("flightDate")]
        public string FlightDate { get; set; }

        [BsonElement("flightSlot")]
        public string FlightSlot { get; set; }

        [BsonElement("aircraftType")]
        public string AircraftType { get; set; }

        [BsonElement("capacity")]
        public int Capacity { get; set; }

        [BsonElement("soldTickets")]
        public int SoldTickets { get; set; }

        [BsonElement("onlineCheckedIn")]
        public int OnlineCheckedIn { get; set; }

        [BsonElement("airportCheckedIn")]
        public int AirportCheckedIn { get; set; }

        [BsonElement("boardedPassenger")]
        public int BoardedPassenger { get; set; }

        [BsonElement("noShowPassenger")]
        public int NoShowPassenger { get; set; }

        [BsonElement("onlineCheckInNoShow")]
        public int OnlineCheckInNoShow { get; set; }

        [BsonElement("missedConnection")]
        public int MissedConnection { get; set; }

        [BsonElement("cancelledPassenger")]
        public int CancelledPassenger { get; set; }
    }
}