using FlightBooking.AgentServices.GroqServices;
using FlightBooking.Dtos.AgentDtos;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IGroqService _geminiService;

        public TravelAgentService(IGroqService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<AgentResponseDto> AskAgentAsync(string prompt)
        {
            return await _geminiService.GetResponseAsync(prompt);
        }
    }
}
