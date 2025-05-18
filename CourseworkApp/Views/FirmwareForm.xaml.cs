using CourseworkApp.ViewModels;
namespace CourseworkApp.Views;

/// <summary>
/// FirmwareForm.xaml code-behind.
/// This page is responsible for displaying and managing firmware configurations.
/// </summary>
public partial class FirmwareForm : ContentPage
{
	/// <summary>
	/// Initializes a new instance of the FirmwareForm class.
	/// This constructor sets up the page and binds it to the FirmwareFormViewModel.
	/// </summary>
	public FirmwareForm(FirmwareFormViewModel viewModel)
	{
		BindingContext = viewModel;
		{
			InitializeComponent();
		}
	}
}