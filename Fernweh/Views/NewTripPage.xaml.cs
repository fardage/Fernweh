using System;
using dotMorten.Xamarin.Forms;
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
            await Navigation.PushAsync(new NavigationPage(setupPage));
        }

        private void AutoSuggestBox_TextChanged(object o, AutoSuggestBoxTextChangedEventArgs args)
        {
            viewModel.ExecuteTextChangedCommand((AutoSuggestBox) o, args);
        }

        private void AutoSuggestBox_SuggestionChosen(object o, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            viewModel.ExecuteSuggestionChosenCommand((AutoSuggestBox) o, args);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.UpdateCountrySuggestionsAsync();
        }
    }
}