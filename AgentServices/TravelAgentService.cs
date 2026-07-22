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

        public TravelAgentService(IGroqService groqService, ITravelPromptBuilder promptBuilder, IIntentDetector intentDetector, IWeatherTool weatherTool)
        {
            _groqService = groqService;
            _promptBuilder = promptBuilder;
            _intentDetector = intentDetector;
            _weatherTool = weatherTool;
        }

        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            var intent = _intentDetector.Detect(prompt);

            string intentInstruction;

            switch (intent)
            {
                case TravelIntent.Weather:
                    var weatherResult = await _weatherTool.GetWeatherAsync("Amsterdam");

                    intentInstruction =
                        $"İstifadəçi hava haqqında məlumat istəyir. " +
                        $"Cari hava məlumatları: " +
                        $"Şəhər: {weatherResult.City}, " +
                        $"Temperatur: {weatherResult.Temperature}°C, " +
                        $"Hava vəziyyəti: {weatherResult.Condition}, " +
                        $"Rütubət: %{weatherResult.Humidity}, " +
                        $"Küləyin sürəti: {weatherResult.WindSpeed} km/s. " +
                        $"Bu məlumatlara əsasən istifadəçiyə səyahət və geyim tövsiyələri ver.";
                    break;

                case TravelIntent.Restaurant:
                    intentInstruction =
                        "İstifadəçi restoran tövsiyəsi istəyir.";
                    break;

                case TravelIntent.Hotel:
                    intentInstruction =
                        "İstifadəçi otel tövsiyəsi istəyir.";
                    break;

                default:
                    intentInstruction =
                        "İstifadəçinin səyahətlə bağlı sualına kömək et.";
                    break;
            }

            var finalPrompt = _promptBuilder.BuildPrompt(
                $"{intentInstruction}\n\nİstifadəçinin əsl sualı:\n{prompt}");

            var result = await _groqService.GetResponseAsync(finalPrompt);

            result.Intent = intent.ToString();

            return result;
        }
    }
}
