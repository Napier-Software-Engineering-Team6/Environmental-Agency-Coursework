// Namespaces: CourseworkApp.Services or CourseworkApp.Services.Interfaces
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  public interface ISensorHistoryService
  {
    Task LogActionAsync(int? configId, int? firmwareId, string actionType, string status, string details, string performedBy);
  }
}