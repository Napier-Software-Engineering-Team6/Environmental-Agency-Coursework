using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.ViewModels;
using CourseworkApp.Services;

namespace CourseworkApp.Views;
/// <summary>
/// Interaction logic for AdminConfig.xaml
/// </summary>
public partial class AdminConfig : ContentPage
{
	private readonly AdminConfigViewModel _viewModel;
	/// <summary>
	/// Constructor for AdminConfig page.
	/// Initializes the page and sets the BindingContext to the provided view model
	/// </summary>
	/// <param name="viewModel"></param>
	public AdminConfig(AdminConfigViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	/// <summary>
	/// Invoked when the page appears.
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_viewModel.LoadDataCommand is AsyncRelayCommand asyncCommand && asyncCommand.CanExecute(null))
		{
			await asyncCommand.ExecuteAsync(null);
		}
	}
}