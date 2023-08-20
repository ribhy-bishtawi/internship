using Newtonsoft.Json;
class JsonStrategy : IInputStrategy
{
    public WeatherData ParseInput(string input)
    {
        return JsonConvert.DeserializeObject<WeatherData>(input)!;
    }
}