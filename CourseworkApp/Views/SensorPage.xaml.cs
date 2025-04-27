using CourseworkApp.ViewModels;

namespace CourseworkApp.Views
{
    public partial class SensorPage : ContentPage
    {
        private readonly SensorViewModel _viewModel;

        public SensorPage(SensorViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadSensorsAsync();
        }
    }
}
