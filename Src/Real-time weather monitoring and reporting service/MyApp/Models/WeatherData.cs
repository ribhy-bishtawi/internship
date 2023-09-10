using System.Xml.Serialization;
using System.Collections.Generic;
using WeatherSys.Interfaces;

namespace WeatherSys.Models;
public class WeatherData : IWeatherData
{
    public string? Location { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }

    [XmlIgnore]
    public IInputStrategy? InputStrategy { get; set; }
    [XmlIgnore]
    private readonly IWeatherObserverManager observerManager;
    private WeatherData()
    {
    }

    public WeatherData(IWeatherObserverManager observerManager)
    {
        this.observerManager = observerManager;
    }

    public void ProcessInput(string input)
    {
        WeatherData weatherData = InputStrategy!.ParseInput(input);
        Location = weatherData.Location;
        Temperature = weatherData.Temperature;
        Humidity = weatherData.Humidity;
        observerManager.NotifyObservers(this);
    }
}