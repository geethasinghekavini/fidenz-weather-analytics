using System.Text.Json;
using WeatherAnalytics.Api.Models;

namespace WeatherAnalytics.Api.Services;

public class CityService : ICityService
{
    private readonly IWebHostEnvironment _environment;

    public CityService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<List<City>> GetCitiesAsync()
    {
        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "Data",
            "cities.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "The cities.json file could not be found.",
                filePath);
        }

        await using var stream = File.OpenRead(filePath);

        var cityList = await JsonSerializer.DeserializeAsync<CityList>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return cityList?.List ?? new List<City>();
    }
}