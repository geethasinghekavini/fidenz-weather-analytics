using Microsoft.AspNetCore.Mvc;

namespace WeatherAnalytics.Api.Models
{
    public class WeatherData
    {
        public MainWeather Main { get; set; } = new();
        public WindWeather Wind { get; set; } = new();
        public CloudsWeather Clouds { get; set; } = new();
        public List<WeatherDescription> Weather { get; set; } = new();
        public int Visibility { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class MainWeather
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
    }

    public class WindWeather
    {
        public double Speed { get; set; }
    }

    public class CloudsWeather
    {
        public int All { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
