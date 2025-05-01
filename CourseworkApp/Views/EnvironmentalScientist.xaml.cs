using CourseworkApp.ViewModels;

namespace CourseworkApp.Views
{
    public partial class EnvironmentalScientist : ContentPage
    {
        public EnvironmentalScientist(EnvironmentalScientistViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Set the ViewModel as the BindingContext
        }
    }
}
