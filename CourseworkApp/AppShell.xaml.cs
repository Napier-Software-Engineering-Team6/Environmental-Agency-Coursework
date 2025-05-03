using CourseworkApp.Views; // Ensure this namespace is correct and matches the location of AdminConfigPage

namespace CourseworkApp;


public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("AdminConfig", typeof(AdminConfig));
		Routing.RegisterRoute("AdminConfig/ConfigForm", typeof(ConfigForm));
		Routing.RegisterRoute("AdminFirmware", typeof(AdminFirmware));
		Routing.RegisterRoute("AdminFirmware/FirmwareForm", typeof(FirmwareForm));
	}
}
