using Moq;
using Xunit;
using CourseworkApp.ViewModels;
using CourseworkApp.Services;
using CourseworkApp.Database.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.Input;

namespace CourseworkApp.Tests.ViewModels;

public class AdminConfigViewModelTests
{
  private readonly Mock<IConfigurationService> _mockConfigService;
  private readonly Mock<INavigationService> _mockNavService;
  private readonly AdminConfigViewModel _viewModel;

  private BaseSensorConfig CreateSampleBaseConfig()
  {
    return new BaseSensorConfig
    {
      MonitorFrequencySeconds = 60,
      MonitorDurationSeconds = 30,
      LocationLatitude = 56.0731,
      LocationLongitude = -3.4604
    };
  }

  private SensorConfigurations CreateSampleSensorConfig(int id, string name, string type = "AirQuality")
  {
    return new SensorConfigurations
    {
      ConfigId = id,
      SensorType = type,
      ConfigName = name,
      ConfigData = CreateSampleBaseConfig(),
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };
  }

  public AdminConfigViewModelTests()
  {
    _mockConfigService = new Mock<IConfigurationService>();
    _mockNavService = new Mock<INavigationService>();
    _viewModel = new AdminConfigViewModel(_mockConfigService.Object, _mockNavService.Object);
  }

  [Fact]
  public void Constructor_Initializes_Correctly()
  {
    // Assert
    Assert.NotNull(_viewModel.Configurations);
    Assert.Empty(_viewModel.Configurations);
    Assert.Null(_viewModel.SelectedConfiguration);
    Assert.True(string.IsNullOrEmpty(_viewModel.ErrorMessage));
    Assert.False(_viewModel.IsLoading);
    Assert.NotNull(_viewModel.LoadDataCommand);
    Assert.NotNull(_viewModel.EditConfigurationCommand);
  }

  [Fact]
  public async Task LoadDataCommand_Populates_Configurations_OnSuccess()
  {
    // Arrange
    var fakeConfigs = new List<SensorConfigurations>
        {
            CreateSampleSensorConfig(1, "City Centre Monitor"),
            CreateSampleSensorConfig(2, "Park Sensor", "WaterQuality")
        };
    _mockConfigService.Setup(s => s.GetAllConfigurationsAsync()).ReturnsAsync(fakeConfigs);

    // Act
    await ((AsyncRelayCommand)_viewModel.LoadDataCommand).ExecuteAsync(null);

    // Assert
    Assert.Equal(2, _viewModel.Configurations.Count);
    Assert.Contains(_viewModel.Configurations, c => c.ConfigId == 1 && c.SensorType == "AirQuality"); // Check relevant fields
    Assert.Contains(_viewModel.Configurations, c => c.ConfigId == 2 && c.SensorType == "WaterQuality");
    Assert.True(string.IsNullOrEmpty(_viewModel.ErrorMessage));
    Assert.False(_viewModel.IsLoading);
    _mockConfigService.Verify(s => s.GetAllConfigurationsAsync(), Times.Once);
  }

  [Fact]
  public async Task LoadDataCommand_Clears_Configurations_BeforeLoading()
  {
    // Arrange
    _viewModel.Configurations.Add(CreateSampleSensorConfig(99, "Old Config"));

    var fakeConfigs = new List<SensorConfigurations>
        {
          CreateSampleSensorConfig(1, "New Config A")
        };
    _mockConfigService.Setup(s => s.GetAllConfigurationsAsync()).ReturnsAsync(fakeConfigs);

    // Act
    await ((AsyncRelayCommand)_viewModel.LoadDataCommand).ExecuteAsync(null);

    // Assert
    Assert.Single(_viewModel.Configurations);
    Assert.DoesNotContain(_viewModel.Configurations, c => c.ConfigId == 99);
    Assert.Contains(_viewModel.Configurations, c => c.ConfigId == 1);
  }

  [Fact]
  public async Task LoadDataCommand_Sets_ErrorMessage_OnFailure()
  {
    // Arrange
    var exceptionMessage = "Database connection failed";
    _mockConfigService.Setup(s => s.GetAllConfigurationsAsync()).ThrowsAsync(new System.Exception(exceptionMessage));

    // Act
    await ((AsyncRelayCommand)_viewModel.LoadDataCommand).ExecuteAsync(null);

    // Assert
    Assert.Empty(_viewModel.Configurations);
    Assert.False(_viewModel.IsLoading);
    Assert.NotNull(_viewModel.ErrorMessage);
    Assert.Contains(exceptionMessage, _viewModel.ErrorMessage);
    _mockConfigService.Verify(s => s.GetAllConfigurationsAsync(), Times.Once);
  }

  [Fact]
  public async Task LoadDataCommand_Sets_IsLoading_Correctly()
  {
    // Arrange
    var tcs = new TaskCompletionSource<List<SensorConfigurations>>();

    _mockConfigService.Setup(s => s.GetAllConfigurationsAsync()).Returns(tcs.Task.ContinueWith(t => (IEnumerable<SensorConfigurations>)t.Result));

    // Act
    var loadTask = ((AsyncRelayCommand)_viewModel.LoadDataCommand).ExecuteAsync(null);

    // Assert: Should be loading
    Assert.True(_viewModel.IsLoading);
    Assert.True(string.IsNullOrEmpty(_viewModel.ErrorMessage));

    // Complete the async operation
    tcs.SetResult(new List<SensorConfigurations> { CreateSampleSensorConfig(1, "Loaded Config") });
    await loadTask;

    // Assert: Should not be loading after completion
    Assert.False(_viewModel.IsLoading);
    Assert.Single(_viewModel.Configurations); // Ensure data was loaded
  }


  [Fact]
  public async Task EditConfigurationCommand_Navigates_With_SelectedConfiguration()
  {
    // Arrange
    var selectedConfig = CreateSampleSensorConfig(5, "Config To Edit", "Weather");

    _mockNavService.Setup(s => s.GoToAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()))
      .Returns(Task.CompletedTask);

    // Act
    await ((AsyncRelayCommand<SensorConfigurations?>)_viewModel.EditConfigurationCommand).ExecuteAsync(selectedConfig);

    // Assert
    _mockNavService.Verify(s => s.GoToAsync(
        "///AdminConfig/ConfigForm",
        It.Is<Dictionary<string, object?>>(d => d.ContainsKey("ConfigToEdit") && d["ConfigToEdit"] == selectedConfig)),
        Times.Once);
    Assert.True(string.IsNullOrEmpty(_viewModel.ErrorMessage));
  }

  [Fact]
  public async Task EditConfigurationCommand_Sets_ErrorMessage_When_No_Configuration_Is_Passed()
  {
    // Arrange

    // Act
    await ((AsyncRelayCommand<SensorConfigurations?>)_viewModel.EditConfigurationCommand).ExecuteAsync(null);

    // Assert
    _mockNavService.Verify(s => s.GoToAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()), Times.Never);
    Assert.Equal("Please select a configuration to edit.", _viewModel.ErrorMessage);
  }
}