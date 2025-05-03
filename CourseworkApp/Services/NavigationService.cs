using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Data.Common;

namespace CourseworkApp.Services;

public class NavigationService : INavigationService
{
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

  public Task NavigateBackAsync()
  {
    if (Shell.Current == null)
    {
      System.Diagnostics.Debug.WriteLine("Error: Shell.Current is null. Cannot navigate back.");
      return Task.CompletedTask;
    }

    return Shell.Current.GoToAsync("..");
  }

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
