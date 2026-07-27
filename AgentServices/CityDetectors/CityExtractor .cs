using FlightBooking.AgentSettings;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FlightBooking.AgentServices.CityDetectors
{
    public class GroqCityExtractor : ICityExtractor
    {
        private readonly HttpClient _httpClient;
        private readonly GroqSettings _settings;

        public GroqCityExtractor(HttpClient httpClient, IOptions<GroqSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string?> ExtractCityAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return null;

            var extractionPrompt =
                "Aşağıdakı istifadəçi mesajında keçən şəhər və ya rayon adını müəyyən et. " +
                "Yalnız məkanın adını qaytar. Heç bir izah, durğu işarəsi və ya əlavə mətn yazma. " +
                "Əgər mesajda şəhər və ya rayon adı yoxdursa, yalnız NONE yaz.\n\n" +
                $"İstifadəçi mesajı: {prompt}";

            var requestBody = new
            {
                model = _settings.Model,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Sən istifadəçi mesajlarından şəhər və rayon adlarını müəyyən edən məlumat çıxarma xidmətinin bir hissəsisən."
                    },
                    new
                    {
                        role = "user",
                        content = extractionPrompt
                    }
                },
                temperature = 0
            };

            var json = JsonSerializer.Serialize(requestBody);

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions");

            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            using var document = JsonDocument.Parse(responseContent);

            var city = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?.Trim();

            if (string.IsNullOrWhiteSpace(city) ||
                city.Equals("NONE", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return city;
        }
    }
}