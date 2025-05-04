using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Maps;

namespace CourseworkApp.ViewModels;

public partial class SensorMapViewModel : ObservableObject
{
  [ObservableProperty]
  private Location initialLocation;

  [ObservableProperty]
  private Distance initialRadius;

  public SensorMapViewModel()
  {
    InitialLocation = new Location(55.9486, -3.2021);
    InitialRadius = Distance.FromKilometers(2);
  }
}