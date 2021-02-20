using System;
using System.Threading.Tasks;
using Fernweh.Models;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class TripDetailPage
    {
        private readonly TripDetailViewModel _viewModel;

        public TripDetailPage(TripDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        private async void Menu_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Edit Trip", DialogActions.Cancel,
                DialogActions.Delete, DialogActions.Rename, DialogActions.AddTemplateCategory,
                DialogActions.AddEmptyCategory);

            switch (action)
            {
                case DialogActions.Delete:
                    await DeleteItem_Clicked();
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

            if (!string.IsNullOrEmpty(categoryName)) _viewModel.AddEmptyCategoryAsync(categoryName);
        }

        private async Task DeleteItem_Clicked()
        {
            await Navigation.PopAsync().ContinueWith(_ => _viewModel.TripsHolder.RemoveTrip(_viewModel.Trip));
        }

        private async Task RenameItem_Clicked()
        {
            var tripName = await DisplayPromptAsync("Rename Trip", "Enter New Trip Name:");

            if (!string.IsNullOrEmpty(tripName))
            {
                Title = tripName;
                _viewModel.TripName = tripName;
                _viewModel.Trip.Destination = tripName;
                await _viewModel.TripsHolder.UpdateTrip(_viewModel.Trip);
            }
        }

        private async Task AddTemplateCategory_Clicked()
        {
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, _viewModel.Trip, _viewModel.TripsHolder));
            await Navigation.PushModalAsync(new NavigationPage(setupPage));
        }

        private void Label_Clicked(object sender, EventArgs e)
        {
            if ((sender as Label)?.BindingContext is Item item) item.Packed = !item.Packed;
        }
    }

    internal static class DialogActions
    {
        public const string Cancel = "Cancel";
        public const string Delete = "Delete";
        public const string Rename = "Rename";
        public const string AddTemplateCategory = "Add Category from Template";
        public const string AddEmptyCategory = "Add Category Empty";
    }
}