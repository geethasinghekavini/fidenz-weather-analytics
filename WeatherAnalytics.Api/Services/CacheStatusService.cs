namespace WeatherAnalytics.Api.Services;

public class CacheStatusService
{
    private readonly Dictionary<string, string> _status = new();

    public void SetStatus(string cityCode, string status)
    {
        _status[cityCode] = status;
    }

    public Dictionary<string, string> GetStatus()
    {
        return new Dictionary<string, string>(_status);
    }
}