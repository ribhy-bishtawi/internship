using WeatherSys.Models;
using Xunit;

namespace WeatherSys.Tests.Models;
public class BotConfigurationTests
{
    [Fact]
    public void BotConfiguration_CanBeCreated()
    {
        // Arrange
        BotConfiguration config = new BotConfiguration();

        Assert.NotNull(config);
    }

    [Fact]
    public void BotConfiguration_PropertiesAreSetCorrectly()
    {
        // Arrange
        BotConfiguration config = new BotConfiguration
        {
            Enabled = true,
            HumidityThreshold = 50,
            TemperatureThreshold = 25,
            Message = "Sample message"
        };

        Assert.Equal(true, config.Enabled);
        Assert.Equal(50, config.HumidityThreshold);
        Assert.Equal(25, config.TemperatureThreshold);
        Assert.Equal("Sample message", config.Message);
    }

    [Fact]
    public void BotConfiguration_PropertiesAreNullable()
    {
        BotConfiguration config = new BotConfiguration
        {
            Enabled = null,
            HumidityThreshold = null,
            TemperatureThreshold = null,
            Message = null
        };


        Assert.Null(config.Enabled);
        Assert.Null(config.HumidityThreshold);
        Assert.Null(config.TemperatureThreshold);
        Assert.Null(config.Message);
    }
}

