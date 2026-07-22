using FlightBooking.Dtos.AgentDtos;

namespace FlightBooking.AgentServices
{
    public interface ITravelAgentService
    {
       Task<AgentResponseDto> AskAgentAsync(string prompt);
    }
}
