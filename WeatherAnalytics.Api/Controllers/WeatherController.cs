using Microsoft.AspNetCore.Mvc;
using WeatherAnalytics.Api.Services;

namespace WeatherAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ICityService _cityService;

    public WeatherController(
        IWeatherService weatherService,
        ICityService cityService)
    {
        _weatherService = weatherService;
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather()
    {
        var cities = await _cityService.GetCitiesAsync();

        var weather =
            await _weatherService.GetWeatherForCitiesAsync(cities);

        return Ok(weather);
    }

    [HttpGet("{cityCode}")]
    public async Task<IActionResult> GetWeather(string cityCode)
    {
        var weather =
            await _weatherService.GetWeatherAsync(cityCode);

        if (weather == null)
        {
            return NotFound();
        }

        return Ok(weather);
    }

    [HttpGet("cache-status")]
    public IActionResult GetCacheStatus()
    {
        return Ok(_weatherService.GetCacheStatus());
    }
}