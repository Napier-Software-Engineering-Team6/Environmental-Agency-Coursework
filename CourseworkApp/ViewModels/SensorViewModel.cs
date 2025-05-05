using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels
{
    /// <summary>
    /// ViewModel for managing and displaying sensor data in the UI.
    /// Handles loading, refreshing, and error messaging.
    /// </summary>
    public partial class SensorViewModel : ObservableObject
    {
        private readonly SensorService sensorService;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        public ObservableCollection<SensorModel> Sensors { get; } = new();

        public bool HasNoSensors => Sensors.Count == 0;

        public IRelayCommand RefreshSensorsCommand { get; }

        public SensorViewModel(SensorService sensorService)
        {
            this.sensorService = sensorService;
            RefreshSensorsCommand = new AsyncRelayCommand(LoadSensorsAsync);
        }

        /// <summary>
        /// Loads sensor data asynchronously and updates the collection.
        /// </summary>
        public async Task LoadSensorsAsync(CancellationToken cancellationToken = default)
        {
            if (IsBusy) return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                Sensors.Clear();
                var sensors = await sensorService.GetAllSensorsAsync();
                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);
                }
                OnPropertyChanged(nameof(HasNoSensors));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading sensors: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}