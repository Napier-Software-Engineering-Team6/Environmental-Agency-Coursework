using CourseworkApp.Enums;

namespace CourseworkApp.Common
{
  public class ValidationResult
  {
    public ValidationStatus Status { get; set; }
    public List<string> Errors { get; set; } = new();

    public bool IsValid => Status == ValidationStatus.Success;
  }

}