using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels
{
    /// <summary>
    /// ViewModel for managing and displaying sensor data in the UI.
    /// Handles loading and refreshing logic.
    /// </summary>
    public class SensorViewModel : INotifyPropertyChanged
    {
        private readonly SensorService _sensorService;
        public ObservableCollection<SensorModel> Sensors { get; set; } = new();

        public bool IsBusy { get; set; }
        public bool HasNoSensors => Sensors.Count == 0; //used to capture empty state in UI

        /// <summary>
        /// Command to trigger sensor data refresh.
        /// </summary>
        public Command RefreshSensorsCommand { get; }

        /// <summary>
        /// Constructor for SensorViewModel with dependency injected service.
        /// </summary>
        /// <param name="sensorService">Service for sensor operations.</param>
        public SensorViewModel(SensorService sensorService)
        {
            _sensorService = sensorService;
            RefreshSensorsCommand = new Command(async () => await LoadSensorsAsync(forceReload: true));
        }

        /// <summary>
        /// Loads sensor data, with optional forced reload from the database.
        /// </summary>
        /// <param name="forceReload">Forces no-tracking context query if true.</param>
        public async Task LoadSensorsAsync(bool forceReload = false)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Sensors.Clear();
                var sensors = await _sensorService.GetAllSensorsAsync(forceReload);
                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);
                }
                OnPropertyChanged(nameof(HasNoSensors));
                Console.WriteLine($">>> Loaded {sensors.Count} sensors.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error loading sensors: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies UI of property value changes.
        /// </summary>
        /// <param name="propertyName">Property name to notify (optional).</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}