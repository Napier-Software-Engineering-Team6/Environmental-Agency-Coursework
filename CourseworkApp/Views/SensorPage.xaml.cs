using CourseworkApp.ViewModels;
using CourseworkApp.Services;
using CourseworkApp.Repositories;

namespace CourseworkApp.Views
{
    public partial class SensorPage : ContentPage
    {
        private readonly SensorViewModel _viewModel;

        public SensorPage()
        {
            InitializeComponent();

            var sensorRepository = new SensorRepository(); // assume you already have ISensorRepository
            var sensorService = new SensorService(sensorRepository);
            _viewModel = new SensorViewModel(sensorService);

            BindingContext = _viewModel;

            _viewModel.LoadSensorsCommand.Execute(null);
        }
    }
}