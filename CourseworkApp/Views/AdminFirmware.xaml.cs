using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.ViewModels;

namespace CourseworkApp.Views;
/// <summary>
/// AdminFirmware.xaml code-behind.
/// This page is responsible for displaying and managing firmware configurations.
/// It allows the user to view, edit, and delete firmware configurations.
/// The page is bound to the AdminFirmwareViewModel, which contains the logic for managing firmware data.
/// </summary>
public partial class AdminFirmware : ContentPage
{
	private readonly AdminFirmwareViewModel _viewModel;
	/// <summary>
	/// Initializes a new instance of the AdminFirmware class.
	/// This constructor sets up the page and binds it to the AdminFirmwareViewModel.
	/// </summary>
	public AdminFirmware(AdminFirmwareViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
	/// <summary>
	/// Invoked when the page appears.
	/// This method executes the LoadDataCommand if it can be executed.
	/// It is responsible for loading the firmware data when the page is displayed.
	/// </summary>
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_viewModel.LoadDataCommand is AsyncRelayCommand asyncCommand && asyncCommand.CanExecute(null))
		{
			await asyncCommand.ExecuteAsync(null);
		}

	}
}