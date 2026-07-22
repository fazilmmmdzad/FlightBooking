using FlightBooking.AgentServices.GroqServices;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.Dtos.AgentDtos;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IGroqService _groqService;
        private readonly ITravelPromptBuilder _promptBuilder;

        public TravelAgentService(IGroqService groqService, ITravelPromptBuilder promptBuilder)
        {
            _groqService = groqService;
            _promptBuilder = promptBuilder;
        }

        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            var finalPrompt = _promptBuilder.BuildPrompt(prompt);
            return await _groqService.GetResponseAsync(finalPrompt);
        }
    }
}
