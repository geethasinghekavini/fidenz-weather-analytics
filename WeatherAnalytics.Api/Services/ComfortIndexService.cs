namespace WeatherAnalytics.Api.Services;

public class ComfortIndexService : IComfortIndexService
{
    public double Calculate(
        double temperature,
        double humidity,
        double windSpeed,
        double cloudiness)
    {
        // Temperature: ideal around 22°C
        var temperatureScore =
            Math.Max(0, 100 - Math.Abs(temperature - 22) * 5);

        // Humidity: ideal around 50%
        var humidityScore =
            Math.Max(0, 100 - Math.Abs(humidity - 50) * 2);

        // Wind: light wind is comfortable
        var windScore =
            Math.Max(0, 100 - Math.Abs(windSpeed - 2) * 15);

        // Moderate cloudiness is acceptable
        var cloudScore =
            Math.Max(0, 100 - Math.Abs(cloudiness - 30) * 1.2);

        var score =
            (temperatureScore * 0.40) +
            (humidityScore * 0.30) +
            (windScore * 0.15) +
            (cloudScore * 0.15);

        return Math.Round(Math.Clamp(score, 0, 100), 2);
    }
}