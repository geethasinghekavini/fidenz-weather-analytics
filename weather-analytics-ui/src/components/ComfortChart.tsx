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

interface ComfortChartProps {
    data: WeatherData[];
}

function ComfortChart({ data }: ComfortChartProps) {
    return (
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
                        data={data}
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
                        />

                        <XAxis
                            type="number"
                            domain={[0, 100]}
                            tick={{ fontSize: 11 }}
                        />

                        <YAxis
                            type="category"
                            dataKey="cityName"
                            width={80}
                            tick={{ fontSize: 11 }}
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
                            barSize={18}
                        />
                    </BarChart>
                </ResponsiveContainer>
            </div>
        </section>
    );
}

export default ComfortChart;