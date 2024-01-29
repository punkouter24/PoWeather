using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PoWeather.Data;
using Microsoft.Extensions.Configuration;

namespace PoWeather.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var weatherApiConfig = configuration.GetSection("WeatherApi");
            _apiKey = weatherApiConfig.GetValue<string>("ApiKey");
            _baseUrl = weatherApiConfig.GetValue<string>("BaseUrl");
        }

        public async Task<WeatherInfo> GetWeatherAsync(string zipCode)
        {
            var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>($"{_baseUrl}/current.json?key={_apiKey}&q={zipCode}");
            return response?.Current;
        }
    }
}
