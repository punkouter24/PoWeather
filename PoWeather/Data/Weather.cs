using System.Text.Json.Serialization;

namespace PoWeather.Data
{
    public class WeatherResponse
    {
        public CurrentWeather Current { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temp_c")]
        public float TempC { get; set; }

        [JsonPropertyName("temp_f")]
        public float TempF { get; set; }
        // ... other relevant fields
    }
}
