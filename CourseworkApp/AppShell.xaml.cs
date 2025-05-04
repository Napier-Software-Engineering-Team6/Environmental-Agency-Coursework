using CourseworkApp.Services;
using CourseworkApp.Views;

namespace CourseworkApp;

/// <summary>
/// Main Shell class for the application, defining navigation structure and handling role-based tab visibility.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Initializes the application shell, registers routes, configures UI based on user role, and adds logout support.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();

        // Register page routes used in admin tab
        Routing.RegisterRoute("AdminConfig", typeof(AdminConfig));
        Routing.RegisterRoute("AdminConfig/ConfigForm", typeof(ConfigForm));

        // Hide the Admin tab if the logged-in user is not an Admin
        if (SessionService.LoggedInUser?.Role != Models.UserRole.Admin)
        {
            AdminTab.IsVisible = false;
        }

        // Add logout toolbar item
        var logoutItem = new ToolbarItem
        {
            Text = "Logout",
            Command = new Command(Logout)
        };
        ToolbarItems.Add(logoutItem);
    }

    /// <summary>
    /// Logs the user out, clears the session, and navigates back to the LoginPage.
    /// </summary>
    private void Logout()
    {
        SessionService.LoggedInUser = null;
        Application.Current.MainPage = new AppShell(); // Reset nav stack
        Shell.Current.GoToAsync("//LoginPage");
    }
}
