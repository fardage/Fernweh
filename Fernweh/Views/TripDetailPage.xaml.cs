using System;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class TripDetailPage : ContentPage
    {
        private readonly TripDetailViewModel viewModel;

        public TripDetailPage(TripDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private async void DeleteItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ContinueWith(_ => MessagingCenter.Send(this, "DeleteTrip", viewModel.Trip));
        }
    }
}