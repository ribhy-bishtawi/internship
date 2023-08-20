using System.Xml.Serialization;

class XMLStrategy : IInputStrategy
{
    public WeatherData ParseInput(string input)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WeatherData));
        StringReader stringReader = new StringReader(input);
        WeatherData newData = (WeatherData)serializer.Deserialize(stringReader)!;
        return newData;
    }
}