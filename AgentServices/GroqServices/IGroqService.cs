using FlightBooking.Dtos.AgentDtos;

namespace FlightBooking.AgentServices.GroqServices
{
    public interface IGroqService
    {
        Task<AgentResponseDto> GetResponseAsync(string prompt);
    }
}
