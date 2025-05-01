using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.ViewModels;
using CourseworkApp.Services;

namespace CourseworkApp.Views;

public partial class AdminConfig : ContentPage
{
	private readonly AdminConfigViewModel _viewModel;
	public AdminConfig(AdminConfigViewModel viewModel)
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