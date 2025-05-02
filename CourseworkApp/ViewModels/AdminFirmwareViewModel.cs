using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models; // Make sure this using is correct
using CourseworkApp.Services;       // Make sure this using is correct
using System.Windows.Input;
using System.Diagnostics; // Added for potential debugging

namespace CourseworkApp.ViewModels;

public partial class AdminFirmwareViewModel : ObservableObject
{
  // Renamed service dependency
  private readonly IFirmwareService _firmwareService;
  private readonly INavigationService _navigationService;

  // Changed model type and renamed property
  [ObservableProperty]
  private ObservableCollection<FirmwareConfigurations> firmwareVersions;

  // Changed model type and renamed property
  [ObservableProperty]
  private FirmwareConfigurations? selectedFirmware;

  [ObservableProperty]
  private string? errorMessage;

  [ObservableProperty]
  private bool isLoading;

  public ICommand LoadDataCommand { get; }

  // Updated constructor signature and service assignment
  public AdminFirmwareViewModel(IFirmwareService firmwareService, INavigationService navigationService)
  {
    _firmwareService = firmwareService;
    _navigationService = navigationService;

    // Initialized with new property name and type
    firmwareVersions = new ObservableCollection<FirmwareConfigurations>();
    selectedFirmware = null; // Renamed
    errorMessage = string.Empty;
    IsLoading = false;

    // Point LoadDataCommand to the renamed loading method
    LoadDataCommand = new AsyncRelayCommand(LoadAllFirmwareAsync);
  }

  // Renamed method and updated logic to use firmware service and collection
  private async Task LoadAllFirmwareAsync()
  {
    try
    {
      IsLoading = true;
      ErrorMessage = string.Empty;
      FirmwareVersions.Clear(); // Use the renamed collection

      // Call the firmware service method
      var firmwares = await _firmwareService.GetAllFirmwareAsync();

      foreach (var firmware in firmwares)
        FirmwareVersions.Add(firmware); // Add to the renamed collection
    }
    catch (Exception ex)
    {
      // Updated error message
      ErrorMessage = $"Failed to load firmware versions: {ex.Message}";
      // Optional: Log the full exception for debugging
      // Debug.WriteLine($"Error loading firmware: {ex}");
    }
    finally
    {
      IsLoading = false;
    }
  }

  // Renamed command method, updated parameter type and navigation details
  [RelayCommand]
  private async Task EditFirmware(FirmwareConfigurations? firmwareToEdit)
  {
    if (firmwareToEdit != null)
    {
      var parameters = new Dictionary<string, object?>
            {
                // Updated parameter key and value
                { "FirmwareToEdit", firmwareToEdit }
            };

      // Updated navigation route
      await _navigationService.GoToAsync("///AdminFirmware/FirmwareForm", parameters);
    }
    else
    {
      // Updated error message
      ErrorMessage = "Please select a firmware version to edit.";
    }
  }
}