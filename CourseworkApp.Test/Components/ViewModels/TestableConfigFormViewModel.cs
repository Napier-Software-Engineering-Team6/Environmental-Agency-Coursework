// Inside your CourseworkApp.Test project

using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using CourseworkApp.ViewModels; // Assuming your ViewModel namespace
using System.Threading.Tasks;
using CourseworkApp.Database.Models; // Ensure this is included

// This class inherits from the ViewModel you want to test
// We make it public so it can be instantiated in the test class.
public class TestableConfigFormViewModel : ConfigFormViewModel
{
  // Add a public constructor that mirrors the base class constructor
  // and passes dependencies up to the base
  public TestableConfigFormViewModel(
      IConfigurationService configurationService,
      IValidationService validationService, // Keep this dependency even if ValidateAsync doesn't use it directly anymore, as the constructor requires it.
      INavigationService navigationService,
      ILoggingService loggingService,
      ISensorConfigurationFactory configurationFactory
      )
      : base(configurationService, validationService, navigationService, loggingService, configurationFactory)
  {
  }

  // Expose the protected ValidateAsync method for testing
  public Task<bool> CallValidateAsync()
  {
    // Call the protected method from the base class
    return base.ValidateAsync();
  }

  // Expose the protected GetEntityType method for testing
  public string CallGetEntityType()
  {
    // Call the protected method from the base class
    return base.GetEntityType();
  }

  // Expose the protected SaveAsync method for testing
  public Task<bool> CallSaveAsync()
  {
    return base.SaveAsync();
  }

  // You might need to expose the protected OnConfigToEditChanged method
  // if you want to test its behavior directly by manually calling it.
  // However, setting the public ConfigToEdit property will automatically
  // trigger the partial method, which is usually sufficient for testing.
  // If you needed direct access, you could add:
  // public void CallOnConfigToEditChanged(SensorConfigurations? value)
  // {
  //     base.OnConfigToEditChanged(value);
  // }

  // Expose the ConfigToEdit property publicly for easier access in tests
  public new SensorConfigurations? ConfigToEdit
  {
    get => base.ConfigToEdit;
    set => base.ConfigToEdit = value;
  }

  // Expose other ObservableProperties publicly for setting and asserting values
  public new int ConfigId
  {
    get => base.ConfigId;
    set => base.ConfigId = value;
  }

  public new int MonitorFrequencySeconds
  {
    get => base.MonitorFrequencySeconds;
    set => base.MonitorFrequencySeconds = value;
  }

  public new int MonitorDurationSeconds
  {
    get => base.MonitorDurationSeconds;
    set => base.MonitorDurationSeconds = value;
  }

  public new double LocationLatitude
  {
    get => base.LocationLatitude;
    set => base.LocationLatitude = value;
  }

  public new double LocationLongitude
  {
    get => base.LocationLongitude;
    set => base.LocationLongitude = value;
  }

  public new bool IsActive
  {
    get => base.IsActive;
    set => base.IsActive = value;
  }

  public new string ErrorMessage
  {
    get => base.ErrorMessage;
    set => base.ErrorMessage = value;
  }

  public new bool IsBusy
  {
    get => base.IsBusy;
    set => base.IsBusy = value;
  }
}
