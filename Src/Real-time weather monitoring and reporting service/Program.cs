using System.Text.RegularExpressions;
using Newtonsoft.Json;
class Program

{
    static void Main(string[] args)
    {
        string configJson = File.ReadAllText("Configuration.json");
        BotConfigurations botConfigurations = JsonConvert.DeserializeObject<BotConfigurations>(configJson)!;

        RainBot rainBot = new RainBot
        {
            Enabled = botConfigurations.RainBot!.Enabled!,
            HumidityThreshold = botConfigurations.RainBot.HumidityThreshold,
            Message = botConfigurations.RainBot.Message
        };

        SunBot sunBot = new SunBot
        {
            Enabled = botConfigurations.SunBot!.Enabled!,
            TemperatureThreshold = botConfigurations.SunBot.TemperatureThreshold,
            Message = botConfigurations.SunBot.Message
        };

        SnowBot snowBot = new SnowBot
        {
            Enabled = botConfigurations.SnowBot!.Enabled!,
            TemperatureThreshold = botConfigurations.SnowBot.TemperatureThreshold,
            Message = botConfigurations.SnowBot.Message
        };

        Console.WriteLine("Enter weather data: ");
        string? inputData = Console.ReadLine();
        if (inputData == null)
        {
            Console.WriteLine("Please enter a valid format (JSON or XML)");
            return;
        }
        WeatherData weatherData = new WeatherData();
        weatherData.Subscribe(rainBot);
        weatherData.Subscribe(sunBot);
        weatherData.Subscribe(snowBot);

        if (IsJson(inputData))
        {
            weatherData.FromJson(inputData!);
        }
        else if (IsXml(inputData))
        {
            weatherData.FromXml(inputData!);
        }
        else
        {
            Console.WriteLine("Unknown format.");
        }

    }
    static bool IsJson(string input)
    {
        return input.TrimStart().StartsWith("{") && input.TrimEnd().EndsWith("}");
    }

    static bool IsXml(string input)
    {
        return Regex.IsMatch(input.Trim(), @"^<\w+.*?>.*?<\/\w+>$");
    }

}



public class BotConfigurations
{
    public BotConfiguration? RainBot { get; set; }

    public BotConfiguration? SunBot { get; set; }

    public BotConfiguration? SnowBot { get; set; }
}

public class BotConfiguration
{

    public bool? Enabled { get; set; }
    public int? HumidityThreshold { get; set; }
    public int? TemperatureThreshold { get; set; }
    public string? Message { get; set; }
}
