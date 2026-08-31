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
                "cities.json file was not found.",
                filePath);
        }

        var json = await File.ReadAllTextAsync(filePath);

        var cityData = JsonSerializer.Deserialize<CityData>(json);

        return cityData?.List ?? new List<City>();
    }
}