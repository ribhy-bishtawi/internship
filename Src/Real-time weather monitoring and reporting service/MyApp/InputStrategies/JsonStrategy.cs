using Newtonsoft.Json;
using WeatherSys.Interfaces;
using WeatherSys.Models;

namespace WeatherSys.Strategies;
class JsonStrategy : IInputStrategy
{
    public WeatherData ParseInput(string input)
    {
        return JsonConvert.DeserializeObject<WeatherData>(input)!;
    }
}