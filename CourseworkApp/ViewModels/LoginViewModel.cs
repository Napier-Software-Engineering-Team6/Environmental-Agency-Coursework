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
            var user = _loginService.Authenticate(Username ?? "", Password ?? ""); //This prevents nulls from ever reaching Authenticate.

            if (user != null)
            {
                SessionService.LoggedInUser = user;

                if (user.Role == UserRole.Admin)
                    await Shell.Current.GoToAsync($"//AdminPage");
                else
                    await Shell.Current.GoToAsync($"//HomePage");
            }
            else
            {
                ErrorMessage = "Invalid credentials";
                IsErrorVisible = true;
            }
        }
    }
}
