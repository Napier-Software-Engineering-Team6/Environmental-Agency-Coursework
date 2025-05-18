using CommunityToolkit.Mvvm.ComponentModel;
using CourseworkApp.Database.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseworkApp.ViewModels;

/// \file MainPageViewModel.cs
/// \brief ViewModel for the MainPage of the application.

/// <summary>
/// ViewModel for the MainPage, responsible for handling data and business logic. 
/// </summary>
/// <param name="CourseDbContext">The database context used for data access.</param>
public partial class MainPageViewModel : ObservableObject
{

  private readonly CourseDbContext _context; // can update db context here to use different db context
  private bool _isInitialized = false;

  [ObservableProperty]
  private string _title = "MAUI DB Test";

  [ObservableProperty]
  private string _description = "Initializing...";

  /// <summary>
  /// Constructor for MainPageViewModel. Initializes the database context.
  /// </summary>
  /// <param name="context">Injected CourseDbContext instance.</param>
  public MainPageViewModel(CourseDbContext context) //need to ensure correct db context
  {
    _context = context;
    Debug.WriteLine(">>> MainPageViewModel Constructor: Finished.");
  }
  /**
     * @brief Initializes the ViewModel asynchronously. Ensures data is loaded only once.
     * @return A Task representing the asynchronous operation.
     * @remarks This method should be called when the associated view appears. It calls LoadDataAsync.
     */
  public async Task InitializeAsync()
  {
    if (_isInitialized)
      return;

    Debug.WriteLine(">>> InitializeAsync: Starting initial data load...");
    try
    {
      await LoadDataAsync(2); // update with the ID you want to load
      _isInitialized = true;
      Debug.WriteLine(">>> InitializeAsync: Initial data load complete.");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"!!! InitializeAsync: FAILED - {ex}");
      Description = "Failed to load initial data.";
    }
  }

  private async Task LoadDataAsync(int id)
  {
    Debug.WriteLine($">>> LoadDataAsync called with id: {id}");
    try
    {
      Debug.WriteLine($">>> LoadDataAsync: Querying for id: {id}");
      var record = await _context.MainPageDB
        .FirstOrDefaultAsync(mp => mp.Id == id);
      Debug.WriteLine($">>> LoadDataAsync: Query completed for id: {id}. Record found: {record != null}");

      if (record != null)
      {
        Title = $"Record ID: {record.Id}";
        Description = record.Text;
      }
      else
      {
        Description = $"Record with ID {id} not found.";
      }
    }
    catch (Exception ex)
    {
      Description = $"Error loading data: {ex.Message}";
      Debug.WriteLine($"!!! Error accessing database in LoadDataAsync for id {id}: {ex}");
    }
    finally
    {
      Debug.WriteLine($">>> LoadDataAsync finished for id: {id}");
    }
  }
}
