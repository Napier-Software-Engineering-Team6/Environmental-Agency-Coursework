namespace CourseworkApp;
using CourseworkApp.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("AdminConfig", typeof(AdminConfig));
		Routing.RegisterRoute("AdminConfig/ConfigForm", typeof(ConfigForm));
		Routing.RegisterRoute("AdminFirmware", typeof(AdminFirmware));
		Routing.RegisterRoute("AdminFirmware/FirmwareForm", typeof(FirmwareForm));
		Routing.RegisterRoute("SensorMap", typeof(SensorMap));
	}
}
