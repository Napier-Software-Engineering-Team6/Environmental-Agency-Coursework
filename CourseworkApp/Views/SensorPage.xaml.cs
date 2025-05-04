using CourseworkApp.Services;
using CourseworkApp.ViewModels;

namespace CourseworkApp.Views
{
    /// <summary>
    /// Code-behind for SensorPage.xaml. Binds SensorViewModel to page context.
    /// Uses IAlertService to show errors.
    /// </summary>
    public partial class SensorPage : ContentPage
    {
        private readonly SensorViewModel viewModel;
        private readonly IAlertService alertService;

        public SensorPage(SensorViewModel viewModel, IAlertService alertService)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.alertService = alertService;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.LoadSensorsAsync();

            if (!string.IsNullOrEmpty(viewModel.ErrorMessage))
            {
                await alertService.ShowAlertAsync("Error", viewModel.ErrorMessage);
            }
        }
    }
}