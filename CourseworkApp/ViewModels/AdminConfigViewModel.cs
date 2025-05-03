using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System.Windows.Input;

namespace CourseworkApp.ViewModels;
/// <summary>
/// ViewModel for the AdminConfig page.
/// This class is responsible for managing the state and behavior of the AdminConfig viwq
/// </summary>
public partial class AdminConfigViewModel : ObservableObject
{
	private readonly IConfigurationService _configurationService;
	private readonly INavigationService _navigationService;

	[ObservableProperty]
	private ObservableCollection<SensorConfigurations> configurations;

	[ObservableProperty]
	private SensorConfigurations? selectedConfiguration;

	[ObservableProperty]
	private string? errorMessage;

	[ObservableProperty]
	private bool isLoading;
	/// <summary>
	/// 
	/// </summary>
	public ICommand LoadDataCommand { get; }
	/// <summary>
	/// 
	/// </summary>
	/// <param name="configurationService"></param>
	/// <param name="navigationService"></param>
	public AdminConfigViewModel(IConfigurationService configurationService, INavigationService navigationService)
	{
		_configurationService = configurationService;
		_navigationService = navigationService;
		configurations = new ObservableCollection<SensorConfigurations>();
		selectedConfiguration = null;
		errorMessage = string.Empty;
		IsLoading = false;

		LoadDataCommand = new AsyncRelayCommand(LoadAllConfigurationsAsync);
	}
	private async Task LoadAllConfigurationsAsync()
	{
		try
		{
			IsLoading = true;
			ErrorMessage = string.Empty;
			Configurations.Clear();

			var configs = await _configurationService.GetAllConfigurationsAsync();

			foreach (var config in configs)
				Configurations.Add(config);
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Failed to load configurations: {ex.Message}";
		}
		finally
		{
			IsLoading = false;
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="configToEdit"></param>
	/// <returns></returns>
	[RelayCommand]
	private async Task EditConfiguration(SensorConfigurations? configToEdit)
	{
		if (configToEdit != null)
		{
			var parameters = new Dictionary<string, object?>
			{
				{ "ConfigToEdit", configToEdit }
			};

			await _navigationService.GoToAsync("///AdminConfig/ConfigForm", parameters);
		}
		else
		{
			ErrorMessage = "Please select a configuration to edit.";
		}
	}
}