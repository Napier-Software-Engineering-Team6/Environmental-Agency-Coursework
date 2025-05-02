// In CourseworkApp.Services (or a sub-namespace)
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  // Define a simple result class (could be shared or specific)
  public class UpdateResult
  {
    public bool Success { get; set; }
    public string Message { get; set; } // Can hold error or success details
  }

  public interface ISensorUpdateService
  {
    Task<UpdateResult> AttemptApplyConfigurationToSensorAsync(
        string sensorIdentifier,
        int configId,
        string appliedByUserId);
  }
}