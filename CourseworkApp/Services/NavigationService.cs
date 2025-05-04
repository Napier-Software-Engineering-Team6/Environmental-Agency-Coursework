using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Data.Common;

namespace CourseworkApp.Services;
/// <summary>
/// Interface for navigation service.
/// This interface defines methods for navigating between pages in the application.
/// </summary>
public class NavigationService : INavigationService
{
  /// <summary>
  /// Navigates to a specified route with optional parameters.
  /// This method uses the Shell.Current instance to perform navigation.  
  /// </summary>
  /// <param name="route"></param>
  /// <param name="parameters"></param>
  /// <returns></returns>
  public Task GoToAsync(string route, IDictionary<string, object?>? parameters = null)
  {
    if (Shell.Current == null)
    {
      System.Diagnostics.Debug.WriteLine("Error: Shell.Current is null. Cannot navigate.");
      return Task.CompletedTask;
    }

    if (parameters == null)
    {
      return Shell.Current.GoToAsync(route);
    }
    else
    {
      return Shell.Current.GoToAsync(route, parameters);
    }
  }
  /// <summary>
  /// Navigates back to the previous page in the navigation stack.
  /// This method uses the Shell.Current instance to perform navigation.
  /// </summary>
  /// <returns></returns>
  public Task NavigateBackAsync()
  {
    if (Shell.Current == null)
    {
      System.Diagnostics.Debug.WriteLine("Error: Shell.Current is null. Cannot navigate back.");
      return Task.CompletedTask;
    }

    return Shell.Current.GoToAsync("..");
  }
  /// <summary>
  /// Navigates back to the root page of the navigation stack.
  /// This method uses the Shell.Current instance to perform navigation.  
  /// </summary>
  /// <returns></returns>
  public Task NavigateBackToRootAsync()
  {
    if (Shell.Current == null)
    {
      System.Diagnostics.Debug.WriteLine("Error: Shell.Current is null. Cannot navigate back to root.");
      return Task.CompletedTask;
    }

    return Shell.Current.Navigation.PopToRootAsync();
  }
}
