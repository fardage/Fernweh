using System;
using System.Threading.Tasks;
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

        private async void Share_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Trip.IsShared)
            {
                await DisplayAlert("Share Checklist", $"Already being shared:\n{viewModel.Trip.Id}", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Share Checklist?", "Want to share this checklist with someone?", "Yes",
                    "No");
                if (answer)
                {
                    await viewModel.ShareTripAsync();
                    await DisplayAlert("Success", $"Sharing code:\n{viewModel.Trip.Id}", "OK");
                }
            }
        }

        private async void Menu_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Edit Trip", DialogActions.Cancel,
                DialogActions.Delete, DialogActions.Rename, DialogActions.AddTemplateCategory,
                DialogActions.AddEmptyCategory);

            switch (action)
            {
                case DialogActions.Delete:
                    await DeleteTrip_Clicked();
                    break;
                case DialogActions.Rename:
                    await RenameItem_Clicked();
                    break;
                case DialogActions.AddTemplateCategory:
                    await AddTemplateCategory_Clicked();
                    break;
                case DialogActions.AddEmptyCategory:
                    await AddEmptyCategory_Clicked();
                    break;
            }
        }

        private async Task AddEmptyCategory_Clicked()
        {
            var categoryName = await DisplayPromptAsync("Add Empty Category", "Enter New Category Name:");

            if (!string.IsNullOrEmpty(categoryName)) viewModel.AddEmptyCategoryAsync(categoryName);
        }

        private async Task DeleteTrip_Clicked()
        {
            await Navigation.PopAsync().ContinueWith(_ => MessagingCenter.Send(this, "DeleteTrip", viewModel.Trip));
        }

        private async Task RenameItem_Clicked()
        {
            var tripName = await DisplayPromptAsync("Rename Trip", "Enter New Trip Name:");

            if (!string.IsNullOrEmpty(tripName))
            {
                Title = tripName;
                viewModel.TripName = tripName;
                viewModel.Trip.Destination = tripName;
                MessagingCenter.Send(this, "RenameTrip", viewModel.Trip);
            }
        }

        private async Task AddTemplateCategory_Clicked()
        {
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, viewModel.Trip));
            await Navigation.PushModalAsync(new NavigationPage(setupPage));
        }
    }
}