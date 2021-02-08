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
            await Navigation.PopAsync();
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, viewModel.NewTrip));
            await Navigation.PushAsync(new NavigationPage(setupPage));
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SearchDestinationPage()));
        }
    }
}