using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.ViewModels;

namespace CourseworkApp.Views;

public partial class AdminFirmware : ContentPage
{
	private readonly AdminFirmwareViewModel _viewModel;
	public AdminFirmware(AdminFirmwareViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_viewModel.LoadDataCommand is AsyncRelayCommand asyncCommand && asyncCommand.CanExecute(null))
		{
			await asyncCommand.ExecuteAsync(null);
		}

	}
}