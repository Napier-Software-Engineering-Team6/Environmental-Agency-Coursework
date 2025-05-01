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

	}


}
