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


}