using WeatherSys.Models;

namespace WeatherSys.Interfaces;
public interface IInputStrategy
{
    WeatherData ParseInput(string input);
}

