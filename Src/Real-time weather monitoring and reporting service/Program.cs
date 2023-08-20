using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WatherSys.Models;
using WatherSys.Models.Bots;
using WatherSys.Models.Strategies;


class Program
{
    static void Main(string[] args)
    {
        BotConfigurations botConfigurations = LoadConfigrations();
        WeatherData weatherData = InitializeWeatherData(botConfigurations);

        Console.WriteLine("Enter weather data: ");
        string? inputData = Console.ReadLine();
        if (inputData == null)
        {
            Console.WriteLine("Please enter a valid format (JSON or XML)");
            return;
        }

        ProcessInput(weatherData, inputData);

    }
    static BotConfigurations LoadConfigrations()
    {
        string configJson = File.ReadAllText("Configuration.json");
        BotConfigurations botConfigurations = JsonConvert.DeserializeObject<BotConfigurations>(configJson)!;
        return botConfigurations;
    }

    static WeatherData InitializeWeatherData(BotConfigurations botConfigurations)
    {
        RainBot rainBot = CreateRainBot(botConfigurations.RainBot!);
        SunBot sunBot = CreateSunBot(botConfigurations.SunBot!);
        SnowBot snowBot = CreateSnowBot(botConfigurations.SnowBot!);

        WeatherData weatherData = new WeatherData();
        weatherData.Subscribe(rainBot);
        weatherData.Subscribe(sunBot);
        weatherData.Subscribe(snowBot);

        return weatherData;
    }
    static RainBot CreateRainBot(BotConfiguration configuration)
    {
        return new RainBot
        {
            Enabled = configuration.Enabled ?? false,
            HumidityThreshold = configuration.HumidityThreshold,
            Message = configuration.Message
        };
    }
    static SunBot CreateSunBot(BotConfiguration configuration)
    {
        return new SunBot
        {
            Enabled = configuration.Enabled ?? false,
            TemperatureThreshold = configuration.TemperatureThreshold,
            Message = configuration.Message
        };
    }

    static SnowBot CreateSnowBot(BotConfiguration configuration)
    {
        return new SnowBot
        {
            Enabled = configuration.Enabled ?? false,
            TemperatureThreshold = configuration.TemperatureThreshold,
            Message = configuration.Message
        };
    }

    static void ProcessInput(WeatherData weatherData, string inputData)
    {
        if (IsJson(inputData))
        {
            weatherData.InputStrategy = new JsonStrategy();
        }
        else if (IsXml(inputData))
        {
            weatherData.InputStrategy = new XMLStrategy();
        }
        else
        {
            Console.WriteLine("Unknown format.");
            return;
        }
        weatherData.ProcessInput(inputData);
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
// TODO
// Ask how to use the Enabled propertiy. 