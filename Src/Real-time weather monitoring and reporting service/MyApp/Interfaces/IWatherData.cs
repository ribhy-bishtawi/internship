using WeatherSys.Interfaces;
using System.Xml.Serialization;
namespace WeatherSys.Interfaces;
public interface IWeatherData
{
    string? Location { get; set; }
    double? Temperature { get; set; }
    double? Humidity { get; set; }
    void ProcessInput(string input);
    IInputStrategy? InputStrategy { get; set; }



}

