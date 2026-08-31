using Microsoft.AspNetCore.Mvc;
namespace WeatherAnalytics.Api.Models
{
    public class CityData  
    {
        public List<City> List { get; set; } = new();
    }
}
