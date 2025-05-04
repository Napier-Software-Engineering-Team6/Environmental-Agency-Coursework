using System;
using System.Threading.Tasks;
using Moq;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using CourseworkApp.ViewModels;

namespace CourseworkApp.Test.Components.ViewModels;
public class TestableBaseFormViewModel : BaseFormViewModel
{
  public Func<Task<bool>> ValidateAsyncFunc { get; set; } = () => Task.FromResult(true);
  public Func<Task<bool>> SaveAsyncFunc { get; set; } = () => Task.FromResult(true);
  public Func<string> GetEntityTypeFunc { get; set; } = () => "TestEntity";

  public TestableBaseFormViewModel(INavigationService navigationService, ILoggingService loggingService)
      : base(navigationService, loggingService) { }

  // Implement abstract methods by invoking the delegates
  protected override Task<bool> ValidateAsync() => ValidateAsyncFunc();
  protected override Task<bool> SaveAsync() => SaveAsyncFunc();
  protected override string GetEntityType() => GetEntityTypeFunc();

  public bool SaveAsyncCalled { get; }

}