namespace WeatherSys.Models;
public class BotConfiguration
{

    public bool? Enabled { get; set; }
    public int? HumidityThreshold { get; set; }
    public int? TemperatureThreshold { get; set; }

    public string? Message { get; set; }
}