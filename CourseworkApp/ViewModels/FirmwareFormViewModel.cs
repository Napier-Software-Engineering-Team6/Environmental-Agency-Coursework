using CommunityToolkit.Mvvm.ComponentModel;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels;

[QueryProperty(nameof(FirmwareToEdit), "FirmwareToEdit")]
public partial class FirmwareFormViewModel : BaseFormViewModel
{
  private readonly IFirmwareService _firmwareService;
  private readonly IValidationService _validationService;
  private readonly ISensorHistoryService _sensorHistoryService;

  [ObservableProperty]
  private FirmwareConfigurations? firmwareToEdit;

  [ObservableProperty]
  private int firmwareId;

  [ObservableProperty]
  private string sensorType = string.Empty;

  [ObservableProperty]
  private string firmwareVersion = string.Empty;

  [ObservableProperty]
  private DateTime releaseDate = DateTime.MinValue;

  [ObservableProperty]
  private DateTime endofLifeDate = DateTime.MinValue;

  [ObservableProperty]
  private bool isActive;

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

  protected override async Task<bool> SaveAsync()
  {
    if (FirmwareToEdit == null)
    {
      ErrorMessage = "Firmware data is not available.";
      await _loggingService?.LogErrorAsync("SaveAsync called with null FirmwareToEdit.");
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
        await _loggingService?.LogErrorAsync($"FirmwareService.UpdateFirmwareAsync failed for FirmwareId {FirmwareToEdit.FirmwareId}.");
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
      await _loggingService?.LogErrorAsync($"Exception during UpdateFirmwareAsync for firmware {FirmwareToEdit.FirmwareId}.", ex);
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

  protected override async Task<bool> ValidateAsync()
  {
    if (_validationService == null)
    {
      if (string.IsNullOrWhiteSpace(FirmwareVersion))
      {
        ErrorMessage = "Firmware Version cannot be empty.";
        return false;
      }
      if (EndofLifeDate < ReleaseDate)
      {
        ErrorMessage = "End of Life Date cannot be earlier than Release Date.";
        return false;
      }
      ErrorMessage = string.Empty;
      return true;
    }

    if (FirmwareToEdit == null)
    {
      ErrorMessage = "Firmware data is not available for validation.";
      return false;
    }

    FirmwareToEdit.FirmwareVersion = FirmwareVersion;
    FirmwareToEdit.ReleaseDate = ReleaseDate;
    FirmwareToEdit.EndofLifeDate = EndofLifeDate;
    FirmwareToEdit.IsActive = IsActive;

    List<string> validationErrors = _validationService.ValidateFirmware(FirmwareToEdit);

    bool isValid = !validationErrors.Any();

    if (!isValid)
    {
      string errorDetailsForDb = string.Join("; ", validationErrors);
      ErrorMessage = string.Join(Environment.NewLine, validationErrors);

      await _loggingService?.LogWarningAsync($"Validation failed for Firmware ID {FirmwareToEdit.FirmwareId}: {ErrorMessage.Replace(Environment.NewLine, "; ")}");

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
        await _loggingService?.LogErrorAsync($"Validation failed for firmware ({FirmwareToEdit.FirmwareId}). Not logging to SensorConfigHistory.");
        ErrorMessage += $"{Environment.NewLine}(Note: Cannot log validation failure history for unsaved firmware)";
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
    return "FirmwareConfiguration";
  }
}