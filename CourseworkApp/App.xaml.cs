using CourseworkApp.Views; // Required if you navigate directly to LoginPage
using Microsoft.Maui.Controls;

namespace CourseworkApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Navigate to login screen on startup (clears nav stack)
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
