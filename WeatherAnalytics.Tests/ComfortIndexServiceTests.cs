using WeatherAnalytics.Api.Services;

namespace WeatherAnalytics.Tests;

public class ComfortIndexServiceTests
{
    private readonly ComfortIndexService _service = new();

    [Fact]
    public void IdealConditions_ShouldReturnHighScore()
    {
        var result = _service.Calculate(
            temperature: 22,
            humidity: 50,
            windSpeed: 2,
            cloudiness: 30);

        Assert.Equal(100, result);
    }

    [Fact]
    public void Score_ShouldNeverBeBelowZero()
    {
        var result = _service.Calculate(
            temperature: 60,
            humidity: 100,
            windSpeed: 20,
            cloudiness: 100);

        Assert.True(result >= 0);
    }

    [Fact]
    public void Score_ShouldNeverExceedOneHundred()
    {
        var result = _service.Calculate(
            temperature: 22,
            humidity: 50,
            windSpeed: 2,
            cloudiness: 30);

        Assert.True(result <= 100);
    }

    [Fact]
    public void ComfortableConditions_ShouldScoreHigherThanExtremeConditions()
    {
        var comfortable = _service.Calculate(
            temperature: 24,
            humidity: 55,
            windSpeed: 2,
            cloudiness: 30);

        var extreme = _service.Calculate(
            temperature: 40,
            humidity: 95,
            windSpeed: 10,
            cloudiness: 100);

        Assert.True(comfortable > extreme);
    }
}