// Namespaces: CourseworkApp.Services or CourseworkApp.Services.Interfaces
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  /// <summary>
  /// Interface for sensor history service.
  /// This interface defines methods for logging sensor actions and retrieving sensor history.
  /// </summary>
  public interface ISensorHistoryService
  {
    Task LogActionAsync(int? configId, int? firmwareId, string actionType, string status, string details, string performedBy);
  }
}