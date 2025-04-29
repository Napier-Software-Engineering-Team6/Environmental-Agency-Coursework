using CourseworkApp.ViewModels;
using Xunit;
using System;
using System.Linq;


//When the app starts, the Environmental Scientist page should show data for the ‘Air’ category by default, and both the table and chart should have some data visible

public class EnvironmentalScientistViewModelTests
{
    [Fact]
    public void DefaultCategory_IsSelected_AndDataIsPopulated()
    {
        var vm = new EnvironmentalScientistViewModel();
        Assert.Equal("Air", vm.SelectedCategory);
        Assert.NotEmpty(vm.DisplayedData);
        Assert.NotEmpty(vm.ChartData);
    }
//If the user switches from ‘Air’ to ‘Water’ in the category picker, the table and chart should immediately update to show only water sensor data
    [Fact]
    public void ChangingCategory_UpdatesDisplayedDataAndChartData()
    {
        var vm = new EnvironmentalScientistViewModel();
        vm.SelectedCategory = "Water";
        Assert.All(vm.DisplayedData, m => Assert.Equal("pH", m.Detail));
        Assert.All(vm.ChartData, c => Assert.Contains("2025-04", c.Label));
    }
//If the user selects a start and end date, only the data collected between those dates should appear in the table and chart
    [Fact]
    public void ChangingDateRange_FiltersDataCorrectly()
    {
        var vm = new EnvironmentalScientistViewModel();
        vm.SelectedCategory = "Weather";
        vm.StartDate = new DateTime(2025, 4, 2);
        vm.EndDate = new DateTime(2025, 4, 3);
        Assert.All(vm.DisplayedData, m =>
            Assert.True(DateTime.Parse(m.Date) >= vm.StartDate && DateTime.Parse(m.Date) <= vm.EndDate));
    }

//If the user picks a start date that’s after the current end date, the app should automatically move the end date forward so the range is always valid.
    [Fact]
    public void EndDate_CannotBeBeforeStartDate()
    {
        var vm = new EnvironmentalScientistViewModel();
        var originalEndDate = vm.EndDate;
        vm.StartDate = vm.EndDate.AddDays(1);
        Assert.Equal(vm.StartDate, vm.EndDate);
    }
}


