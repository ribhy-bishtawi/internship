using System;
using System.Xml.Serialization;
using WeatherSys.Interfaces;
using WeatherSys.Models;
using WeatherSys.Strategies;
using Xunit;
using Moq;


namespace YourProject.Tests.Models;

public class WeatherDataTests
{
    [Fact]
    public void WeatherData_CanBeCreated()
    {


        var weatherData = new WeatherData();

        Assert.NotNull(weatherData);
    }

    [Fact]
    public void WeatherData_Properties_SetAndGetCorrectly()
    {
        // Arrange
        var weatherData = new WeatherData
        {
            Location = "City Name",
            Temperature = 32.0,
            Humidity = 40.0,
        };

        var location = weatherData.Location;
        var temperature = weatherData.Temperature;
        var humidity = weatherData.Humidity;

        // Assert
        Assert.Equal("City Name", location);
        Assert.Equal(32.0, temperature);
        Assert.Equal(40.0, humidity);
    }
    [Fact]
    public void WeatherData_Serialization_IgnoresInputStrategy()
    {
        var mockedInputStrategy = new Mock<IInputStrategy>();

        var weatherData = new WeatherData
        {
            Location = "TestLocation",
            Temperature = 25.5,
            Humidity = 60.0,
            InputStrategy = mockedInputStrategy.Object,
        };

        var serializer = new XmlSerializer(typeof(WeatherData));

        string xmlResult;
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, weatherData);
            xmlResult = writer.ToString();
        }

        Assert.False(xmlResult.Contains("InputStrategy"));
    }

}

