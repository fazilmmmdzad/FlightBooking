using FlightBooking.Dtos.RestaurantDtos;

namespace FlightBooking.AgentServices.FoursquareServices
{
    public interface IFoursquareService
    {
        Task<List<RestaurantDto>> SearchRetaurantsAsync(string query);
    }
}
