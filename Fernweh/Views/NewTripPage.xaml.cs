using System;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class NewItemPage : ContentPage
    {
        private readonly NewTripViewModel viewModel;

        public NewItemPage(NewTripViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, viewModel.NewTrip));
            await Navigation.PushModalAsync(new NavigationPage(setupPage));
        }
    }
}