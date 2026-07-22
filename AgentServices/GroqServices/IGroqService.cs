namespace FlightBooking.AgentServices.GroqServices
{
    public interface IGroqService
    {
        Task<string> GetResponseAsync(string prompt);
    }
}
