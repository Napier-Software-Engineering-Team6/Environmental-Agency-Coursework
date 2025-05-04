using Microsoft.Maui.Controls;
using CourseworkApp.ViewModels;

namespace CourseworkApp.Views
{
    public partial class EnvironmentalScientist : ContentPage
    {
        public EnvironmentalScientist(EnvironmentalScientistViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}