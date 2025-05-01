using Xunit;
using CourseworkApp.ViewModels;
using CourseworkApp.Database.Repositories;
using System;

public class EnvironmentalScientistViewModelSimpleTests
{
    // Helper: Provide a dummy WeatherRepository (it won't be used for Air/Water)
    private WeatherRepository DummyRepo() => null;

    [Fact]
    public void Constructor_Sets_Default_Category_And_ChartData()
    {
        var vm = new EnvironmentalScientistViewModel(DummyRepo());
        Assert.Equal("Air", vm.SelectedCategory);
        Assert.NotNull(vm.Categories);
        Assert.Contains("Air", vm.Categories);
        Assert.NotNull(vm.ChartData);
    }

    [Fact]
    public void Setting_SelectedCategory_To_Air_Loads_Mock_Data()
    {
        var vm = new EnvironmentalScientistViewModel(DummyRepo())
        {
            SelectedCategory = "Air",
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2025, 4, 4)
        };

        // Force data reload
        vm.GetType().GetMethod("LoadMockData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(vm, null);

        Assert.NotNull(vm.DisplayedData);
        Assert.Equal(4, vm.DisplayedData.Count); // 4 mock Air measurements
        Assert.All(vm.DisplayedData, m => Assert.Equal("Air", vm.SelectedCategory));
    }

    [Fact]
    public void Setting_SelectedCategory_To_Water_Loads_Mock_Data()
    {
        var vm = new EnvironmentalScientistViewModel(DummyRepo())
        {
            SelectedCategory = "Water",
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2025, 4, 4)
        };

        // Force data reload
        vm.GetType().GetMethod("LoadMockData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(vm, null);

        Assert.NotNull(vm.DisplayedData);
        Assert.Equal(4, vm.DisplayedData.Count); // 4 mock Water measurements
        Assert.All(vm.DisplayedData, m => Assert.Equal("Water", vm.SelectedCategory));
    }

    [Fact]
    public void StartDate_After_EndDate_Adjusts_EndDate()
    {
        var vm = new EnvironmentalScientistViewModel(DummyRepo())
        {
            EndDate = new DateTime(2025, 4, 10)
        };

        vm.StartDate = new DateTime(2025, 4, 15);

        Assert.Equal(vm.StartDate, vm.EndDate);
    }

    [Fact]
    public void EndDate_Before_StartDate_Adjusts_StartDate()
    {
        var vm = new EnvironmentalScientistViewModel(DummyRepo())
        {
            StartDate = new DateTime(2025, 4, 20)
        };

        vm.EndDate = new DateTime(2025, 4, 15);

        Assert.Equal(vm.EndDate, vm.StartDate);
    }
}
