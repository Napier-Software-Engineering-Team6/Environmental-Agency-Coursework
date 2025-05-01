using System.Threading.Tasks;
using System.Collections.Generic;

namespace CourseworkApp.Services;

public interface INavigationService
{
  Task GoToAsync(string route, IDictionary<string, object?> parameters = null);

  Task NavigateBackAsync();

  Task NavigateBackToRootAsync();
}
