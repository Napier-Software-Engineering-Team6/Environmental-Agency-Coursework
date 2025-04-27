using CourseworkApp.Models;
using CourseworkApp.Services;
using CourseworkApp.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CourseworkApp.Test.ViewModels
{
    public class SensorViewModelTests
    {
        private readonly Mock<SensorService> _mockSensorService;
        private readonly SensorViewModel _sensorViewModel;

        public SensorViewModelTests()
        {
            _mockSensorService = new Mock<SensorService>(null);
            _sensorViewModel = new SensorViewModel(_mockSensorService.Object);
        }

        [Fact]
        public async Task LoadSensorsAsync_PopulatesSensorsCollection()
        {
            // Arrange
            var expectedSensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Name = "Sensor 1", Status = "Active" },
                new SensorModel { Id = 2, Name = "Sensor 2", Status = "Inactive" }
            };

            _mockSensorService.Setup(service => service.GetAllSensorsAsync())
                              .ReturnsAsync(expectedSensors);

            // Act
            await _sensorViewModel.LoadSensorsAsync();

            // Assert
            Assert.Equal(2, _sensorViewModel.Sensors.Count);
            Assert.Equal("Sensor 1", _sensorViewModel.Sensors[0].Name);
            Assert.Equal("Sensor 2", _sensorViewModel.Sensors[1].Name);
        }

        [Fact]
        public async Task LoadSensorsAsync_SetsIsBusyCorrectly()
        {
            // Arrange
            var expectedSensors = new List<SensorModel>();
            _mockSensorService.Setup(service => service.GetAllSensorsAsync())
                            .ReturnsAsync(expectedSensors);

            // Act
            await _sensorViewModel.LoadSensorsAsync();

            // Assert
            Assert.False(_sensorViewModel.IsBusy);
        }
    }
}
