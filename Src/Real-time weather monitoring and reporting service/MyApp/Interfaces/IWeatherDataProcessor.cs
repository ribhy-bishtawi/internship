using WeatherSys.Models;
namespace WeatherSys.Interfaces;
public interface IWeatherDataProcessor
{
    void ProcessInput(IWeatherData weatherData, string input);

}

