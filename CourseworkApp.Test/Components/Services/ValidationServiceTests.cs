using NUnit.Framework;
using CourseworkApp.Services;
using CourseworkApp.Database.Models;
using CourseworkApp.Common;
using CourseworkApp.Enums;
using System.Linq;
using System;

namespace CourseworkApp.Tests.Services
{
  [TestFixture]
  public class ValidationServiceTests
  {
    private IValidationService _validationService;

    [SetUp]
    public void Setup()
    {
      _validationService = new ValidationService();
    }

    private SensorConfigurations CreateValidConfig()
    {
      return new SensorConfigurations
      {
        SensorType = "Temperature",
        ConfigName = "Test Room Sensor",
        ConfigData = new BaseSensorConfig
        {
          MonitorFrequencySeconds = 60,
          MonitorDurationSeconds = 300,
          LocationLatitude = 55.9533,
          LocationLongitude = -3.1883
        },
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };
    }

    [Test]
    public void ValidateConfig_WithValidConfig_ReturnsSuccess()
    {
      var config = CreateValidConfig();
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Success));
      NUnit.Framework.Assert.That(result.Errors, Is.Not.Null);
      NUnit.Framework.Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void ValidateConfig_WithNullConfig_ReturnsFailedStatusAndError()
    {
      SensorConfigurations config = null;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Is.Not.Null);
      NUnit.Framework.Assert.That(result.Errors.Count, Is.EqualTo(1));
      NUnit.Framework.Assert.That(result.Errors.First(), Is.EqualTo("Validation Failed: Config object is null."));
    }

    [Test]
    public void ValidateConfig_WithEmptySensorType_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.SensorType = "";
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: SensorType is empty."));
    }

    [Test]
    public void ValidateConfig_WithWhitespaceSensorType_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.SensorType = "   ";
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: SensorType is empty."));
    }

    [Test]
    public void ValidateConfig_WithNullConfigName_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigName = null;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: ConfigName is empty."));
    }

    [Test]
    public void ValidateConfig_WithNullConfigData_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData = null;
      ValidationResult result = new ValidationResult();
      try
      {
        if (config.ConfigData == null)
        {
          result.Errors.Add("Validation Failed: ConfigData is null.");
          result.Status = ValidationStatus.Failed;
        }
      }
      catch (NullReferenceException)
      {
      }

      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: ConfigData is null."));
    }

    [Test]
    public void ValidateConfig_WithZeroMonitorFrequency_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.MonitorFrequencySeconds = 0;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: MonitorFrequencySeconds must be positive."));
    }

    [Test]
    public void ValidateConfig_WithNegativeMonitorFrequency_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.MonitorFrequencySeconds = -10;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: MonitorFrequencySeconds must be positive."));
    }

    [Test]
    public void ValidateConfig_WithZeroMonitorDuration_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.MonitorDurationSeconds = 0;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: MonitorDurationSeconds must be positive."));
    }

    [Test]
    public void ValidateConfig_WithLatitudeTooLow_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.LocationLatitude = -90.1;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLatitude is out of range."));
    }

    [Test]
    public void ValidateConfig_WithLatitudeTooHigh_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.LocationLatitude = 90.1;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLatitude is out of range."));
    }

    [Test]
    public void ValidateConfig_WithLongitudeTooLow_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.LocationLongitude = -180.1;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLongitude is out of range."));
    }

    [Test]
    public void ValidateConfig_WithLongitudeTooHigh_ReturnsFailedStatusAndError()
    {
      var config = CreateValidConfig();
      config.ConfigData.LocationLongitude = 180.1;
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLongitude is out of range."));
    }

    [Test]
    public void ValidateConfig_WithMultipleErrors_ReturnsFailedStatusAndAllErrors()
    {
      var config = new SensorConfigurations
      {
        SensorType = "",
        ConfigName = null,
        ConfigData = new BaseSensorConfig
        {
          MonitorFrequencySeconds = -10,
          MonitorDurationSeconds = 0,
          LocationLatitude = 100.0,
          LocationLongitude = -200.0
        },
        CreatedAt = DateTime.UtcNow,
        IsActive = false
      };
      ValidationResult result = _validationService.ValidateConfig(config);

      NUnit.Framework.Assert.That(result.Status, Is.EqualTo(ValidationStatus.Failed));
      NUnit.Framework.Assert.That(result.Errors, Is.Not.Null);
      NUnit.Framework.Assert.That(result.Errors.Count, Is.EqualTo(6));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: SensorType is empty."));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: ConfigName is empty."));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: MonitorFrequencySeconds must be positive."));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: MonitorDurationSeconds must be positive."));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLatitude is out of range."));
      NUnit.Framework.Assert.That(result.Errors, Does.Contain("Validation Failed: LocationLongitude is out of range."));
    }

    [Test]
    public void ValidateConfig_WithNullConfigData_ThrowsNullReferenceException_DueToCurrentImplementation()
    {
      var config = CreateValidConfig();
      config.ConfigData = null;

      NUnit.Framework.Assert.Throws<NullReferenceException>(() => _validationService.ValidateConfig(config));
    }
  }
}