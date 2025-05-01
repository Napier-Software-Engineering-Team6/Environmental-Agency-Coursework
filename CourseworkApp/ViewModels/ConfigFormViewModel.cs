using CommunityToolkit.Mvvm.ComponentModel;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace CourseworkApp.ViewModels;


[QueryProperty(nameof(ConfigToEdit), "ConfigToEdit")]
public partial class ConfigFormViewModel : BaseFormViewModel
{
	private readonly IConfigurationService _configurationService;
	private readonly IValidationService? _validationService;


	[ObservableProperty]
	private SensorConfigurations? configToEdit;

	[ObservableProperty]
	private int configId;

	[ObservableProperty]
	private int monitorFrequencySeconds;

	[ObservableProperty]
	private int monitorDurationSeconds;

	[ObservableProperty]
	private double locationLatitude;

	[ObservableProperty]
	private double locationLongitude;

	[ObservableProperty]
	private bool isActive;

	public ConfigFormViewModel(IConfigurationService configurationService, IValidationService validationService, INavigationService navigationService, ILoggingService loggingService, ISensorConfigurationFactory configurationFactory)
			: base(navigationService, loggingService)
	{
		_configurationService = configurationService;
		_validationService = validationService;

		// Initialize properties to default values (this is fine)
		ConfigId = 0;
		MonitorFrequencySeconds = 0;
		MonitorDurationSeconds = 0;
		LocationLatitude = 0.0; // Use 0.0 for double
		LocationLongitude = 0.0; // Use 0.0 for double
		IsActive = false;
		ErrorMessage = string.Empty; // Initialize base class properties too
		IsBusy = false;
	}
	partial void OnConfigToEditChanged(SensorConfigurations? value)
	{


		if (value != null)
		{
			ConfigId = value.ConfigId;
			MonitorFrequencySeconds = value.ConfigData?.MonitorFrequencySeconds ?? 0;
			MonitorDurationSeconds = value.ConfigData?.MonitorDurationSeconds ?? 0;
			LocationLatitude = value.ConfigData?.LocationLatitude ?? 0.0;
			LocationLongitude = value.ConfigData?.LocationLongitude ?? 0.0;
			IsActive = value.IsActive;
			ErrorMessage = string.Empty;
			IsBusy = false;
		}
		else
		{
			ErrorMessage = "Error loading configuration data.";
			ConfigId = 0; // Reset fields for null case
			MonitorFrequencySeconds = 0;
			MonitorDurationSeconds = 0;
			LocationLatitude = 0.0;
			LocationLongitude = 0.0;
			IsActive = false;
			IsBusy = false;
		}
	}
	protected override async Task<bool> SaveAsync()
	{

		if (ConfigToEdit == null || ConfigToEdit.ConfigData == null)
		{
			ErrorMessage = "Configuration data is not available or incomplete.";
			if (_loggingService != null) await _loggingService.LogErrorAsync("SaveAsync called with null ConfigToEdit or ConfigData.");
			return false;
		}

		ConfigToEdit.ConfigData.MonitorFrequencySeconds = MonitorFrequencySeconds;
		ConfigToEdit.ConfigData.MonitorDurationSeconds = MonitorDurationSeconds;
		ConfigToEdit.ConfigData.LocationLatitude = LocationLatitude;
		ConfigToEdit.ConfigData.LocationLongitude = LocationLongitude;
		ConfigToEdit.IsActive = IsActive;

		try
		{
			bool success = await _configurationService.UpdateConfigurationAsync(ConfigToEdit, "TempUser");
			if (success)
			{
				await LogActionAsync("Update Configuration", "Success", $"Config ID {ConfigToEdit.ConfigId} updated.");
				return true;
			}
			else
			{
				ErrorMessage = "Failed to save configuration to the database.";
				if (_loggingService != null) await _loggingService.LogErrorAsync($"ConfigurationService.UpdateConfigurationAsync failed for ConfigId {ConfigToEdit.ConfigId}.");
				await LogActionAsync("Update Configuration", "Failed", $"Config ID {ConfigToEdit?.ConfigId} update failed.");
				return false;
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = $"An error occurred while saving: {ex.Message}";
			if (_loggingService != null) await _loggingService.LogErrorAsync($"Exception during UpdateConfigurationAsync for config {ConfigToEdit.ConfigId}.", ex);
			await LogActionAsync("Update Configuration", "Error", $"Exception updating Config ID {ConfigToEdit?.ConfigId}: {ex.Message}");
			return false;
		}
	}

	protected override Task<bool> ValidateAsync()

	{
		if (_validationService == null)
		{
			ErrorMessage = "Validation service is not available.";
			return Task.FromResult(false);
		}

		if (ConfigToEdit == null)
		{
			ErrorMessage = "Configuration data is not available.";
			return Task.FromResult(false);
		}

		if (ConfigToEdit.ConfigData == null)
		{
			ErrorMessage = "Configuration details (ConfigData) is incomplete.";
			return Task.FromResult(false);
		}

		ConfigToEdit.ConfigData.MonitorFrequencySeconds = MonitorFrequencySeconds;
		ConfigToEdit.ConfigData.MonitorDurationSeconds = MonitorDurationSeconds;
		ConfigToEdit.ConfigData.LocationLatitude = LocationLatitude;
		ConfigToEdit.ConfigData.LocationLongitude = LocationLongitude;

		List<string> validationErrors = _validationService.ValidateConfig(ConfigToEdit);

		bool isValid = !validationErrors.Any(); // Or validationErrors.Count == 0;

		if (!isValid)
		{
			ErrorMessage = string.Join(Environment.NewLine, validationErrors);
			if (_loggingService != null) _ = _loggingService.LogWarningAsync($"Validation failed for Config ID {ConfigToEdit.ConfigId}: {ErrorMessage.Replace(Environment.NewLine, "; ")}");
		}
		else
		{
			ErrorMessage = string.Empty; // Clear error message if validation passes
		}
		return Task.FromResult(isValid);
	}

	protected override string GetEntityType()
	{
		return "SensorConfiguration";
	}
}