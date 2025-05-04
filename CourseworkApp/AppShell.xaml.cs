using CourseworkApp.Services;
using CourseworkApp.Views; // Ensure this includes AdminConfig and others

namespace CourseworkApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

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

	private void Logout()
    {
        SessionService.LoggedInUser = null;
        Application.Current.MainPage = new AppShell(); // Reset nav stack
        Shell.Current.GoToAsync("//LoginPage");
    }
    
}
