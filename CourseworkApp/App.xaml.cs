namespace CourseworkApp;

public partial class App : Application
{
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCeEx1WmFZfVtgdV9DZ1ZVR2Y/P1ZhSXxWdkBjXX5ddX1RQmhVWEB9XUs=");

		InitializeComponent();

		MainPage = new AppShell();
	}
}
