﻿using System.Collections.ObjectModel;
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
        public SetupTripViewModel(INavigation navigation, Trip trip)
        {
            Navigation = navigation;
            NewTrip = trip;

            SwipedCommand = new Command<SwipedCardEventArgs>(eventArgs => ExecuteSwipedCommand(eventArgs));

            TemplateCategories = TemplateProvider.GetChecklist();
            SelectedCategories = new Collection<ItemCategory>();
        }

        public INavigation Navigation { get; set; }
        public Trip NewTrip { get; set; }
        public Collection<ItemCategory> TemplateCategories { get; set; }
        public Collection<ItemCategory> SelectedCategories { get; set; }
        public SwipeCardDirection SupportedSwipeDirections => SwipeCardDirection.Right | SwipeCardDirection.Left;
        public ICommand SwipedCommand { get; }

        private void ExecuteSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            var selected = eventArgs.Item as ItemCategory;

            if (eventArgs.Direction == SwipeCardDirection.Right) SelectedCategories.Add(eventArgs.Item as ItemCategory);

            if (selected == TemplateCategories.Last())
            {
                NewTrip.Categories = SelectedCategories;
                MessagingCenter.Send(this, "AddTrip", NewTrip);
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
            }
        }
    }
}