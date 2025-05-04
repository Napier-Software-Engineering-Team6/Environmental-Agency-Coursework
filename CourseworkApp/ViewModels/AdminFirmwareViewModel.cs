using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models; // Make sure this using is correct
using CourseworkApp.Services;       // Make sure this using is correct
using System.Windows.Input;
using System.Diagnostics; // Added for potential debugging

namespace CourseworkApp.ViewModels;

/// <summary>
/// ViewModel for managing firmware in the admin interface.
/// Provides functionality for loading, displaying, and editing firmware configurations.
/// </summary>
public partial class AdminFirmwareViewModel : ObservableObject
{
  private readonly IFirmwareService _firmwareService;
  private readonly INavigationService _navigationService;

  /// <summary>
  /// Gets or sets the collection of firmware configurations.
  /// </summary>
  [ObservableProperty]
  private ObservableCollection<FirmwareConfigurations> firmwareVersions;

  /// <summary>
  /// Gets or sets the currently selected firmware configuration.
  /// </summary>
  [ObservableProperty]
  private FirmwareConfigurations? selectedFirmware;

  /// <summary>
  /// Gets or sets the error message to display in the UI.
  /// </summary>
  [ObservableProperty]
  private string? errorMessage;

  /// <summary>
  /// Gets or sets a value indicating whether the ViewModel is currently loading data.
  /// </summary>
  [ObservableProperty]
  private bool isLoading;

  /// <summary>
  /// Command to load all firmware configurations asynchronously.
  /// </summary>
  public ICommand LoadDataCommand { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminFirmwareViewModel"/> class.
  /// </summary>
  /// <param name="firmwareService">Service for managing firmware operations.</param>
  /// <param name="navigationService">Service for handling navigation.</param>
  public AdminFirmwareViewModel(IFirmwareService firmwareService, INavigationService navigationService)
  {
    _firmwareService = firmwareService;
    _navigationService = navigationService;

    firmwareVersions = new ObservableCollection<FirmwareConfigurations>();
    selectedFirmware = null;
    errorMessage = string.Empty;
    IsLoading = false;

    LoadDataCommand = new AsyncRelayCommand(LoadAllFirmwareAsync);
  }

  /// <summary>
  /// Loads all firmware configurations asynchronously.
  /// Clears the current collection and populates it with data from the firmware service.
  /// </summary>
  private async Task LoadAllFirmwareAsync()
  {
    try
    {
      IsLoading = true;
      ErrorMessage = string.Empty;
      FirmwareVersions.Clear();

      // Call the firmware service method
      var firmwares = await _firmwareService.GetAllFirmwareAsync();

      foreach (var firmware in firmwares)
        FirmwareVersions.Add(firmware);
    }
    catch (Exception ex)
    {
      // Log and display an error message
      ErrorMessage = $"Failed to load firmware versions: {ex.Message}";
      // Optional: Log the full exception for debugging
      // Debug.WriteLine($"Error loading firmware: {ex}");
    }
    finally
    {
      IsLoading = false;
    }
  }

  /// <summary>
  /// Navigates to the firmware editing form with the selected firmware configuration.
  /// </summary>
  /// <param name="firmwareToEdit">The firmware configuration to edit.</param>
  [RelayCommand]
  private async Task EditFirmware(FirmwareConfigurations? firmwareToEdit)
  {
    if (firmwareToEdit != null)
    {
      var parameters = new Dictionary<string, object?>
            {
                { "FirmwareToEdit", firmwareToEdit }
            };

      // Navigate to the firmware form
      await _navigationService.GoToAsync("///AdminFirmware/FirmwareForm", parameters);
    }
    else
    {
      // Display an error message if no firmware is selected
      ErrorMessage = "Please select a firmware version to edit.";
    }
  }
}