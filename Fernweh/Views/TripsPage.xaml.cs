using System;
using Fernweh.Models;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class TripsPage : ContentPage
    {
        private readonly TripsViewModel viewModel;

        public TripsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TripsViewModel();
        }

        private async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject) sender;
            var item = (Trip) layout.BindingContext;
            await Navigation.PushAsync(new TripDetailPage(new TripDetailViewModel(item)));
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(new NewTripViewModel())));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Trips.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}