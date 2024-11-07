using NetLah.Extensions.ClockProvider;

var builder = WebApplication.CreateBuilder(args);

builder.AddLegacyClockProvider(true);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var clockProvider = app.Services.GetRequiredService<IClockProvider>();

#if NET8_0_OR_GREATER

if (clockProvider.Provider is TimeProvider provider)
{
    Console.WriteLine($"ClockProvider underlying provider: {provider.GetType().FullName}");
}
else
{
    Console.WriteLine($"ClockProvider underlying provider is unknown: {clockProvider.Provider?.GetType().FullName}");
}

#else

if (clockProvider.Provider is Microsoft.AspNetCore.Authentication.ISystemClock systemClock)
{
    Console.WriteLine($"ClockProvider underlying provider: {systemClock.GetType().FullName}");
}
else
{
    Console.WriteLine($"ClockProvider underlying provider is unknown: {clockProvider.Provider?.GetType().FullName}");
}

#endif

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
