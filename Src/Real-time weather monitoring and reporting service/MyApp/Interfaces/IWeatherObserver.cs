namespace WeatherSys.Interfaces;
public interface IWeatherObserver
{
    void Update(IWeatherData weatherData);
}
