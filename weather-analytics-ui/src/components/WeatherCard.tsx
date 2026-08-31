interface WeatherData {
    cityCode: string;
    cityName: string;
    temperature: number;
    humidity: number;
    windSpeed: number;
    cloudiness: number;
    pressure: number;
    visibility: number;
    weatherDescription: string;
    comfortScore: number;
    rank: number;
}

interface WeatherCardProps {
    city: WeatherData;
}

function getWeatherIcon(description: string) {
    const value = description.toLowerCase();

    if (value.includes("rain")) return "Rain";
    if (value.includes("cloud")) return "Cloudy";
    if (value.includes("clear")) return "Clear";
    if (value.includes("snow")) return "Snow";
    if (value.includes("mist") || value.includes("fog")) return "Mist";

    return "Weather";
}

function WeatherCard({ city }: WeatherCardProps) {
    return (
        <article
            className={`weather-card ${city.rank <= 3 ? "top-card" : ""
                }`}
        >
            <div className="card-header">

                <div className="city-info">

                    <div>
                        <span className="rank">
                            RANK #{city.rank}
                        </span>

                        <h3>{city.cityName}</h3>

                        <p>
                            {city.weatherDescription}
                        </p>
                    </div>

                </div>

                <div className="score">

                    <span>Comfort</span>

                    <strong>
                        {city.comfortScore.toFixed(1)}
                    </strong>

                    <small>/100</small>

                </div>

            </div>

            <div className="score-bar">
                <div
                    style={{
                        width: `${city.comfortScore}%`,
                    }}
                />
            </div>

            <div className="temperature">
                {city.temperature.toFixed(1)}
                <span>°C</span>
            </div>

            <div className="weather-details">

                <div>
                    <span>Humidity</span>
                    <strong>
                        {city.humidity}%
                    </strong>
                </div>

                <div>
                    <span>Wind</span>
                    <strong>
                        {city.windSpeed.toFixed(1)} m/s
                    </strong>
                </div>

                <div>
                    <span>Clouds</span>
                    <strong>
                        {city.cloudiness}%
                    </strong>
                </div>

                <div>
                    <span>Pressure</span>
                    <strong>
                        {city.pressure} hPa
                    </strong>
                </div>

            </div>

        </article>
    );
}

export default WeatherCard;