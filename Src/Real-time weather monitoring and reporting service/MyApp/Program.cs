using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WeatherSys.Models;
using WeatherSys.Bots;
using WeatherSys.Strategies;
using WeatherSys.Interfaces;


class Program
{
    static void Main(string[] args)
    {
        BotConfigurations botConfigurations = LoadConfigrations();
        IWeatherObserverManager weatherObserverData = InitializeWeatherData(botConfigurations);

        Console.WriteLine("Enter weather data: ");
        string? inputData = Console.ReadLine();
        if (inputData == null)
        {
            Console.WriteLine("Please enter a valid format (JSON or XML)");
            return;
        }

        ProcessInput(weatherObserverData, inputData);

    }
    static BotConfigurations LoadConfigrations()
    {
        string configJson = File.ReadAllText("Configuration.json");
        BotConfigurations botConfigurations = JsonConvert.DeserializeObject<BotConfigurations>(configJson)!;
        return botConfigurations;
    }

    static IWeatherObserverManager InitializeWeatherData(BotConfigurations botConfigurations)
    {
        RainBot rainBot = CreateRainBot(botConfigurations.RainBot!);
        SunBot sunBot = CreateSunBot(botConfigurations.SunBot!);
        SnowBot snowBot = CreateSnowBot(botConfigurations.SnowBot!);

        IWeatherObserverManager weatherObserverData = new WeatherObserverManager();
        weatherObserverData.Subscribe(rainBot);
        weatherObserverData.Subscribe(sunBot);
        weatherObserverData.Subscribe(snowBot);

        return weatherObserverData;
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

    static void ProcessInput(IWeatherObserverManager weatherObserverData, string inputData)
    {
        IWeatherData weatherData = new WeatherData();
        IWeatherDataProcessor weatherDataProcessor = new WeatherDataProcessor(weatherObserverData);
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
        weatherDataProcessor.ProcessInput(weatherData, inputData);
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