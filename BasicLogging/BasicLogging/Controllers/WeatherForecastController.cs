using Microsoft.AspNetCore.Mvc;

namespace BasicLogging.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Use the Log() method
        //_logger.Log(LogLevel.Information, "This is a logging message.");
        // Use the LogInformation method
        logger.LogInformation("This is a logging message.");
        // Use the LogTrace() method
        logger.LogTrace("This is a trace message");
        // Use the event id
        logger.LogInformation(EventIds.LoginEvent, "This is a logging message with event id.");
        logger.LogInformation("This is a logging message with args: Today is {Week}. It is {Time}.",
            DateTime.Now.DayOfWeek, DateTime.Now.ToLongTimeString());
        try
        {
            // Omitted for brevity
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "This is a logging message with exception.");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}