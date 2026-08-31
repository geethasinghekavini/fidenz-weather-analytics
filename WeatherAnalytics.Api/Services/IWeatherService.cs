using Microsoft.AspNetCore.Mvc;
using WeatherAnalytics.Api.Models;

namespace WeatherAnalytics.Api.Services
{
    public interface IWeatherService
    {
        Task<WeatherData?> GetWeatherAsync(string cityCode);
    }
}
