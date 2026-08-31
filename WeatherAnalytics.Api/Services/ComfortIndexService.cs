using Microsoft.AspNetCore.Mvc;

namespace WeatherAnalytics.Api.Services
{
    public class ComfortIndexService : IComfortIndexService
    {
        public double Calculate(
            double temperature,
            int humidity,
            double windSpeed,
            int cloudiness)
        {
            var temperatureScore =
                Clamp(100 - Math.Abs(temperature - 22) * 5);

            var humidityScore =
                Clamp(100 - Math.Abs(humidity - 50) * 2);

            var windScore =
                Clamp(100 - Math.Abs(windSpeed - 2) * 15);

            var cloudScore =
                Clamp(100 - Math.Abs(cloudiness - 30) * 1.5);

            var comfortIndex =
                temperatureScore * 0.40 +
                humidityScore * 0.30 +
                windScore * 0.15 +
                cloudScore * 0.15;

            return Math.Round(
                Clamp(comfortIndex),
                2);
        }

        private static double Clamp(double value)
        {
            return Math.Clamp(value, 0, 100);
        }
    }
}

