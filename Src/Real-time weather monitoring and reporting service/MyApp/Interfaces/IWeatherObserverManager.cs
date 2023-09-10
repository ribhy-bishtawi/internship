namespace WeatherSys.Interfaces;
public interface IWeatherObserverManager
{
    void Subscribe(IWeatherObserver observer);
    void Unsubscribe(IWeatherObserver observer);
    void NotifyObservers(IWeatherData weatherData);
}
