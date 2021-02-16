using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class AddSharedTripPage : ContentPage
    {
        private readonly AddSharedTripViewModel viewModel;

        public AddSharedTripPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AddSharedTripViewModel(Navigation);
        }
    }
}