using System.Threading.Tasks;
using System.Collections.Generic;

namespace CourseworkApp.Services;
/// <summary>
/// 
/// </summary>
public interface INavigationService
{
  /// <summary>
  /// Navigates to a specified route with optional parameters.
  /// This method is used to navigate to different pages in the application.
  /// </summary>
  /// <param name="route"></param>
  /// <param name="parameters"></param>
  /// <returns></returns>
  Task GoToAsync(string route, IDictionary<string, object?>? parameters = null);
  /// <summary>
  /// Navigates back to the previous page in the navigation stack.
  /// This method is used to return to the previous page after navigating to a new one.
  /// </summary>
  /// <returns></returns>
  Task NavigateBackAsync();
  /// <summary>
  /// Navigates back to the root page in the navigation stack.
  /// This method is used to return to the main page of the application.
  /// </summary>
  /// <returns></returns>
  Task NavigateBackToRootAsync();
}
