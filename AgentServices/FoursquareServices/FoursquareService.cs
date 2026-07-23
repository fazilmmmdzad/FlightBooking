using FlightBooking.AgentSettings;
using FlightBooking.Dtos.RestaurantDtos;
using FlightBooking.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FlightBooking.AgentServices.FoursquareServices;

public class FoursquareService : IFoursquareService
{
    private readonly HttpClient _httpClient;
    private readonly FoursquareSettings _settings;

    public FoursquareService(
        HttpClient httpClient,
        IOptions<FoursquareSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<List<RestaurantDto>> SearchRetaurantsAsync(string query)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", _settings.ApiKey);

        var url =
            $"https://api.foursquare.com/v3/places/search?query={Uri.EscapeDataString(query)}&categories=13065&limit=10";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<RestaurantDto>();

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);

        var restaurants = new List<RestaurantDto>();

        if (!document.RootElement.TryGetProperty("results", out var results))
            return restaurants;

        foreach (var item in results.EnumerateArray())
        {
            string placeId = item.TryGetProperty("fsq_id", out var id)
                ? id.GetString() ?? ""
                : "";

            string name = item.TryGetProperty("name", out var n)
                ? n.GetString() ?? ""
                : "";

            string address = "";

            if (item.TryGetProperty("location", out var location))
            {
                if (location.TryGetProperty("formatted_address", out var formatted))
                {
                    address = formatted.GetString() ?? "";
                }
            }

            restaurants.Add(new RestaurantDto
            {
                PlaceId = placeId,
                Name = name,
                Address = address,
                Rating = null,
                UserRatingCount = null,
                PriceLevel = null,
                GoogleMapsUrl = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(address)}"
            });
        }

        return restaurants;
    }
}