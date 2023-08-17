using Newtonsoft.Json;
using System.Xml.Serialization;

public class WeatherData
{
    public string? Location { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    private List<IWeatherObserver> observers = new List<IWeatherObserver>();

    public void Subscribe(IWeatherObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IWeatherObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void FromJson(string json)
    {
        WeatherData newData = JsonConvert.DeserializeObject<WeatherData>(json)!;
        Location = newData.Location;
        Temperature = newData.Temperature;
        Humidity = newData.Humidity;
        NotifyObservers();


    }

    public string ToXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WeatherData));
        StringWriter stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, this);
        return stringWriter.ToString();
    }

    public void FromXml(string xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WeatherData));
        StringReader stringReader = new StringReader(xml);
        WeatherData newData = (WeatherData)serializer.Deserialize(stringReader)!;
        Location = newData.Location;
        Temperature = newData.Temperature;
        Humidity = newData.Humidity;
        NotifyObservers();
    }


}
