using System;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        private readonly TripDetailViewModel viewModel;

        public ItemDetailPage(TripDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private async void DeleteItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ContinueWith(_ => MessagingCenter.Send(this, "DeleteTrip", viewModel.Trip));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ChecklistGroups.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}