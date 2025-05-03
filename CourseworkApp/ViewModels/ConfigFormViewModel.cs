using CommunityToolkit.Mvvm.ComponentModel;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using CourseworkApp.Enums;
using CourseworkApp.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace CourseworkApp.ViewModels;


[QueryProperty(nameof(ConfigToEdit), "ConfigToEdit")]
public partial class ConfigFormViewModel : BaseFormViewModel
{
	private readonly IConfigurationService _configurationService;
	private readonly IValidationService _validationService;

	private readonly ISensorHistoryService _sensorHistoryService;


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

	public ConfigFormViewModel(IConfigurationService configurationService, IValidationService validationService, INavigationService navigationService, ILoggingService loggingService, ISensorConfigurationFactory configurationFactory, ISensorHistoryService sensorHistoryService)
			: base(navigationService, loggingService)
	{
		_configurationService = configurationService;
		_validationService = validationService;
		_sensorHistoryService = sensorHistoryService;

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
			if (_loggingService != null)
			{
				await _loggingService.LogErrorAsync("SaveAsync called with null ConfigToEdit or ConfigData.");
			}
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
				await _sensorHistoryService.LogActionAsync(
										configId: ConfigToEdit.ConfigId,
										firmwareId: null,
										actionType: "Configuration Update",
										status: "Success",
										details: $"Configuration '{ConfigToEdit.ConfigName}' updated.",
										performedBy: "TempUser"
										);
				ErrorMessage = string.Empty;
				return true;
			}
			else
			{
				ErrorMessage = "Failed to save configuration to the database.";
				if (_loggingService != null)
				{
					await _loggingService.LogErrorAsync($"ConfigurationService.UpdateConfigurationAsync failed for ConfigId {ConfigToEdit.ConfigId}.");
				}
				await _sensorHistoryService.LogActionAsync(
					configId: ConfigToEdit.ConfigId,
										firmwareId: null,
										actionType: "Configuration Update",
										status: "Failed",
										details: $"Update attempt failed (service returned false). Config Name: '{ConfigToEdit.ConfigName}'.",
										performedBy: "TempUser"
				);
				return false;
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = $"An error occurred while saving: {ex.Message}";
			if (_loggingService != null)
			{
				await _loggingService.LogErrorAsync($"Exception during UpdateConfigurationAsync for config {ConfigToEdit.ConfigId}.", ex);
			}
			await _sensorHistoryService.LogActionAsync(
				configId: ConfigToEdit.ConfigId,
								firmwareId: null,
								actionType: "Configuration Update",
								status: "Error",
								details: $"Exception during update attempt: {ex.Message}",
								performedBy: "TempUser"
			);
			return false;
		}
	}

	protected override async Task<bool> ValidateAsync()

	{
		if (_validationService == null)
		{
			ErrorMessage = "Validation service is not available.";
			return false;
		}

		if (ConfigToEdit == null)
		{
			ErrorMessage = "Configuration data is not available.";
			return false;
		}

		if (ConfigToEdit.ConfigData == null)
		{
			ErrorMessage = "Configuration details (ConfigData) is incomplete.";
			return false;
		}

		ConfigToEdit.ConfigData.MonitorFrequencySeconds = MonitorFrequencySeconds;
		ConfigToEdit.ConfigData.MonitorDurationSeconds = MonitorDurationSeconds;
		ConfigToEdit.ConfigData.LocationLatitude = LocationLatitude;
		ConfigToEdit.ConfigData.LocationLongitude = LocationLongitude;

		ValidationResult validationErrors = _validationService.ValidateConfig(ConfigToEdit);

		bool isValid = !validationErrors.Errors.Any();

		if (!isValid)
		{
			string errorDetailsForDb = string.Join("; ", validationErrors.Errors);


			ErrorMessage = string.Join(Environment.NewLine, validationErrors.Errors);



			if (_loggingService != null)
			{
				_ = _loggingService.LogWarningAsync($"Validation failed for Config ID {ConfigToEdit.ConfigId}: {ErrorMessage.Replace(Environment.NewLine, "; ")}");
			}
			if (ConfigToEdit.ConfigId > 0)
			{
				await _sensorHistoryService.LogActionAsync(
										configId: ConfigToEdit.ConfigId,
										firmwareId: null,
										actionType: "Configuration Validation",
										status: "Validation Failed",
										details: $"Validation failed before update attempt. Errors: {errorDetailsForDb}",
										performedBy: "TempUser"
										);
			}
			else
			{
				if (_loggingService != null)
					_ = _loggingService.LogErrorAsync($"Validation failed for config ({ConfigToEdit.ConfigId}). Not logging to SensorConfigHistory.");
				ErrorMessage += $"{Environment.NewLine}(Note: Cannot log validation failure history for unsaved configuration)";
			}
		}
		else
		{
			ErrorMessage = string.Empty;
		}
		return isValid;
	}
	protected override string GetEntityType()
	{
		return "SensorConfiguration";
	}
}