using CourseworkApp.ViewModels;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CourseworkApp.Views;
/// <summary>
/// This class represents the configuration form for the application. It initializes the view model and sets it as the binding context for the page.
/// </summary>
public partial class ConfigForm : ContentPage
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="viewModel"></param>
	public ConfigForm(ConfigFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}