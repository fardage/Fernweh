using System;
using Fernweh.Services.HereMaps;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class SearchDestinationPage : ContentPage
    {
        private readonly SearchDestinationViewModel viewModel;

        public SearchDestinationPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SearchDestinationViewModel();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = (Suggestion) e.Item;
            MessagingCenter.Send(this, "ItemSelected", selected);
            Navigation.PopModalAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}