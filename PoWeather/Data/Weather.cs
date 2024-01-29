namespace PoWeather.Data
{
  public class WeatherResponse
    {
        public CurrentWeather Current { get; set; }
    }

    public class CurrentWeather
    {
        public float TempC { get; set; }
        public float TempF { get; set; }
        // ... other relevant fields
    }
}
