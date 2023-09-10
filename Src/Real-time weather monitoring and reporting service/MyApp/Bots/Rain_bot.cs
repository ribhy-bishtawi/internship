namespace WeatherSys.Bots;

using System;
using System.Collections.Generic;
using WeatherSys.Interfaces;

public class RainBot : IWeatherObserver
{
    public bool? Enabled { get; set; }
    public int? HumidityThreshold { get; set; }
    public string? Message { get; set; }

    public void Update(IWeatherData weatherData)
    {
        if (weatherData.Humidity > HumidityThreshold)
        {
            Console.WriteLine("RainBot activated!");
            Console.WriteLine($"RainBot: \"{Message}\" ");
        }
    }
}
