using CommunityToolkit.Mvvm.ComponentModel;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using CourseworkApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels;

/// <summary>
/// ViewModel for managing firmware forms, including editing and validation of firmware configurations.
/// </summary>
[QueryProperty(nameof(FirmwareToEdit), "FirmwareToEdit")]
public partial class FirmwareFormViewModel : BaseFormViewModel
{
  private readonly IFirmwareService _firmwareService;
  private readonly IValidationService _validationService;
  private readonly ISensorHistoryService _sensorHistoryService;

  /// <summary>
  /// Gets or sets the firmware configuration being edited.
  /// </summary>
  [ObservableProperty]
  private FirmwareConfigurations? firmwareToEdit;

  /// <summary>
  /// Gets or sets the ID of the firmware.
  /// </summary>
  [ObservableProperty]
  private int firmwareId;

  /// <summary>
  /// Gets or sets the sensor type associated with the firmware.
  /// </summary>
  [ObservableProperty]
  private string sensorType = string.Empty;

  /// <summary>
  /// Gets or sets the firmware version.
  /// </summary>
  [ObservableProperty]
  private string firmwareVersion = string.Empty;

  /// <summary>
  /// Gets or sets the release date of the firmware.
  /// </summary>
  [ObservableProperty]
  private DateTime releaseDate = DateTime.MinValue;

  /// <summary>
  /// Gets or sets the end-of-life date of the firmware.
  /// </summary>
  [ObservableProperty]
  private DateTime endofLifeDate = DateTime.MinValue;

  /// <summary>
  /// Gets or sets a value indicating whether the firmware is active.
  /// </summary>
  [ObservableProperty]
  private bool isActive;

  /// <summary>
  /// Initializes a new instance of the <see cref="FirmwareFormViewModel"/> class.
  /// </summary>
  /// <param name="firmwareService">Service for managing firmware operations.</param>
  /// <param name="validationService">Service for validating firmware data.</param>
  /// <param name="navigationService">Service for handling navigation.</param>
  /// <param name="loggingService">Service for logging errors and warnings.</param>
  /// <param name="sensorHistoryService">Service for logging sensor history actions.</param>
  public FirmwareFormViewModel(
      IFirmwareService firmwareService,
      IValidationService validationService,
      INavigationService navigationService,
      ILoggingService loggingService,
      ISensorHistoryService sensorHistoryService)
      : base(navigationService, loggingService)
  {
    _firmwareService = firmwareService;
    _validationService = validationService;
    _sensorHistoryService = sensorHistoryService;

    FirmwareId = 0;
    SensorType = "N/A";
    FirmwareVersion = string.Empty;
    ReleaseDate = DateTime.Today;
    EndofLifeDate = DateTime.Today.AddYears(1);
    IsActive = false;
    ErrorMessage = string.Empty;
    IsBusy = false;
  }

  /// <summary>
  /// Handles changes to the <see cref="FirmwareToEdit"/> property.
  /// Updates the ViewModel properties with the new firmware data.
  /// </summary>
  /// <param name="value">The new firmware configuration.</param>
  partial void OnFirmwareToEditChanged(FirmwareConfigurations? value)
  {
    if (value != null)
    {
      FirmwareId = value.FirmwareId;
      SensorType = value.SensorType;
      FirmwareVersion = value.FirmwareVersion;
      ReleaseDate = value.ReleaseDate;
      EndofLifeDate = value.EndofLifeDate;
      IsActive = value.IsActive;
      ErrorMessage = string.Empty;
      IsBusy = false;
    }
    else
    {
      ErrorMessage = "Error loading firmware data.";
      FirmwareId = 0;
      SensorType = "Error";
      FirmwareVersion = string.Empty;
      ReleaseDate = DateTime.MinValue;
      EndofLifeDate = DateTime.MinValue;
      IsActive = false;
      IsBusy = false;
    }
  }

  /// <summary>
  /// Saves the firmware data asynchronously.
  /// </summary>
  /// <returns>A task that represents the asynchronous save operation. Returns true if the save was successful; otherwise, false.</returns>
  protected override async Task<bool> SaveAsync()
  {
    if (FirmwareToEdit == null)
    {
      ErrorMessage = "Firmware data is not available.";
      await _loggingService.LogErrorAsync("SaveAsync called with null FirmwareToEdit.");
      return false;
    }

    FirmwareToEdit.FirmwareVersion = FirmwareVersion;
    FirmwareToEdit.ReleaseDate = ReleaseDate;
    FirmwareToEdit.EndofLifeDate = EndofLifeDate;
    FirmwareToEdit.IsActive = IsActive;

    try
    {
      bool success = await _firmwareService.UpdateFirmwareAsync(FirmwareToEdit, "TempUser");
      if (success)
      {
        await _sensorHistoryService.LogActionAsync(
            configId: null,
            firmwareId: FirmwareToEdit.FirmwareId,
            actionType: "Firmware Update",
            status: "Success",
            details: $"Firmware '{FirmwareToEdit.FirmwareVersion}' for sensor type '{FirmwareToEdit.SensorType}' updated.",
            performedBy: "TempUser"
        );
        ErrorMessage = string.Empty;
        return true;
      }
      else
      {
        ErrorMessage = "Failed to save firmware changes to the database.";
        await _loggingService.LogErrorAsync($"FirmwareService.UpdateFirmwareAsync failed for FirmwareId {FirmwareToEdit.FirmwareId}.");
        await _sensorHistoryService.LogActionAsync(
            configId: null,
            firmwareId: FirmwareToEdit.FirmwareId,
            actionType: "Firmware Update",
            status: "Failed",
            details: $"Update attempt failed (service returned false). Firmware Version: '{FirmwareToEdit.FirmwareVersion}'.",
            performedBy: "TempUser"
        );
        return false;
      }
    }
    catch (Exception ex)
    {
      ErrorMessage = $"An error occurred while saving firmware: {ex.Message}";
      await _loggingService.LogErrorAsync($"Exception during UpdateFirmwareAsync for firmware {FirmwareToEdit.FirmwareId}.", ex);
      await _sensorHistoryService.LogActionAsync(
          configId: null,
          firmwareId: FirmwareToEdit.FirmwareId,
          actionType: "Firmware Update",
          status: "Error",
          details: $"Exception during update attempt: {ex.Message}",
          performedBy: "TempUser"
      );
      return false;
    }
  }

  /// <summary>
  /// Validates the firmware data asynchronously.
  /// </summary>
  /// <returns>A task that represents the asynchronous validation operation. Returns true if the data is valid; otherwise, false.</returns>
  protected override async Task<bool> ValidateAsync()
  {
    if (_validationService == null)
    {
      ErrorMessage = "Validation service is not available.";
      await _loggingService.LogErrorAsync("ValidationService is null in FirmwareFormViewModel.ValidateAsync.");
      return false;
    }

    if (FirmwareToEdit == null)
    {
      ErrorMessage = "Firmware data is not available for validation.";
      await _loggingService.LogWarningAsync("ValidateAsync called when FirmwareToEdit is null.");
      return false;
    }

    FirmwareToEdit.FirmwareVersion = FirmwareVersion;
    FirmwareToEdit.ReleaseDate = ReleaseDate;
    FirmwareToEdit.EndofLifeDate = EndofLifeDate;
    FirmwareToEdit.IsActive = IsActive;

    ValidationResult validationResult = _validationService.ValidateFirmware(FirmwareToEdit);

    bool isValid = validationResult.IsValid;

    if (!isValid)
    {
      string errorDetailsForDb = string.Join("; ", validationResult.Errors);
      ErrorMessage = string.Join(Environment.NewLine, validationResult.Errors);

      if (FirmwareToEdit.FirmwareId > 0)
      {
        await _sensorHistoryService.LogActionAsync(
            configId: null,
            firmwareId: FirmwareToEdit.FirmwareId,
            actionType: "Firmware Validation",
            status: "Validation Failed",
            details: $"Validation failed before update attempt. Errors: {errorDetailsForDb}",
            performedBy: "TempUser"
        );
      }
      else
      {
        await _loggingService.LogErrorAsync($"Validation failed for unsaved firmware (ID {FirmwareToEdit.FirmwareId}). Not logging to SensorConfigHistory.");
        ErrorMessage += $"{Environment.NewLine}(Note: Cannot log validation failure history for unsaved firmware)";
      }
    }
    else
    {
      ErrorMessage = string.Empty;
    }
    return isValid;
  }

  /// <summary>
  /// Gets the type of the entity being managed by this ViewModel.
  /// </summary>
  /// <returns>The entity type as a string.</returns>
  protected override string GetEntityType()
  {
    return "FirmwareConfiguration";
  }
}