using System.Text.Json;
using FlightBooking.Dtos.AgentDtos;
using FlightBooking.Dtos.WeatherDtos;
using FlightBooking.Settings;
using Microsoft.Extensions.Options;

namespace FlightBooking.Tools.WeatherTool
{
    public class WeatherTool : IWeatherTool
    {
        private readonly HttpClient _httpClient;
        private readonly RapidApiSettings _settings;

        public WeatherTool(
            HttpClient httpClient,
            IOptions<RapidApiSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<WeatherResult?> GetWeatherAsync(string city)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_settings.WeatherBaseUrl}/weather?location={Uri.EscapeDataString(city)}&format=json&u=c");

            request.Headers.Add("x-rapidapi-key", _settings.ApiKey);
            request.Headers.Add("x-rapidapi-host", _settings.WeatherHost);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument document = JsonDocument.Parse(json);

            var root = document.RootElement;

            var location = root.GetProperty("location");
            var current = root.GetProperty("current_observation");
            var condition = current.GetProperty("condition");

            var result = new WeatherResult
            {
                City = location.GetProperty("city").GetString() ?? city,
                Country = location.GetProperty("country").GetString() ?? "",
                TimeZoneId = location.GetProperty("timezone_id").GetString() ?? "",

                Temperature = condition.GetProperty("temperature").GetDecimal(),
                Condition = condition.GetProperty("text").GetString() ?? "",

                Humidity = current.GetProperty("atmosphere").GetProperty("humidity").GetInt32(),
                Visibility = current.GetProperty("atmosphere").GetProperty("visibility").GetInt32(),
                Pressure = current.GetProperty("atmosphere").GetProperty("pressure").GetInt32(),

                WindSpeed = current.GetProperty("wind").GetProperty("speed").GetDouble(),
                WindDirection = current.GetProperty("wind").GetProperty("direction").GetString() ?? "",

                Sunrise = current.GetProperty("astronomy").GetProperty("sunrise").GetString() ?? "",
                Sunset = current.GetProperty("astronomy").GetProperty("sunset").GetString() ?? ""
            };

            if (root.TryGetProperty("forecasts", out var forecasts))
            {
                foreach (var item in forecasts.EnumerateArray())
                {
                    result.Forecasts.Add(new WeatherForecastResult
                    {
                        Day = item.GetProperty("day").GetString() ?? string.Empty,
                        Date = item.GetProperty("date").GetInt64(),
                        Low = item.GetProperty("low").GetInt32(),
                        High = item.GetProperty("high").GetInt32(),
                        Condition = item.GetProperty("text").GetString() ?? string.Empty
                    });
                }
            }

            return result;
        }
    }
}