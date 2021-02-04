using System;
using System.Collections.Generic;
using Fernweh.Models;
using Fernweh.Services;
using Fernweh.Views;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        private readonly List<string> _colors = new List<string>(new List<string>
            {"#45EC9C", "#7E57FF", "#FE5D7A", "#FFB422"});

        private string _destination = "Destination";
        private DateTime _endDate = DateTime.Now;
        private DateTime _startDate = DateTime.Now;

        public NewTripViewModel()
        {
            NewTrip = new Trip
            {
                Destination = "Destination",
                ColorA = GetRandomColor(),
                ColorB = GetRandomColor(),
                StartDate = _startDate,
                EndDate = _endDate
            };

            SubscribeToMessagingCenter();
        }

        public Trip NewTrip { get; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                NewTrip.StartDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                NewTrip.EndDate = value;
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                SetProperty(ref _destination, value);
                NewTrip.Destination = value;
            }
        }

        private void SubscribeToMessagingCenter()
        {
            MessagingCenter.Subscribe<SearchDestinationPage, Suggestion>(this, "ItemSelected",
                (obj, selected) => { Destination = selected.Label; });
        }

        private string GetRandomColor()
        {
            var random = new Random();
            var index = random.Next(_colors.Count);
            return _colors[index];
        }
    }
}