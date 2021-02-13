using System;
using Fernweh.Services.HereMaps;
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

            MessagingCenter.Subscribe<SearchDestinationPage, Suggestion>(this, "ItemSelected",
                (obj, selected) =>
                {
                    var searchBar = this.FindByName<Label>("DestinationSearchBar");

                    if (!string.IsNullOrEmpty(selected.Address.City))
                        searchBar.Text = " " + selected.Address.City;
                    else if (!string.IsNullOrEmpty(selected.Address.State))
                        searchBar.Text = " " + selected.Address.State;
                    else if (!string.IsNullOrEmpty(selected.Address.CountryName))
                        searchBar.Text = " " + selected.Address.CountryName;
                    else
                        searchBar.Text = " " + selected.Label;
                });
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, viewModel.NewTrip));
            await Navigation.PushAsync(setupPage);
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SearchDestinationPage()));
        }
    }
}