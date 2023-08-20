using Newtonsoft.Json;
using WatherSys.Models.Interfaces;
using WatherSys.Models;

namespace WatherSys.Models.Strategies;
class JsonStrategy : IInputStrategy
{
    public WeatherData ParseInput(string input)
    {
        return JsonConvert.DeserializeObject<WeatherData>(input)!;
    }
}