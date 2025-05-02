using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using CourseworkApp.ViewModels;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Test.Components.ViewModels;



public class ConfigFormViewModelTests
{
  private readonly Mock<IConfigurationService> _mockConfigService;
  private readonly Mock<IValidationService> _mockValidationService;
  private readonly Mock<INavigationService> _mockNavigationService;
  private readonly Mock<ILoggingService> _mockLoggingService;
  private readonly Mock<ISensorConfigurationFactory> _mockConfigFactory;

  private readonly Mock<ISensorHistoryService> _mockSensorHistoryService;
  private readonly string _testUser = "TempUser";

  private readonly TestableConfigFormViewModel _viewModel;

  public ConfigFormViewModelTests()
  {
    _mockConfigService = new Mock<IConfigurationService>();
    _mockValidationService = new Mock<IValidationService>();
    _mockNavigationService = new Mock<INavigationService>();
    _mockLoggingService = new Mock<ILoggingService>();
    _mockConfigFactory = new Mock<ISensorConfigurationFactory>();
    _mockSensorHistoryService = new Mock<ISensorHistoryService>();



    var defaultConfigData = new BaseSensorConfig
    {
      MonitorFrequencySeconds = 5,
      MonitorDurationSeconds = 60,
      LocationLatitude = 10.0,
      LocationLongitude = -5.0,
    };

    var defaultSensorConfig = new SensorConfigurations
    {
      ConfigId = 1,
      SensorType = "DefaultSensorType",
      ConfigName = "DefaultConfig",
      ConfigData = defaultConfigData,
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };


    _mockConfigFactory.Setup(f => f.CreateDefault()).Returns(defaultSensorConfig);


    _viewModel = new TestableConfigFormViewModel(
        _mockConfigService.Object,
        _mockValidationService.Object,
        _mockNavigationService.Object,
        _mockLoggingService.Object,
        _mockConfigFactory.Object,
        _mockSensorHistoryService.Object
    );
  }

  [Fact]
  public void Constructor_InitializesPropertiesToDefaultValues()
  {

    Assert.Equal(0, _viewModel.ConfigId);
    Assert.Equal(0, _viewModel.MonitorFrequencySeconds);
    Assert.Equal(0, _viewModel.MonitorDurationSeconds);
    Assert.Equal(0.0, _viewModel.LocationLatitude);
    Assert.Equal(0.0, _viewModel.LocationLongitude);
    Assert.False(_viewModel.IsActive);
    Assert.Equal(string.Empty, _viewModel.ErrorMessage);
    Assert.False(_viewModel.IsBusy);

    Assert.Null(_viewModel.ConfigToEdit);
  }

  [Fact]
  public void OnConfigToEditChanged_PopulatesProperties_WhenConfigIsNotNull()
  {
    // Arrange
    var sensorConfig = new SensorConfigurations
    {
      ConfigId = 123,
      SensorType = "TestSensor",
      ConfigName = "TestConfig",
      ConfigData = new BaseSensorConfig
      {
        MonitorFrequencySeconds = 10,
        MonitorDurationSeconds = 300,
        LocationLatitude = 45.67,
        LocationLongitude = -78.90,
      },
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };


    _viewModel.ConfigToEdit = sensorConfig;


    Assert.Equal(123, _viewModel.ConfigId);
    Assert.Equal(10, _viewModel.MonitorFrequencySeconds);
    Assert.Equal(300, _viewModel.MonitorDurationSeconds);
    Assert.Equal(45.67, _viewModel.LocationLatitude);
    Assert.Equal(-78.90, _viewModel.LocationLongitude);
    Assert.Equal(string.Empty, _viewModel.ErrorMessage);
    Assert.False(_viewModel.IsBusy);
  }

  [Fact]
  public async Task ValidateAsync_ReturnsTrue_WhenValuesAreValid()
  {
    // Arrange
    var sensorConfig = new SensorConfigurations
    {
      ConfigId = 123, // Give it an ID
      SensorType = "TestSensor",
      ConfigName = "ValidConfig",
      ConfigData = new BaseSensorConfig
      {
        MonitorFrequencySeconds = 10,
        MonitorDurationSeconds = 20,
        LocationLatitude = 50.0,
        LocationLongitude = -1.0
      },
      IsActive = true,
      CreatedAt = DateTime.UtcNow
    };

    _viewModel.ConfigToEdit = sensorConfig;

    _viewModel.MonitorFrequencySeconds = 10;
    _viewModel.MonitorDurationSeconds = 20;
    _viewModel.LocationLatitude = 50.0;
    _viewModel.LocationLongitude = -1.0;
    _viewModel.ErrorMessage = "Initial error";

    _mockValidationService
        .Setup(v => v.ValidateConfig(It.IsAny<SensorConfigurations>()))
        .Returns(new List<string>());

    _viewModel.ErrorMessage = "Initial error";


    // Act
    var isValid = await _viewModel.CallValidateAsync();

    // Assert
    Assert.True(isValid);
    Assert.Equal(string.Empty, _viewModel.ErrorMessage);
  }

  [Fact]
  public async Task SaveAsync_Succeeds_WhenConfigurationServiceUpdatesSuccessfully()
  {
    // Arrange
    var initialConfig = new SensorConfigurations
    {
      ConfigId = 789,
      SensorType = "TestSensor",
      ConfigName = "TestConfig",
      ConfigData = new BaseSensorConfig
      {
        MonitorFrequencySeconds = 1,
        MonitorDurationSeconds = 10,
        LocationLatitude = 1.1,
        LocationLongitude = 2.2,
      },
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };
    _viewModel.ConfigToEdit = initialConfig;

    _viewModel.MonitorFrequencySeconds = 15;
    _viewModel.MonitorDurationSeconds = 30;
    _viewModel.LocationLatitude = 3.3;
    _viewModel.LocationLongitude = 4.4;
    _viewModel.IsActive = false;
    _viewModel.ErrorMessage = "Initial error";

    _mockConfigService.Setup(s => s.UpdateConfigurationAsync(It.IsAny<SensorConfigurations>(), _testUser))
                     .ReturnsAsync(true);

    // Act
    var result = await _viewModel.CallSaveAsync();

    // Assert
    Assert.True(result);

    Assert.Equal(15, initialConfig.ConfigData.MonitorFrequencySeconds);
    Assert.Equal(30, initialConfig.ConfigData.MonitorDurationSeconds);
    Assert.Equal(3.3, initialConfig.ConfigData.LocationLatitude);
    Assert.Equal(4.4, initialConfig.ConfigData.LocationLongitude);
    Assert.False(initialConfig.IsActive);


    _mockConfigService.Verify(s => s.UpdateConfigurationAsync(initialConfig, _testUser), Times.Once());

  }

  [Fact]
  public void GetEntityType_ReturnsCorrectString()
  {

    // Act
    var entityType = _viewModel.CallGetEntityType();

    // Assert
    Assert.Equal("SensorConfiguration", entityType);
  }
}
