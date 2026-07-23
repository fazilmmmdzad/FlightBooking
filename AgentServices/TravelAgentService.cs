using FlightBooking.AgentServices.CityDetectors;
using FlightBooking.AgentServices.GroqServices;
using FlightBooking.AgentServices.IntentDetectors;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.Dtos.AgentDtos;
using FlightBooking.Tools.WeatherTool;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IGroqService _groqService;
        private readonly ITravelPromptBuilder _promptBuilder;
        private readonly IIntentDetector _intentDetector;
        private readonly IWeatherTool _weatherTool;
        private readonly ICityExtractor _cityExtractor;
        public TravelAgentService(IGroqService groqService, ITravelPromptBuilder promptBuilder, IIntentDetector intentDetector, IWeatherTool weatherTool, ICityExtractor cityExtractor)
        {
            _groqService = groqService;
            _promptBuilder = promptBuilder;
            _intentDetector = intentDetector;
            _weatherTool = weatherTool;
            _cityExtractor = cityExtractor;
        }

        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            var intent = _intentDetector.Detect(prompt);

            string intentInstruction;

            string? city = null;
            WeatherResult? weatherResult = null;

            switch (intent)
            {
                case TravelIntent.Weather:
                    {
                        city = await _cityExtractor.ExtractCityAsync(prompt);

                        if (string.IsNullOrWhiteSpace(city))
                        {
                            intentInstruction =
                                "İstifadəçi hava məlumatı soruşur, lakin şəhər qeyd etməyib. " +
                                "Əvvəlcə hansı şəhərin hava məlumatını öyrənmək istədiyini soruş.";

                            break;
                        }

                        weatherResult = await _weatherTool.GetWeatherAsync(city);

                        if (weatherResult == null)
                        {
                            intentInstruction =
                                $"'{city}' şəhəri üçün hava məlumatı əldə edilə bilmədi. " +
                                "İstifadəçidən şəhərin adını yenidən dəqiqləşdirməsini xahiş et.";

                            break;
                        }

                        var forecastText = string.Join(
                            "\n",
                            weatherResult.Forecasts.Select(x =>
                                $"{x.Day}: Ən aşağı {x.Low}°C, Ən yüksək {x.High}°C, Hava: {x.Condition}")
                        );

                        intentInstruction =
                            $"İstifadəçi hava məlumatı soruşur.\n\n" +
                            $"Weather Tool tərəfindən əldə edilmiş real məlumatlar:\n" +
                            $"Şəhər: {weatherResult.City}\n" +
                            $"Ölkə: {weatherResult.Country}\n" +
                            $"Saat qurşağı: {weatherResult.TimeZoneId}\n" +
                            $"Temperatur: {weatherResult.Temperature}°C\n" +
                            $"Hava: {weatherResult.Condition}\n" +
                            $"Rütubət: %{weatherResult.Humidity}\n" +
                            $"Külək: {weatherResult.WindSpeed} km/s ({weatherResult.WindDirection})\n" +
                            $"Görünüş məsafəsi: {weatherResult.Visibility} km\n" +
                            $"Təzyiq: {weatherResult.Pressure} hPa\n" +
                            $"Günəşin doğuşu: {weatherResult.Sunrise}\n" +
                            $"Günəşin batışı: {weatherResult.Sunset}\n" +
                            $"7 günlük proqnoz:\n{forecastText}\n\n" +
                            $"Yalnız yuxarıdakı real Weather Tool məlumatlarından istifadə et. " +
                            $"Heç bir hava proqnozu uydurma. " +
                            $"Lazım gələrsə geyim, çətir və qısa səyahət tövsiyəsi ver.";

                        break;
                    }

                case TravelIntent.Restaurant:
                    intentInstruction =
                        "İstifadəçi restoran tövsiyəsi istəyir.";
                    break;

                case TravelIntent.Hotel:
                    intentInstruction =
                        "İstifadəçi otel tövsiyəsi istəyir.";
                    break;

                case TravelIntent.Transportation:
                    intentInstruction =
                        "İstifadəçi nəqliyyat seçimləri haqqında məlumat istəyir.";
                    break;

                case TravelIntent.Currency:
                    intentInstruction =
                        "İstifadəçi valyuta məzənnəsi haqqında məlumat istəyir.";
                    break;

                case TravelIntent.Itinerary:
                    intentInstruction =
                        "İstifadəçi üçün səyahət planı hazırla.";
                    break;

                case TravelIntent.Attraction:
                    intentInstruction =
                        "İstifadəçi gəzməli yerlər haqqında tövsiyə istəyir.";
                    break;

                default:
                    intentInstruction =
                        "İstifadəçinin səyahətlə bağlı sualına kömək et.";
                    break;
            }

            var finalPrompt = _promptBuilder.BuildPrompt(
                $"{intentInstruction}\n\nİstifadəçinin sorğusu:\n{prompt}"
            );

            var result = await _groqService.GetResponseAsync(finalPrompt);

            result.Intent = intent.ToString();
            result.City = city;
            result.Weather = weatherResult;

            return result;
        }
    }
}
