using CourseworkApp.Enums;

namespace CourseworkApp.Common
{
  /// <summary>
  /// Represents the result of a validation process.
  public class ValidationResult
  {
    public ValidationStatus Status { get; set; }
    public List<string> Errors { get; set; } = new();

    public bool IsValid => Status == ValidationStatus.Success;
  }

}