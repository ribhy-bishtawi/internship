using System.Xml.Serialization;
using System.Collections.Generic;
using WeatherSys.Interfaces;

namespace WeatherSys.Models;
public class WeatherObserverManager : IWeatherObserverManager
{
    private List<IWeatherObserver> observers = new List<IWeatherObserver>();

    public void Subscribe(IWeatherObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IWeatherObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(IWeatherData weatherData)
    {
        foreach (var observer in observers)
        {
            observer.Update(weatherData);
        }
    }
}






