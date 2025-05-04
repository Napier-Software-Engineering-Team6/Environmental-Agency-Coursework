using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using CourseworkApp.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CourseworkApp.Models.Enums;


namespace CourseworkApp.Test.ViewModels
{
    /// <summary>
    /// Unit tests for SensorViewModel to validate data loading and UI state behaviors.
    /// </summary>
    public class SensorViewModelTests
    {
        private readonly Mock<SensorService> _mockSensorService;
        private readonly SensorViewModel _sensorViewModel;

        /// <summary>
        /// Initializes the test class with a mock SensorService for dependency injection.
        /// </summary>
        public SensorViewModelTests()
        {
            _mockSensorService = new Mock<SensorService>(null);
            _sensorViewModel = new SensorViewModel(_mockSensorService.Object);
        }

        /// <summary>
        /// Verifies that LoadSensorsAsync correctly populates the Sensors collection with data.
        /// </summary>
        [Fact]
        public async Task LoadSensorsAsync_PopulatesSensorsCollection()
        {
            // Arrange
            var expectedSensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Name = "Sensor 1", Status = SensorStatus.Active },
                new SensorModel { Id = 2, Name = "Sensor 2", Status = SensorStatus.Inactive }
            };

            _mockSensorService
                .Setup(service => service.GetAllSensorsAsync(false))
                .ReturnsAsync(expectedSensors);

            // Act
            await _sensorViewModel.LoadSensorsAsync();

            // Assert
            Assert.Equal(2, _sensorViewModel.Sensors.Count);
            Assert.Equal("Sensor 1", _sensorViewModel.Sensors[0].Name);
            Assert.Equal("Sensor 2", _sensorViewModel.Sensors[1].Name);
        }

        /// <summary>
        /// Verifies that the IsBusy flag is properly reset after sensor data is loaded.
        /// </summary>
        [Fact]
        public async Task LoadSensorsAsync_SetsIsBusyCorrectly()
        {
            // Arrange
            var expectedSensors = new List<SensorModel>();
            _mockSensorService
                .Setup(service => service.GetAllSensorsAsync(false))
                .ReturnsAsync(expectedSensors);

            // Act
            await _sensorViewModel.LoadSensorsAsync();

            // Assert
            Assert.False(_sensorViewModel.IsBusy);
        }
    }
}
