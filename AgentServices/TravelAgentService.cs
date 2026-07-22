using FlightBooking.AgentServices.GroqServices;

namespace FlightBooking.AgentServices
{
    public class TravelAgentService : ITravelAgentService
    {
        private readonly IGroqService _geminiService;

        public TravelAgentService(IGroqService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<string> AskAgentAsync(string prompt)
        {
            return await _geminiService.GetResponseAsync(prompt);
        }
    }
}
