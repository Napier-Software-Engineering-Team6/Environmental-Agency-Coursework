using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CourseworkApp.Test;

public class DatabaseSensorFixture
{
  internal TestDbContext? _testDbContext { get; private set; }
  public DatabaseSensorFixture()
  {
    _testDbContext = new TestDbContext();

    _testDbContext.Database.EnsureDeleted();
    _testDbContext.Database.Migrate();
  }

  public void SeedData()
  {

    var sensorConfig1 = new SensorConfigurations()
    {
      SensorType = "Temperature",
      ConfigName = "Test Config 1",
      ConfigData = new BaseSensorConfig()
      {
        MonitorDurationSeconds = 60,
        MonitorFrequencySeconds = 5,
        LocationLatitude = 51.5074,
        LocationLongitude = -0.1278
      },
      CreatedAt = DateTime.Now,
      IsActive = true
    };

    _testDbContext.Add(sensorConfig1);

    var sensorConfig2 = new SensorConfigurations()
    {
      SensorType = "Humidity",
      ConfigName = "Test Config 2",
      ConfigData = new BaseSensorConfig()
      {
        MonitorDurationSeconds = 120,
        MonitorFrequencySeconds = 10,
        LocationLatitude = 40.7128,
        LocationLongitude = -74.0060
      },
      CreatedAt = DateTime.Now,
      IsActive = true
    };

    _testDbContext.Add(sensorConfig2);

    var sensorConfig3 = new SensorConfigurations()
    {
      SensorType = "Pressure",
      ConfigName = "Test Config 3",
      ConfigData = new BaseSensorConfig()
      {
        MonitorDurationSeconds = 180,
        MonitorFrequencySeconds = 15,
        LocationLatitude = 34.0522,
        LocationLongitude = -118.2437
      },
      CreatedAt = DateTime.Now,
      IsActive = true
    };

    _testDbContext.Add(sensorConfig3);

    var firmwareConfig1 = new FirmwareConfigurations()
    {
      SensorType = "Temperature",
      FirmwareVersion = "1.0.0",
      FirmwareData = "Firmware data for temperature sensor",
      ReleaseDate = DateTime.Now,
      EndofLifeDate = DateTime.Now.AddYears(2),
      IsActive = true
    };
    _testDbContext.Add(firmwareConfig1);

    var firmwareConfig2 = new FirmwareConfigurations()
    {
      SensorType = "Humidity",
      FirmwareVersion = "1.0.1",
      FirmwareData = "Firmware data for humidity sensor",
      ReleaseDate = DateTime.Now,
      EndofLifeDate = DateTime.Now.AddYears(2),
      IsActive = true
    };
    _testDbContext.Add(firmwareConfig2);

    var firmwareConfig3 = new FirmwareConfigurations()
    {
      SensorType = "Pressure",
      FirmwareVersion = "1.0.2",
      FirmwareData = "Firmware data for pressure sensor",
      ReleaseDate = DateTime.Now,
      EndofLifeDate = DateTime.Now.AddYears(2),
      IsActive = true
    };
    _testDbContext.Add(firmwareConfig3);



    var sensorEntry1 = new Sensors()
    {
      SensorName = "Test Sensor 1",
      SensorType = "Temperature",
      CurrentConfigId = sensorConfig1.ConfigId,
      CurrentFirmwareId = firmwareConfig1.FirmwareId
    };

    sensorEntry1.CurrentConfig = sensorConfig1;
    sensorEntry1.CurrentFirmware = firmwareConfig1;

    _testDbContext.Add(sensorEntry1);

    var sensorEntry2 = new Sensors()
    {
      SensorName = "Test Sensor 2",
      SensorType = "Humidity",
      CurrentConfigId = sensorConfig2.ConfigId,
      CurrentFirmwareId = firmwareConfig2.FirmwareId
    };
    sensorEntry2.CurrentConfig = sensorConfig2;
    sensorEntry2.CurrentFirmware = firmwareConfig2;

    _testDbContext.Add(sensorEntry2);

    var sensorEntry3 = new Sensors()
    {
      SensorName = "Test Sensor 3",
      SensorType = "Pressure",
      CurrentConfigId = sensorConfig3.ConfigId,
      CurrentFirmwareId = firmwareConfig3.FirmwareId
    };
    sensorEntry3.CurrentConfig = sensorConfig3;
    sensorEntry3.CurrentFirmware = firmwareConfig3;

    _testDbContext.Add(sensorEntry3);

    var sensorConfigHistory1 = new SensorConfigHistory()
    {
      ConfigId = sensorConfig1.ConfigId,
      ActionType = "Update",
      Status = "Success",
      Details = "Updated sensor configuration",
      PerformedBy = "Admin1",
      Timestamp = DateTime.Now
    };

    sensorConfigHistory1.Config = sensorConfig1;

    _testDbContext.Add(sensorConfigHistory1);

    var sensorConfigHistory2 = new SensorConfigHistory()
    {
      ConfigId = sensorConfig2.ConfigId,
      ActionType = "Update",
      Status = "Success",
      Details = "Updated sensor configuration",
      PerformedBy = "Admin2",
      Timestamp = DateTime.Now
    };

    sensorConfigHistory2.Config = sensorConfig2;

    _testDbContext.Add(sensorConfigHistory2);

    var sensorConfigHistory3 = new SensorConfigHistory()
    {
      FirmwareId = firmwareConfig3.FirmwareId,
      ActionType = "Update",
      Status = "Success",
      Details = "Updated sensor configuration",
      PerformedBy = "Admin3",
      Timestamp = DateTime.Now
    };
    sensorConfigHistory3.Firmware = firmwareConfig3;

    _testDbContext.Add(sensorConfigHistory3);

    _testDbContext.SaveChanges();
  }



}
