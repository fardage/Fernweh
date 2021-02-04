using System;
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
            MessagingCenter.Send(this, "ItemSelected", e.Item.ToString());
            Navigation.PopModalAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}