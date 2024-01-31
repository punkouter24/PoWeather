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
            // Construct the endpoint with query parameters
            string requestUri = $"current.json?key={_apiKey}&q={zipCode}";

            // Combine the base URL with the endpoint to get the full URL
            Uri fullUri = new Uri(_httpClient.BaseAddress, requestUri);

            // Now you can log or inspect the full URL
            Console.WriteLine($"Requesting Weather API: {fullUri}");

            // Make the HTTP request
            var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(requestUri);

            return response?.Current;
        }
    }
}
