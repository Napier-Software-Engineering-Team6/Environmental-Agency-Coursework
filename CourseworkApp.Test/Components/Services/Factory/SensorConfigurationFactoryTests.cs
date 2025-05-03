using Xunit;
using CourseworkApp.Services.Factory;
using CourseworkApp.Database.Models;


namespace CourseworkApp.Test.Components.Services.Factory
{
  public class SensorConfigurationFactoryTests
  {
    [Fact]
    public void CreateDefault_ShouldReturnExpectedDefaultConfiguration()
    {
      // Arrange
      ISensorConfigurationFactory factory = new SensorConfigurationFactory();

      const int expectedConfigId = 0;
      const string expectedSensorType = "Default";
      const string expectedConfigName = "New Sensor Configuration";
      const int expectedFrequency = 60;
      const int expectedDuration = 300;
      const double expectedLatitude = 0.0;
      const double expectedLongitude = 0.0;
      const bool expectedIsActive = true;

      // Act
      SensorConfigurations result = factory.CreateDefault();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(expectedConfigId, result.ConfigId);
      Assert.Equal(expectedSensorType, result.SensorType);
      Assert.Equal(expectedConfigName, result.ConfigName);
      Assert.Equal(expectedIsActive, result.IsActive);

      Assert.NotNull(result.ConfigData);
      Assert.IsType<BaseSensorConfig>(result.ConfigData);
      Assert.Equal(expectedFrequency, result.ConfigData.MonitorFrequencySeconds);
      Assert.Equal(expectedDuration, result.ConfigData.MonitorDurationSeconds);
      Assert.Equal(expectedLatitude, result.ConfigData.LocationLatitude);
      Assert.Equal(expectedLongitude, result.ConfigData.LocationLongitude);

      Assert.Equal(DateTimeKind.Utc, result.CreatedAt.Kind);
    }
  }
}