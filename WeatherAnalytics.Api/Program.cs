using Microsoft.AspNetCore.Authentication.JwtBearer;
using WeatherAnalytics.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-ockmiyyljz4houh1.us.auth0.com/";
        options.Audience = "https://weather-analytics-api";
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IComfortIndexService, ComfortIndexService>();
builder.Services.AddSingleton<CacheStatusService>();

builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri(
        "https://api.openweathermap.org/data/2.5/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();