using System.Net;
using System.Net.Http.Json;
using WeatherAnalytics.Api.Models;

namespace WeatherAnalytics.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
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
                    ?? "Unknown"
            });
        }

        return results;
    }
}