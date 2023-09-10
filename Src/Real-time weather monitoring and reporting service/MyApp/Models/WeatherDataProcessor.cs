using System.Xml.Serialization;
using System.Collections.Generic;
using WeatherSys.Interfaces;

namespace WeatherSys.Models;
public class WeatherDataProcessor : IWeatherDataProcessor
{
    [XmlIgnore]
    private readonly IWeatherObserverManager observerManager;
    private WeatherDataProcessor()
    {
    }

    public WeatherDataProcessor(IWeatherObserverManager observerManager)
    {
        this.observerManager = observerManager;
    }

    public void ProcessInput(IWeatherData weatherData, string input)
    {
        WeatherData processedWeatherData = weatherData.InputStrategy!.ParseInput(input);
        weatherData.Location = processedWeatherData.Location;
        weatherData.Temperature = processedWeatherData.Temperature;
        weatherData.Humidity = processedWeatherData.Humidity;
        observerManager.NotifyObservers(weatherData);
    }
}