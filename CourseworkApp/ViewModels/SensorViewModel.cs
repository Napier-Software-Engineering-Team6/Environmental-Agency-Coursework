using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using Microsoft.Maui.Controls;

namespace CourseworkApp.ViewModels
{
    public class SensorViewModel : INotifyPropertyChanged
    {
        private readonly SensorService _sensorService;

        public ObservableCollection<SensorModel> Sensors { get; set; } = new ObservableCollection<SensorModel>();

        public ICommand LoadSensorsCommand { get; }

        public ICommand RefreshSensorsCommand { get; }


        //private backing field for IsBusy property
        private bool isBusy;
        //The public property. Exposes the value to the outside (e.g., the UI) 
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public SensorViewModel(SensorService sensorService)
        {
            _sensorService = sensorService;
            //Both LoadSensorsCommand and RefreshSensorsCommand point to the same method (LoadSensorsAsync()), so no duplicated loading logic.DRY (Don't Repeat Yourself).
            LoadSensorsCommand = new Command(async () => await LoadSensorsAsync());
            RefreshSensorsCommand = new Command(async () => await LoadSensorsAsync());

        }

        public async Task LoadSensorsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var sensors = await _sensorService.GetAllSensorsAsync();

                Sensors.Clear();
                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, show alert)
                Console.WriteLine($"Error loading sensors: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
