using CourseworkApp.ViewModels;
using Microsoft.Maui.Controls;

namespace CourseworkApp.Views;

public partial class SensorMap : ContentPage
{
	public SensorMap(SensorMapViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

	}
}