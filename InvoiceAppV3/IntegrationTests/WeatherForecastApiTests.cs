using System.Text.Json;
using FluentAssertions;
using InvoiceAppV3;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class WeatherForecastApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var client = factory.CreateClient();
        // Act
        var response = await client.GetAsync("/WeatherForecast");
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        // Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        weatherForecast.Should().NotBeNull();
        weatherForecast.Should().HaveCount(5);
    }
}

// In the test method, we first create an instance of the HttpClient class using the
// WebApplicationFactory<T> instance. Then we send an HTTP GET request to the
// /WeatherForecast endpoint. The EnsureSuccessStatusCode method ensures that
// the response has a status code in the 200-299 range. Then we check whether the content type of
// the response is application/json; charset=utf-8. Finally, we deserialize the response
// content to a list of WeatherForecast objects and check whether the list contains five items.