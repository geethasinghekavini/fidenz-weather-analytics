using WeatherAnalytics.Api.Models;

namespace WeatherAnalytics.Api.Services;

public interface ICityService
{
    Task<List<City>> GetCitiesAsync();
}