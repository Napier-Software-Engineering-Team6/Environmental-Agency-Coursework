using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Models;
using CourseworkApp.Services;
using CourseworkApp.Views;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels
{
    /// <summary>
    /// ViewModel responsible for handling user login input and authentication logic.
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILoginService _loginService;

        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        [ObservableProperty]
        private string? username;

        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        [ObservableProperty]
        private string? password;

        /// <summary>
        /// Error message shown if login fails.
        /// </summary>
        [ObservableProperty]
        private string? errorMessage;

        /// <summary>
        /// Controls the visibility of the error message.
        /// </summary>
        [ObservableProperty]
        private bool isErrorVisible;

        /// <summary>
        /// Constructs a new instance of the LoginViewModel with the given login service.
        /// </summary>
        /// <param name="loginService">The authentication service to use.</param>
        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Authenticates the user and updates the UI based on their role.
        /// </summary>
        [RelayCommand]
        private async Task Login()
        {
            var user = _loginService.Authenticate(Username ?? "", Password ?? "");

            if (user != null)
            {
                SessionService.LoggedInUser = user;

                // Rebuild the AppShell so the correct tabs are shown
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorMessage = "Invalid credentials";
                IsErrorVisible = true;
            }
        }
    }
}
