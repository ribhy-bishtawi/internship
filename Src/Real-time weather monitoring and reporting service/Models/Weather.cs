using Newtonsoft.Json;
using System.Xml.Serialization;

public class WeatherData
{
    public string? Location { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    private List<IWeatherObserver> observers = new List<IWeatherObserver>();

    public void Subscribe(IWeatherObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IWeatherObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }


}
