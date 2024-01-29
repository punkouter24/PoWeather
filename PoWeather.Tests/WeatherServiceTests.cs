using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Configuration;
using PoWeather.Services;
using System;

namespace PoWeather.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            // Setup mock configuration values
            _mockConfiguration.SetupGet(x => x["WeatherApi:ApiKey"]).Returns("910703a90913434fa8f174503242501");
            _mockConfiguration.SetupGet(x => x["WeatherApi:BaseUrl"]).Returns("http://api.weatherapi.com/v1");

            _weatherService = new WeatherService(new HttpClient(), _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsCorrectWeatherData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"current\": {\"temp_c\": 20.0, \"temp_f\": 68.0}}"), // Mock JSON response
                });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://api.weatherapi.com/v1") // Ensure this matches your mock setup
            };

            var service = new WeatherService(client, _mockConfiguration.Object);

            // Act
            var result = await service.GetWeatherAsync("12345");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(20.0, result.TempC);
            Assert.Equal(68.0, result.TempF);
            // Additional assertions...
        }
    }
}
