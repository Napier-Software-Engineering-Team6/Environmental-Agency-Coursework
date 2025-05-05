using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models;
using CourseworkApp.Services;
using Microsoft.Maui.Dispatching;
using Syncfusion.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics; // Still needed for default color

namespace CourseworkApp.ViewModels
{
  public partial class SensorMapViewModel : ObservableObject
  {
    private readonly ISensorReadingService _sensorReadingService;
    private readonly ILoggingService _loggingService;
    private IDispatcherTimer? _refreshTimer;
    private const int RefreshIntervalSeconds = 30;

    [ObservableProperty]
    private ObservableCollection<MapMarker> _sensorMarkers;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public SensorMapViewModel(ISensorReadingService sensorReadingService, ILoggingService loggingService)
    {
      _sensorReadingService = sensorReadingService ?? throw new ArgumentNullException(nameof(sensorReadingService));
      _loggingService = loggingService;
      _sensorMarkers = new ObservableCollection<MapMarker>();
      InitializeTimer();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
      if (IsLoading) return;

      IsLoading = true;
      ErrorMessage = string.Empty;
      await _loggingService.LogInfoAsync("Attempting to load sensor data for map.", null);

      try
      {
        var lookback = TimeSpan.FromMinutes(15);
        IEnumerable<SensorReadings> readings = await _sensorReadingService.GetRecentReadingsWithLocationAsync(lookback);
        UpdateMapMarkers(readings);
        await _loggingService.LogInfoAsync($"Map markers updated with {readings?.Count() ?? 0} readings.", null);
      }
      catch (Exception ex)
      {
        await _loggingService.LogErrorAsync("Failed to load sensor data in SensorMapViewModel.", ex, null);
        ErrorMessage = "Failed to display sensor data. Check logs for details.";
      }
      finally
      {
        IsLoading = false;
      }
    }

    private void UpdateMapMarkers(IEnumerable<SensorReadings> readings)
    {
      var readingList = readings?.ToList(); // Convert to list for easier checking
      int receivedCount = readingList?.Count ?? 0;
      _loggingService?.LogInfoAsync($"UpdateMapMarkers called. Received {receivedCount} readings.", null);

      if (receivedCount == 0)
      {
        SensorMarkers?.Clear();
        _loggingService?.LogInfoAsync("Cleared existing markers as no new readings received.", null);
        return;
      }

      SensorMarkers?.Clear(); // Clear before adding new ones

      int markersAdded = 0;
      foreach (var reading in readingList)
      {
        _loggingService?.LogInfoAsync($"Processing ReadingId: {reading.ReadingId}", null);
        // **Critical Check:** Log the config/location data presence
        bool hasLocation = reading.Config?.ConfigData != null;
        _loggingService?.LogInfoAsync($"ReadingId: {reading.ReadingId} - Has Location Data: {hasLocation}", null);

        if (hasLocation)
        {
          // Log the actual coordinates being used
          double lat = reading.Config.ConfigData.LocationLatitude;
          double lng = reading.Config.ConfigData.LocationLongitude;
          _loggingService?.LogInfoAsync($"ReadingId: {reading.ReadingId} - Lat: {lat}, Lng: {lng}. Creating marker.", null);

          var marker = new MapMarker
          {
            Latitude = lat,
            Longitude = lng,
            BindingContext = reading
          };

          // Add marker and increment count
          SensorMarkers?.Add(marker);
          markersAdded++;
        }
        else
        {
          _loggingService?.LogWarningAsync($"ReadingId: {reading.ReadingId} skipped - missing Config or ConfigData.", null);
        }
      }
      // Log the final count in the collection
      int finalMarkerCount = SensorMarkers?.Count ?? 0;
      _loggingService?.LogInfoAsync($"UpdateMapMarkers finished. Added {markersAdded} markers. Total markers in collection: {finalMarkerCount}", null);
    }

    private void InitializeTimer()
    {
      if (_refreshTimer == null)
      {
        if (Application.Current?.Dispatcher != null)
        {
          _refreshTimer = Application.Current.Dispatcher.CreateTimer();
        }
        else
        {
          throw new InvalidOperationException("Application.Current or its Dispatcher is null.");
        }
        _refreshTimer.Interval = TimeSpan.FromSeconds(RefreshIntervalSeconds);
        _refreshTimer.Tick += async (sender, e) => await LoadDataCommand.ExecuteAsync(null);
      }
    }

    public void StartUpdates()
    {
      _refreshTimer?.Start();
      if (LoadDataCommand.CanExecute(null))
      {
        _ = LoadDataCommand.ExecuteAsync(null);
      }
    }

    public void StopUpdates()
    {
      _refreshTimer?.Stop();
    }

  }
}