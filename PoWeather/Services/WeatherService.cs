using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PoWeather.Data;

namespace PoWeather.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _httpClient.BaseAddress = new Uri(configuration["WeatherApi:BaseUrl"]);
        }

        public async Task<CurrentWeather> GetWeatherAsync(string zipCode)
        {
            var response = await _httpClient.GetFromJsonAsync<WeatherResponse>($"current.json?key={_apiKey}&q={zipCode}");
            return response?.Current;
        }
    }
}
