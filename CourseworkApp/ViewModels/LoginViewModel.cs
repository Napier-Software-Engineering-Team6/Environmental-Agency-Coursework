using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Models;
using CourseworkApp.Services;
using CourseworkApp.Views;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILoginService _loginService;

        [ObservableProperty]
        private string? username;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        private bool isErrorVisible;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

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
