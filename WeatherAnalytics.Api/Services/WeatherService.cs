using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using WeatherAnalytics.Api.Models;


namespace WeatherAnalytics.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IComfortIndexService _comfortIndexService;
    private readonly IMemoryCache _cache;
    private readonly CacheStatusService _cacheStatusService;

    public WeatherService(
    HttpClient httpClient,
    IConfiguration configuration,
    IMemoryCache cache,
    CacheStatusService cacheStatusService,
    IComfortIndexService comfortIndexService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _cache = cache;
        _cacheStatusService = cacheStatusService;
        _comfortIndexService = comfortIndexService;
    }

    public async Task<WeatherData?> GetWeatherAsync(string cityCode)
    {
        var cacheKey = $"weather_{cityCode}";

        if (_cache.TryGetValue(cacheKey, out WeatherData? cachedWeather))
        {
            _cacheStatusService.SetStatus(cityCode, "HIT");
            return cachedWeather;
        }

        _cacheStatusService.SetStatus(cityCode, "MISS");
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

        var weatherData =
    await response.Content.ReadFromJsonAsync<WeatherData>();

        if (weatherData != null)
        {
            _cache.Set(
                cacheKey,
                weatherData,
                TimeSpan.FromMinutes(5));
        }

        return weatherData;
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

            if (weather.Main == null ||
                weather.Wind == null ||
                weather.Clouds == null)
            {
                throw new InvalidOperationException(
                    $"Incomplete weather data received for city code {city.CityCode}.");
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
    public Dictionary<string, string> GetCacheStatus()
    {
        return _cacheStatusService.GetStatus();
    }
}