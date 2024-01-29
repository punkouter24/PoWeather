// namespace PoWeather.Tests;

// using PoWeather;

// // Example of an integration test
// public class WeatherIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly CustomWebApplicationFactory<Program> _factory;

//     public WeatherIntegrationTests(CustomWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//     }

//     [Fact]
//     public async Task HomePage_ShowsWeather()
//     {
//         // Arrange
//         var client = _factory.CreateClient();

//         // Act
//         var response = await client.GetAsync("/");

//         // Assert
//         response.EnsureSuccessStatusCode();
//         var responseString = await response.Content.ReadAsStringAsync();
//         Assert.Contains("Weather Dashboard", responseString);
//         // Additional assertions...
//     }
// }

