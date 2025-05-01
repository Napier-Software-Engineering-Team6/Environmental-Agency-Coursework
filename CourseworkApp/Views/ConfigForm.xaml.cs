using CourseworkApp.ViewModels;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CourseworkApp.Views;

public partial class ConfigForm : ContentPage
{

	public ConfigForm(ConfigFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}