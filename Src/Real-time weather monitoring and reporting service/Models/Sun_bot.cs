
using System;
using System.Collections.Generic;
public class SunBot : IWeatherObserver
{
    public bool? Enabled { get; set; }
    public int? TemperatureThreshold { get; set; }
    public string? Message { get; set; }

    public void Update(WeatherData weatherData)
    {
        if (weatherData.Temperature > TemperatureThreshold)
        {
            Console.WriteLine("SunBot activated!");
            Console.WriteLine($"SunBot: \"{Message}\" ");
        }
    }
}
