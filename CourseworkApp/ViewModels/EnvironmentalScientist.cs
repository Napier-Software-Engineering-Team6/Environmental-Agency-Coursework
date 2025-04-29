using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CourseworkApp.ViewModels

    /// Developer - Stuart Clarkson
    /// Date - 2025-04-28
    /// Feature - Environmental Scientist
    /// ViewModel for the Environmental Scientist's historical data analysis feature.
    /// Implements MVVM using CommunityToolkit.Mvvm.
    /// </summary>





{
    public partial class EnvironmentalScientistViewModel : ObservableObject
    {
         /// The available sensor categories for selection (Air, Water, Weather).
        [ObservableProperty]
        private ObservableCollection<string> categories = new() { "Air", "Water", "Weather" };

        /// The currently selected category.
        /// Changing this triggers data filtering and chart updates.
        [ObservableProperty]
        private string selectedCategory;

        /// The measurements currently displayed in the UI table, filtered by category and date range.
        [ObservableProperty]
        private ObservableCollection<Measurement> displayedData;

        /// The start date for the selected date range filter
        [ObservableProperty]
        private DateTime startDate = DateTime.Now.AddDays(-7);

        /// The end date for the selected date range filter.
        [ObservableProperty]
        private DateTime endDate = DateTime.Now;

        /// The minimum date for the date range picker.
        [ObservableProperty]
        private DateTime minDate = DateTime.Now.AddMonths(-1);

        /// The maximum date for the date range picker.
        [ObservableProperty]
        private DateTime maxDate = DateTime.Now;

        /// The data points for the chart, filtered by category and date range.
        public ObservableCollection<ChartPoint> ChartData { get; set; }
        
        
        
        /// A mock data source for testing purposes, simulating sensor data for different categories.
        
        private readonly Dictionary<string, ObservableCollection<Measurement>> mockDataByCategory =
            new()
            {
                ["Air"] = new ObservableCollection<Measurement>
                {
                    new Measurement { Date = "2025-04-01", Value = 10, Detail = "PM2.5" },
                    new Measurement { Date = "2025-04-02", Value = 15, Detail = "PM10" },
                    new Measurement { Date = "2025-04-03", Value = 10, Detail = "PM2.9" },
                    new Measurement { Date = "2025-04-04", Value = 15, Detail = "PM7.1" }
                },
                ["Water"] = new ObservableCollection<Measurement>
                {
                    new Measurement { Date = "2025-04-01", Value = 7.0, Detail = "pH" },
                    new Measurement { Date = "2025-04-02", Value = 6.9, Detail = "pH" },
                    new Measurement { Date = "2025-04-03", Value = 7.2, Detail = "pH" },
                    new Measurement { Date = "2025-04-04", Value = 6.2, Detail = "pH" }
                },
                ["Weather"] = new ObservableCollection<Measurement>
                {
                    new Measurement { Date = "2025-04-01", Value = 12, Detail = "Temp" },
                    new Measurement { Date = "2025-04-02", Value = 17, Detail = "Temp" },
                    new Measurement { Date = "2025-04-03", Value = 12, Detail = "Temp" },
                    new Measurement { Date = "2025-04-04", Value = 13, Detail = "Temp" }
                }
            };
        /// Constructor initializes the ViewModel with default values and populates the chart data.
        public EnvironmentalScientistViewModel()
        {
            ChartData = new ObservableCollection<ChartPoint>();
            SelectedCategory = Categories.First();
            UpdateDisplayedData();
        }
     /// Triggered when the selected category changes.
        /// Updates displayed and chart data.
        partial void OnSelectedCategoryChanged(string value)
        {
            UpdateDisplayedData();
        }
        /// Triggered when the start date changes.
        /// Ensures the end date is not before the start date and updates data.
        partial void OnStartDateChanged(DateTime value)
        {
            if (EndDate < value)
                EndDate = value;
            UpdateDisplayedData();
        }
        /// Triggered when the end date changes.
        /// Ensures the start date is not after the end date and updates data.
        partial void OnEndDateChanged(DateTime value)
        {
            if (StartDate > value)
                StartDate = value;
            UpdateDisplayedData();
        }
        /// Filters the mock data based on the selected category and date range,
        /// then updates both the table and chart data.
        private void UpdateDisplayedData()
        {
            if (SelectedCategory == null || !mockDataByCategory.ContainsKey(SelectedCategory))
                return;

            // Filter by date range
            var filtered = mockDataByCategory[SelectedCategory]
                .Where(m => DateTime.TryParse(m.Date, out var dt) && dt >= StartDate && dt <= EndDate)
                .ToList();

            DisplayedData = new ObservableCollection<Measurement>(filtered);

            // Update chart data
            ChartData.Clear();
            foreach (var m in filtered)
            {
                ChartData.Add(new ChartPoint { Label = m.Date, Value = m.Value });
            }
        }
    }
        /// Represents a single environmental measurement for tabular display.
    public class Measurement
    {
        public string Date { get; set; }
        public double Value { get; set; }
        public string Detail { get; set; }
    }
   /// Represents a single data point for chart visualization.
    public class ChartPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
