using Microsoft.AspNetCore.Mvc;

namespace WeatherAnalytics.Api.Services
{
    public interface IComfortIndexService
    {
        double Calculate(
            double temperature,
            int humidity,
            double windSpeed,
            int cloudiness);
    }
}
