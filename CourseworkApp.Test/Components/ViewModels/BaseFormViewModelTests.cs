using Xunit;
using Moq;
using CourseworkApp.Services;
using CourseworkApp.Enums;
using CourseworkApp.ViewModels;
using System.Threading.Tasks;

namespace CourseworkApp.Test.Components.ViewModels
{
  public class BaseFormViewModelTests
  {
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly Mock<ILoggingService> _mockLoggingService;
    private readonly TestableBaseFormViewModel _viewModel;

    public BaseFormViewModelTests()
    {
      _mockNavigationService = new Mock<INavigationService>();
      _mockLoggingService = new Mock<ILoggingService>();

      _viewModel = new TestableBaseFormViewModel(
          _mockNavigationService.Object,
          _mockLoggingService.Object);
    }

    [Fact]
    public async Task Submit_WhenValidationFails_SetsErrorAndLogs()
    {
      // Arrange
      _viewModel.ValidateAsyncFunc = () =>
      {
        _viewModel.ErrorMessage = "Validation Failed Here";
        return Task.FromResult(false);
      };

      // Act
      await _viewModel.SubmitCommand.ExecuteAsync(null);

      // Assert
      Assert.Equal("Validation Failed Here", _viewModel.ErrorMessage);
      Assert.Empty(_viewModel.SuccessMessage);
      Assert.False(_viewModel.IsBusy);

      _mockLoggingService.Verify(l => l.LogUserActionAsync("Submit", ActionStatus.Failed, "Validation Failed Here", null), Times.Once);
      _mockNavigationService.Verify(n => n.NavigateBackAsync(), Times.Never);

    }


    [Fact]
    public async Task Cancel_LogsAndNavigatesBack()
    {
      // Arrange
      _viewModel.GetEntityTypeFunc = () => "SpecificItem";

      // Act
      await _viewModel.CancelCommand.ExecuteAsync(null);

      // Assert
      _mockLoggingService.Verify(l => l.LogUserActionAsync("Cancel", ActionStatus.Success, "SpecificItem form cancelled.", null), Times.Once);
      _mockNavigationService.Verify(n => n.NavigateBackAsync(), Times.Once);
    }
  }
}