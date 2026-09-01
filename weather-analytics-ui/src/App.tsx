import { useEffect, useMemo, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import "./App.css";

import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
} from "recharts";

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

const API_URL = "https://localhost:7278/api/Weather";

function getWeatherIcon(description: string) {
    const value = description.toLowerCase();

    if (value.includes("rain")) return "🌧️";
    if (value.includes("cloud")) return "☁️";
    if (value.includes("clear")) return "☀️";
    if (value.includes("snow")) return "❄️";
    if (value.includes("mist") || value.includes("fog")) return "🌫️";

    return "🌤️";
}

function App() {
    const {
        isAuthenticated,
        isLoading: authLoading,
        loginWithRedirect,
        logout,
        user,
        getAccessTokenSilently,
    } = useAuth0();

    const [weather, setWeather] = useState<WeatherData[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [search, setSearch] = useState("");
    const [sortBy, setSortBy] = useState("rank");
    const [darkMode, setDarkMode] = useState(false);
    const fetchWeather = async () => {
        try {
            setLoading(true);
            setError("");

            const accessToken = await getAccessTokenSilently();

            const response = await fetch(API_URL, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            if (!response.ok) {
                throw new Error(
                    `Failed to load weather data. Status: ${response.status}`
                );
            }

            const data: WeatherData[] = await response.json();

            setWeather(data);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Something went wrong."
            );
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            fetchWeather();
        }
    }, [isAuthenticated]);

    const filteredWeather = useMemo(() => {
        const result = weather.filter((city) =>
            city.cityName
                .toLowerCase()
                .includes(search.toLowerCase())
        );

        return [...result].sort((a, b) => {
            if (sortBy === "temperature") {
                return b.temperature - a.temperature;
            }

            if (sortBy === "score") {
                return b.comfortScore - a.comfortScore;
            }

            return a.rank - b.rank;
        });
    }, [weather, search, sortBy]);

    if (authLoading) {
        return (
            <div className="app">
                <div className="message">
                    <div className="loader"></div>
                    <p>Checking authentication...</p>
                </div>
            </div>
        );
    }

    if (!isAuthenticated) {
        return (
            <div className="app">
                <main className="dashboard">
                    <section className="message">
                        <h2>Weather Analytics</h2>
                        <p>Please log in to access the weather dashboard.</p>

                        <button onClick={() => loginWithRedirect()}>
                            Login
                        </button>
                    </section>
                </main>
            </div>
        );
    }

    return (

     
        <div className={`app ${darkMode ? "dark" : ""}`}>
            <header className="header">
                <div className="brand">
                    <div className="brand-icon">WA</div>

                    <div>
                        <h1>Weather Analytics</h1>
                        <p>Global weather comfort dashboard</p>
                    </div>
                </div>

                <div className="header-actions">
                    <button
                        className="icon-button"
                        onClick={() => setDarkMode(!darkMode)}
                        title="Toggle dark mode"
                    >
                        {darkMode ? "☀️" : "🌙"}
                    </button>

                    <button
                        className="refresh-button"
                        onClick={fetchWeather}
                    >
                        ↻ Refresh
                    </button>

                    {user?.email && (
                        <span className="user-email">
                            {user.email}
                        </span>
                    )}

                    <button
                        className="refresh-button"
                        onClick={() =>
                            logout({
                                logoutParams: {
                                    returnTo: window.location.origin,
                                },
                            })
                        }
                    >
                        Logout
                    </button>
                </div>
            </header>

            <main className="dashboard">

                

                <section className="summary">

                    <div className="summary-card">
                      
                        <div>
                            <span>Total Cities</span>
                            <strong>{weather.length}</strong>
                        </div>
                    </div>

                    <div className="summary-card highlight">
                        
                        <div>
                            <span>Most Comfortable</span>
                            <strong>
                                {weather[0]?.cityName ?? "-"}
                            </strong>
                        </div>
                    </div>

                    <div className="summary-card">
                        
                        <div>
                            <span>Top Comfort Score</span>
                            <strong>
                                {weather[0]?.comfortScore?.toFixed(1) ?? "-"}
                            </strong>
                        </div>
                    </div>

                </section>

                <div className="toolbar">

                    <div className="search-box">
                        🔎
                        <input
                            type="text"
                            placeholder="Search city..."
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                        />
                    </div>

                    <select
                        value={sortBy}
                        onChange={(e) => setSortBy(e.target.value)}
                    >
                        <option value="rank">Sort: Comfort Ranking</option>
                        <option value="score">Sort: Comfort Score</option>
                        <option value="temperature">
                            Sort: Temperature
                        </option>
                    </select>

                </div>

                {loading && (
                    <div className="message">
                        <div className="loader"></div>
                        <p>Loading weather data...</p>
                    </div>
                )}

                {error && (
                    <div className="error">
                        <h3>Unable to load weather data</h3>
                        <p>{error}</p>

                        <button onClick={fetchWeather}>
                            Try Again
                        </button>
                    </div>
                )}

                {!loading && !error && (
                    <>
                        <div className="section-title">
                            <div>
                                <h3>City Rankings</h3>
                                <p>
                                    {filteredWeather.length} cities
                                    displayed
                                </p>
                            </div>
                        </div>

                        <section className="weather-grid">
                            <section className="chart-section">
                                <div className="section-title">
                                    <div>
                                        <h3>Comfort Score Comparison</h3>
                                        <p>Comfort index by city</p>
                                    </div>
                                </div>

                                <div className="chart-container">
                                    <ResponsiveContainer width="100%" height={300}>
                                        <BarChart
                                            data={filteredWeather}
                                            layout="vertical"
                                            margin={{
                                                top: 5,
                                                right: 20,
                                                left: 20,
                                                bottom: 5,
                                            }}
                                        >
                                            <CartesianGrid
                                                strokeDasharray="3 3"
                                                horizontal={false}
                                                stroke={darkMode ? "#343940" : "#e1e4e8"}
                                            />

                                            <XAxis
                                                type="number"
                                                domain={[0, 100]}
                                                tick={{ fontSize: 11, fill: darkMode ? "#c7ccd2" : "#59616a" }}
                                                axisLine={{ stroke: darkMode ? "#4a5058" : "#d5d9de" }}
                                                tickLine={{ stroke: darkMode ? "#4a5058" : "#d5d9de" }}
                                            />

                                            <YAxis
                                                type="category"
                                                dataKey="cityName"
                                                width={90}
                                                tick={{
                                                    fontSize: 11,
                                                    fill: darkMode ? "#c7ccd2" : "#59616a",
                                                }}
                                                axisLine={{ stroke: darkMode ? "#4a5058" : "#d5d9de" }}
                                                tickLine={{ stroke: darkMode ? "#4a5058" : "#d5d9de" }}
                                            />

                                            <Tooltip
                                                formatter={(value) => [
                                                    `${Number(value).toFixed(1)}`,
                                                    "Comfort Score",
                                                ]}
                                            />

                                            <Bar
                                                dataKey="comfortScore"
                                                radius={[0, 4, 4, 0]}
                                                barSize={20}
                                                fill={darkMode ? "#d5d9de" : "#343a40"}
                                            />

                                        </BarChart>
                                    </ResponsiveContainer>
                                </div>
                            </section>

                            {filteredWeather.map((city) => (

                                <article
                                    className={`weather-card ${city.rank <= 3 ? "top-card" : ""
                                        }`}
                                    key={city.cityCode}
                                >

                                    <div className="card-header">

                                        <div className="city-info">

                                            <div className="weather-icon">
                                                {getWeatherIcon(
                                                    city.weatherDescription
                                                )}
                                            </div>

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
                                            <span>💧 Humidity</span>
                                            <strong>
                                                {city.humidity}%
                                            </strong>
                                        </div>

                                        <div>
                                            <span>💨 Wind</span>
                                            <strong>
                                                {city.windSpeed.toFixed(1)} m/s
                                            </strong>
                                        </div>

                                        <div>
                                            <span>☁️ Clouds</span>
                                            <strong>
                                                {city.cloudiness}%
                                            </strong>
                                        </div>

                                        <div>
                                            <span>📊 Pressure</span>
                                            <strong>
                                                {city.pressure} hPa
                                            </strong>
                                        </div>

                                    </div>

                                </article>

                            ))}

                        </section>
                    </>
                )}

            </main>

            <footer>
                Weather Analytics • Powered by OpenWeatherMap
            </footer>

        </div>
    );
}

export default App;