using CourseworkApp.ViewModels;
namespace CourseworkApp.Views;


public partial class FirmwareForm : ContentPage
{
	public FirmwareForm(FirmwareFormViewModel viewModel)
	{
		BindingContext = viewModel;
		{
			InitializeComponent();
		}
	}
}