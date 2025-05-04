using CourseworkApp.ViewModels;
using System.Diagnostics;

namespace CourseworkApp.Views;

/// <summary>
/// Represents the main page of the application.
/// </summary>
/// <remarks>
/// This page is responsible for displaying the main content of the app.
/// </remarks>
/// <seealso cref="CourseworkApp.ViewModels.MainPageViewModel"/>
public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    // 1️⃣ Parameterless constructor for MAUI/XAML
    public MainPage()
    {
        InitializeComponent();
    }

    // 2️⃣ DI constructor for ViewModel injection
    public MainPage(MainPageViewModel viewModel) : this()
    {
        Debug.WriteLine(">>> MainPage Constructor: START");
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
        Debug.WriteLine(">>> MainPage Constructor: AFTER BindingContext set");
    }

    protected override async void OnAppearing()
    {
        Debug.WriteLine(">>> MainPage OnAppearing: START");
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.InitializeAsync();
            Debug.WriteLine(">>> MainPage OnAppearing: END (after InitializeAsync)");
        }
        else
        {
            Debug.WriteLine(">>> MainPage OnAppearing: ViewModel is NULL - Skipping InitializeAsync");
        }
    }
}
