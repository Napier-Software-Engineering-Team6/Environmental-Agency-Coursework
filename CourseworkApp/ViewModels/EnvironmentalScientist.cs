using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CourseworkApp.ViewModels
{
    public partial class EnvironmentalScientistViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> categories = new() { "Air", "Water", "Weather" };

        [ObservableProperty]
        private string selectedCategory;

        [ObservableProperty]
        private ObservableCollection<Measurement> displayedData;

        // Chart data for the graph
        public ObservableCollection<ChartPoint> ChartData { get; set; }

        // Mock data for each category
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
                    new Measurement { Date = "2025-04-01", Value = 7.2, Detail = "pH" },
                    new Measurement { Date = "2025-04-02", Value = 6.2, Detail = "pH" }
                },
                ["Weather"] = new ObservableCollection<Measurement>
                {
                    new Measurement { Date = "2025-04-01", Value = 12, Detail = "Temp" },
                    new Measurement { Date = "2025-04-02", Value = 17, Detail = "Temp" },
                    new Measurement { Date = "2025-04-03", Value = 12, Detail = "Temp" },
                    new Measurement { Date = "2025-04-04", Value = 13, Detail = "Temp" }
                }
            };

        public EnvironmentalScientistViewModel()
        {
            // Set default category and data
            SelectedCategory = Categories.First();
            DisplayedData = mockDataByCategory[SelectedCategory];

            // Initialize chart data
            ChartData = new ObservableCollection<ChartPoint>
            {
                new ChartPoint { Label = "Mon", Value = 12 },
                new ChartPoint { Label = "Tue", Value = 15 },
                new ChartPoint { Label = "Wed", Value = 11 },
                new ChartPoint { Label = "Thu", Value = 18 },
                new ChartPoint { Label = "Fri", Value = 14 }
            };
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            // Update displayed data when category changes
            if (value != null && mockDataByCategory.ContainsKey(value))
                DisplayedData = mockDataByCategory[value];
        }
    }

    public class Measurement
    {
        public string Date { get; set; }
        public double Value { get; set; }
        public string Detail { get; set; }
    }

    public class ChartPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
