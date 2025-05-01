using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseworkApp.ViewModels
{
    public class SensorViewModel : INotifyPropertyChanged
    {
        private readonly SensorService _sensorService;
        public ObservableCollection<SensorModel> Sensors { get; set; } = new();

        public bool IsBusy { get; set; }
        public bool HasNoSensors => Sensors.Count == 0;

        public Command RefreshSensorsCommand { get; }

        public SensorViewModel(SensorService sensorService)
        {
            _sensorService = sensorService;
            RefreshSensorsCommand = new Command(async () => await LoadSensorsAsync(forceReload: true));
        }

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

                Console.WriteLine($">>> Loaded {sensors.Count} sensors.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error loading sensors: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(HasNoSensors)); // Trigger UI to recheck visibility binding
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
