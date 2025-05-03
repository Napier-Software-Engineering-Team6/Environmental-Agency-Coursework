

using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Services;
using CourseworkApp.Enums;

namespace CourseworkApp.ViewModels;

public abstract partial class BaseFormViewModel : ObservableObject

{

	protected readonly INavigationService _navigationService;
	protected readonly ILoggingService _loggingService;

	[ObservableProperty]
	private string errorMessage = string.Empty;
	[ObservableProperty]
	private string successMessage = string.Empty;
	[ObservableProperty]
	private bool isBusy;

	protected const string SubmitAction = "Submit";
	protected BaseFormViewModel(INavigationService navigationService, ILoggingService loggingService)
	{
		_navigationService = navigationService;
		_loggingService = loggingService;
	}

	protected async Task LogActionAsync(string action, string statusString, string message)

	{
		if (Enum.TryParse<ActionStatus>(statusString, true, out ActionStatus statusEnum))
		{
			await _loggingService.LogUserActionAsync(action, statusEnum, message);
		}
		else
		{
			await _loggingService.LogErrorAsync($"Failed to parse ActionStatus: {statusString} for action: {action}");
		}
	}

	protected abstract Task<bool> ValidateAsync();
	protected abstract Task<bool> SaveAsync();
	protected abstract string GetEntityType();

	[RelayCommand]
	private async Task Submit()
	{
		ErrorMessage = string.Empty;
		SuccessMessage = string.Empty;
		IsBusy = true;

		const string Failed = "Failed";
		const string Success = "Success";



		try
		{
			if (!await ValidateAsync())
			{
				await LogActionAsync(SubmitAction, Failed, ErrorMessage);
				return;
			}

			var result = await SaveAsync();
			if (result)
			{
				SuccessMessage = "Operation completed successfully.";
				await LogActionAsync(SubmitAction, Success, SuccessMessage);
				await _navigationService.NavigateBackAsync();
			}
			else
			{
				ErrorMessage = "Failed to save changes.";
				await LogActionAsync(SubmitAction, Failed, ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Error: {ex.Message}";
			await LogActionAsync(SubmitAction, Failed, ErrorMessage);
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task Cancel()
	{
		await LogActionAsync("Cancel", "Success", $"{GetEntityType()} form cancelled.");
		await _navigationService.NavigateBackAsync();
	}


}