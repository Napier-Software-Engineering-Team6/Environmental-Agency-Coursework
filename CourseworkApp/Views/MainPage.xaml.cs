using CourseworkApp.ViewModels;
using System.Diagnostics; // Required for Debug.WriteLine

namespace CourseworkApp.Views;


/**
 * @brief represents the main page of the application.
 * @remarks This page is responsible for displaying the main content of the app.
 * @seealso CourseworkApp.ViewModels.MainPageViewModel
 */
public partial class MainPage : ContentPage
{
	// Store the ViewModel instance
	private readonly MainPageViewModel _viewModel;

	public MainPage(MainPageViewModel viewModel)
	{
		System.Diagnostics.Debug.WriteLine(">>> MainPage Constructor: START");
		InitializeComponent();
		System.Diagnostics.Debug.WriteLine(">>> MainPage Constructor: AFTER InitializeComponent");
		// Store the injected ViewModel and set BindingContext
		_viewModel = viewModel;
		this.BindingContext = _viewModel;
		System.Diagnostics.Debug.WriteLine(">>> MainPage Constructor: AFTER BindingContext set");
	}

	// Override OnAppearing to trigger async initialization
	protected override async void OnAppearing()
	{
		Debug.WriteLine(">>> MainPage OnAppearing: START");
		base.OnAppearing();
		// Call the ViewModel's initialization method
		// Use await Task.Run if InitializeAsync is long and blocks UI,
		// but often awaiting directly is fine if LoadDataAsync releases the UI thread.
		await _viewModel.InitializeAsync();
		Debug.WriteLine(">>> MainPage OnAppearing: END (after InitializeAsync)");
	}
}
