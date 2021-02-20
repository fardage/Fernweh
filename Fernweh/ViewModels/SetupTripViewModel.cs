using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Services;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class SetupTripViewModel : BaseViewModel
    {
        private readonly List<Item> _existingItems = new List<Item>();
        private uint _threshold;

        public SetupTripViewModel(INavigation navigation, Trip trip, TripsHolder tripsHolder)
        {
            Navigation = navigation;
            Trip = trip;
            TripsHolder = tripsHolder;

            SwipedCommand = new Command<SwipedCardEventArgs>(eventArgs => ExecuteSwipedCommand(eventArgs));

            Threshold = (uint) (App.ScreenWidth / 3);

            TemplateCategories = TemplateProvider.GetChecklist();
            TemplateCategories.RemoveAll(x => Trip.Categories.Exists(y => y.Name.Equals(x.Name)));
            SelectedCategories = new List<ItemCategory>();
        }

        public INavigation Navigation { get; set; }

        public TripsHolder TripsHolder { get; set; }

        public Trip Trip { get; set; }

        public List<ItemCategory> TemplateCategories { get; set; }

        public List<ItemCategory> SelectedCategories { get; set; }

        public SwipeCardDirection SupportedSwipeDirections => SwipeCardDirection.Right | SwipeCardDirection.Left;

        public ICommand SwipedCommand { get; }

        public uint Threshold
        {
            get => _threshold;
            set => SetProperty(ref _threshold, value);
        }

        private void ExecuteSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            var selected = eventArgs.Item as ItemCategory;

            if (eventArgs.Direction == SwipeCardDirection.Right)
            {
                selected.Items.RemoveAll(x => x.IsEnabled == false);
                AddCategory(selected);
            }

            if (selected == TemplateCategories.Last()) _ = WrapUpTripAsync();
        }

        internal async Task WrapUpTripAsync()
        {
            Trip.Categories.AddRange(SelectedCategories);
            await TripsHolder.AddTrip(Trip);

            _ = Navigation.PopModalAsync();
        }

        private void AddCategory(ItemCategory category)
        {
            category.Items.RemoveAll(x => _existingItems.Exists(y => y.Name.Equals(x.Name)));
            _existingItems.AddRange(category.Items);
            SelectedCategories.Add(category);
        }
    }
}