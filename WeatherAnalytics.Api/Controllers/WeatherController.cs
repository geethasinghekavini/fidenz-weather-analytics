using Microsoft.AspNetCore.Mvc;
using WeatherAnalytics.Api.Services;

namespace WeatherAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{cityCode}")]
    public async Task<IActionResult> GetWeather(string cityCode)
    {
        var weather = await _weatherService.GetWeatherAsync(cityCode);

        if (weather is null)
        {
            return NotFound();
        }

        return Ok(weather);
    }
}