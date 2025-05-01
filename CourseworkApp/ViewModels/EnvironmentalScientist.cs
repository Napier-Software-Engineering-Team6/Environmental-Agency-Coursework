using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseworkApp.Database.Models;
using CourseworkApp.Database.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.ViewModels
{
    /// <summary>
    /// Developer - Stuart Clarkson
    /// Date - 2025-04-28
    /// Feature - Environmental Scientist
    /// ViewModel for the Environmental Scientist's historical data analysis feature.
    /// Implements MVVM using CommunityToolkit.Mvvm.
    /// </summary>
    public partial class EnvironmentalScientistViewModel : ObservableObject
    {
        /// <summary>
        /// The available sensor categories for selection (Air, Water, Weather).
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> categories = new() { "Air", "Water", "Weather" };

        /// <summary>
        /// The currently selected category.
        /// Changing this triggers data filtering and chart updates.
        /// </summary>
        [ObservableProperty]
        private string selectedCategory;

        /// <summary>
        /// The measurements currently displayed in the UI table, filtered by category and date range.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Measurement> displayedData;

        /// <summary>
        /// The start date for the selected date range filter
        /// </summary>
        [ObservableProperty]
        private DateTime startDate = DateTime.Now.AddDays(-7);

        /// <summary>
        /// The end date for the selected date range filter.
        /// </summary>
        [ObservableProperty]
        private DateTime endDate = DateTime.Now;

        /// <summary>
        /// The minimum date for the date range picker.
        /// </summary>
        [ObservableProperty]
        private DateTime minDate = DateTime.Now.AddMonths(-1);

        /// <summary>
        /// The maximum date for the date range picker.
        /// </summary>
        [ObservableProperty]
        private DateTime maxDate = DateTime.Now;

        /// <summary>
        /// Indicates whether data is currently loading
        /// </summary>
        [ObservableProperty]
        private bool isLoading;

        /// <summary>
        /// The data points for the chart, filtered by category and date range.
        /// </summary>
        public ObservableCollection<ChartPoint> ChartData { get; set; }

        /// <summary>
        /// Repository for accessing weather data from the database
        /// </summary>
        private readonly WeatherRepository _weatherRepository;

        /// <summary>
        /// A mock data source for testing purposes, simulating sensor data for different categories.
        /// </summary>
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

        /// <summary>
        /// Constructor initializes the ViewModel with default values and loads initial data.
        /// </summary>
        public EnvironmentalScientistViewModel(WeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
            ChartData = new ObservableCollection<ChartPoint>();
            SelectedCategory = Categories.First();
            LoadDataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Triggered when the selected category changes.
        /// </summary>
        partial void OnSelectedCategoryChanged(string value)
        {
            LoadDataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Triggered when the start date changes.
        /// Ensures the end date is not before the start date and updates data.
        /// </summary>
        partial void OnStartDateChanged(DateTime value)
        {
            if (EndDate < value)
                EndDate = value;
            LoadDataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Triggered when the end date changes.
        /// Ensures the start date is not after the end date and updates data.
        /// </summary>
        partial void OnEndDateChanged(DateTime value)
        {
            if (StartDate > value)
                StartDate = value;
            LoadDataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Loads data based on current filters, either from repository or mock data
        /// </summary>
        [RelayCommand]
        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                if (SelectedCategory == "Weather")
                {
                    await LoadWeatherDataFromRepositoryAsync();
                }
                else
                {
                    LoadMockData();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (could log or display error message)
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Loads weather data from the repository based on date range
        /// </summary>
        private async Task LoadWeatherDataFromRepositoryAsync()
        {
            var weatherData = await _weatherRepository.GetWeatherDataByDateRangeAsync(StartDate, EndDate);
            
            // Convert to displayed data format
            DisplayedData = new ObservableCollection<Measurement>(
                weatherData.Select(w => new Measurement
                {
                    Date = w.Time.ToString("yyyy-MM-dd HH:mm"),
                    Value = (double)w.Temperature_2m,
                    Detail = "Temp"
                })
            );

            // Update chart data
            ChartData.Clear();
            foreach (var data in weatherData)
            {
                ChartData.Add(new ChartPoint
                {
                    Label = data.Time.ToString("MM-dd HH:mm"),
                    Value = (double)data.Temperature_2m
                });
            }
        }

        /// <summary>
        /// Loads mock data for non-Weather categories
        /// </summary>
        private void LoadMockData()
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

    /// <summary>
    /// Represents a single environmental measurement for tabular display.
    /// </summary>
    public class Measurement
    {
        public string Date { get; set; }
        public double Value { get; set; }
        public string Detail { get; set; }
    }

    /// <summary>
    /// Represents a single data point for chart visualization.
    /// </summary>
    public class ChartPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
