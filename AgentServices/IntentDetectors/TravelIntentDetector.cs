namespace FlightBooking.AgentServices.IntentDetectors
{
    public class TravelIntentDetector : IIntentDetector
    {
        public TravelIntent Detect(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return TravelIntent.Unknown;

            prompt = prompt.ToLower();

            if (prompt.Contains("restoran") ||
                prompt.Contains("yemək") ||
                prompt.Contains("burger") ||
                prompt.Contains("pizza") ||
                prompt.Contains("səhər yeməyi"))
            {
                return TravelIntent.Restaurant;
            }

            if (prompt.Contains("hava") ||
                prompt.Contains("yağış") ||
                prompt.Contains("temperatur") ||
                prompt.Contains("hava durumu") ||
                prompt.Contains("hava vəziyyəti"))
            {
                return TravelIntent.Weather;
            }

            if (prompt.Contains("otel") ||
                prompt.Contains("yerləşmə") ||
                prompt.Contains("qalacaq"))
            {
                return TravelIntent.Hotel;
            }

            if (prompt.Contains("nəqliyyat") ||
                prompt.Contains("metro") ||
                prompt.Contains("avtobus") ||
                prompt.Contains("taksi") ||
                prompt.Contains("hava limanı"))
            {
                return TravelIntent.Transportation;
            }

            if (prompt.Contains("məzənnə") ||
                prompt.Contains("valyuta") ||
                prompt.Contains("avro") ||
                prompt.Contains("funt") ||
                prompt.Contains("dollar"))
            {
                return TravelIntent.Currency;
            }

            if (prompt.Contains("səyahət planı") ||
                prompt.Contains("marşrut") ||
                prompt.Contains("planlaşdır"))
            {
                return TravelIntent.Itinerary;
            }

            if (prompt.Contains("gəzməli") ||
                prompt.Contains("muzey") ||
                prompt.Contains("meydan") ||
                prompt.Contains("köhnə şəhər") ||
                prompt.Contains("turistik") ||
                prompt.Contains("gəzməli yerlər"))
            {
                return TravelIntent.Attraction;
            }

            return TravelIntent.Unknown;
        }
    }
}