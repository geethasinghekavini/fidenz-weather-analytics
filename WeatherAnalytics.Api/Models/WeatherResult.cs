namespace WeatherAnalytics.Api.Models;

public class WeatherResult
{
    public string CityCode { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int Cloudiness { get; set; }
    public int Pressure { get; set; }
    public int Visibility { get; set; }
    public string WeatherDescription { get; set; } = string.Empty;
    public double ComfortScore { get; set; }
    public int Rank { get; set; }
}