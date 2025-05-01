using CourseworkApp.ViewModels;

namespace CourseworkApp.Views
{
    /// <summary>
    /// Code-behind for SensorPage.xaml. Binds SensorViewModel to page context.
    /// </summary>
    public partial class SensorPage : ContentPage
    {
        private readonly SensorViewModel _viewModel;

        /// <summary>
        /// Initializes the page and assigns the SensorViewModel as the BindingContext.
        /// </summary>
        /// <param name="viewModel">The ViewModel injected through dependency injection.</param>
        public SensorPage(SensorViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        /// <summary>
        /// Triggers automatic data loading when the page appears.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Ensure fresh data is loaded when the page appears.
            await _viewModel.LoadSensorsAsync(forceReload: true);
        }
    }
}
