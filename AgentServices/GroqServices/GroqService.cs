using FlightBooking.AgentSettings;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FlightBooking.AgentServices.GroqServices
{
    public class GroqService : IGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly GroqSettings _settings;

        public GroqService(HttpClient httpClient, IOptions<GroqSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = string.IsNullOrWhiteSpace(_settings.Model)
                    ? "llama-3.1-8b-instant"
                    : _settings.Model,

                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = "Sən bir səyahət və restoran təklifləri üzrə asistansan. Qısa, dəqiq və istifadəçi dostu cavab ver."
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },

                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions");

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    _settings.ApiKey.Trim());

            var response = await _httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Groq API xətası ({response.StatusCode}): {responseContent}";
            }

            using var document = JsonDocument.Parse(responseContent);

            return document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?? "Cavab alına bilmədi.";
        }
    }
}