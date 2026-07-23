using System.Reflection.Metadata.Ecma335;

namespace FlightBooking.Dtos.WeatherDtos
{
    public class WeatherForecastResult
    {
        public string Day { get; set; }
        public long Date { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Condition { get; set; }
    }
}
