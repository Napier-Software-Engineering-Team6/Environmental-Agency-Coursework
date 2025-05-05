using CourseworkApp.ViewModels;
using Syncfusion.Maui.Maps;
using Microsoft.Maui.Maps; // Keep for Location type from ViewModel if used
using System;
using System.Collections.ObjectModel; // Needed for MapMarkerCollection potentially, though Syncfusion might have its own

namespace CourseworkApp.Views
{
	public partial class SensorMap : ContentPage
	{
		readonly SensorMapViewModel _viewModel;

		public SensorMap(SensorMapViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
			_viewModel = viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel?.StartUpdates();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_viewModel?.StopUpdates();
		}
	}
}