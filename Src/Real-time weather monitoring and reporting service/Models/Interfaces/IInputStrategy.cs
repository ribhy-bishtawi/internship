namespace WatherSys.Models.Interfaces;
public interface IInputStrategy
{
    WeatherData ParseInput(string input);
}

