using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fernweh.Models;
using Fernweh.Services;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class SetupTripViewModel : BaseViewModel
    {
        private uint _threshold;

        public SetupTripViewModel(INavigation navigation, Trip trip)
        {
            Navigation = navigation;
            NewTrip = trip;

            SwipedCommand = new Command<SwipedCardEventArgs>(eventArgs => ExecuteSwipedCommand(eventArgs));

            Threshold = (uint) (App.ScreenWidth / 3);

            TemplateCategories = TemplateProvider.GetChecklist();
            SelectedCategories = new Collection<ItemCategory>();
        }

        public INavigation Navigation { get; set; }
        public Trip NewTrip { get; set; }
        public Collection<ItemCategory> TemplateCategories { get; set; }
        public Collection<ItemCategory> SelectedCategories { get; set; }
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

            if (eventArgs.Direction == SwipeCardDirection.Right) SelectedCategories.Add(eventArgs.Item as ItemCategory);

            if (selected == TemplateCategories.Last()) WrapUpTrip();
        }

        private void WrapUpTrip()
        {
            NewTrip.Categories = SelectedCategories;
            MessagingCenter.Send(this, "AddTrip", NewTrip);

            Navigation.PopToRootAsync();
            Navigation.PopModalAsync(false);
        }
    }
}