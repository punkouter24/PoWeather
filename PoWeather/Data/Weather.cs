namespace PoWeather.Data
{
    public class WeatherInfo
    {
        public float TempC { get; set; }
    }

    public class WeatherApiResponse
    {
        public WeatherInfo Current { get; set; }
    }
}
