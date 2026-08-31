using System.Net;
using System.Net.Http.Json;
using WeatherAnalytics.Api.Models;

namespace WeatherAnalytics.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IComfortIndexService _comfortIndexService;

    public WeatherService(
    HttpClient httpClient,
    IConfiguration configuration,
    IComfortIndexService comfortIndexService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _comfortIndexService = comfortIndexService;
    }

    public async Task<WeatherData?> GetWeatherAsync(string cityCode)
    {
        var apiKey = _configuration["OpenWeatherMap:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenWeatherMap API key is not configured.");
        }

        var url = $"weather?id={cityCode}&appid={apiKey}&units=metric";

        using var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new InvalidOperationException("OpenWeatherMap API key is invalid or unauthorized.");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException( $"OpenWeatherMap request failed with status code {(int)response.StatusCode}.");
        }

        return await response.Content.ReadFromJsonAsync<WeatherData>();
    }

    public async Task<List<WeatherResult>> GetWeatherForCitiesAsync(
    List<City> cities)
    {
        var results = new List<WeatherResult>();

        foreach (var city in cities)
        {
            var weather = await GetWeatherAsync(city.CityCode);

            if (weather == null)
            {
                continue;
            }

            var comfortScore = _comfortIndexService.Calculate(
             weather.Main.Temp,
             weather.Main.Humidity,
             weather.Wind.Speed,
             weather.Clouds.All);

            results.Add(new WeatherResult
            {
                CityCode = city.CityCode,
                CityName = city.CityName,
                Temperature = weather.Main.Temp,
                Humidity = weather.Main.Humidity,
                WindSpeed = weather.Wind.Speed,
                Cloudiness = weather.Clouds.All,
                Pressure = weather.Main.Pressure,
                Visibility = weather.Visibility,
                WeatherDescription =
                    weather.Weather.FirstOrDefault()?.Description
                    ?? "Unknown",
                ComfortScore = comfortScore
            });
        }

        var rankedResults = results
    .OrderByDescending(x => x.ComfortScore)
    .ToList();

        for (var i = 0; i < rankedResults.Count; i++)
        {
            rankedResults[i].Rank = i + 1;
        }

        return rankedResults;
    }
}