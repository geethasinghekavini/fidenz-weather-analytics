using Microsoft.AspNetCore.Mvc;

namespace WeatherAnalytics.Api.Services
{
    public interface IComfortIndexService
    {
        double Calculate(double temperature, double humidity, double windSpeed, double cloudiness);
    }
}
