using System;
using System.Collections.Generic;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Services.HereMaps;
using Fernweh.Views;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        private readonly List<string> _colors = new List<string>(new List<string>
            {"#45EC9C", "#7E57FF", "#FE5D7A", "#FFB422"});

        private string _colorA;
        private string _colorB;

        private string _destination = "Destination";
        private DateTime _endDate = DateTime.Now;
        private DateTime _startDate = DateTime.Now;    

        public NewTripViewModel(TripsHolder tripsHolder)
        {
            TripsHolder = tripsHolder;

            NewTrip = new Trip
            {
                Destination = "Destination",
                StartDate = _startDate,
                EndDate = _endDate
            };

            ColorA = GetRandomColor();
            ColorB = GetRandomColor();
            SetRandomColorCommand = new Command(ExecuteSetRandomColorCommand);

            SubscribeToMessagingCenter();
        }

        public Trip NewTrip { get; }

        public TripsHolder TripsHolder { get; set; }

        public Command SetRandomColorCommand { get; set; }

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

        public string ColorA
        {
            get => _colorA;
            set
            {
                NewTrip.ColorA = value;
                SetProperty(ref _colorA, value);
            }
        }

        public string ColorB
        {
            get => _colorB;
            set
            {
                NewTrip.ColorB = value;
                SetProperty(ref _colorB, value);
            }
        }

        private void SubscribeToMessagingCenter()
        {
            MessagingCenter.Subscribe<SearchDestinationPage, Suggestion>(this, "ItemSelected",
                (obj, selected) =>
                {
                    if (!string.IsNullOrEmpty(selected.Address.City))
                        Destination = selected.Address.City;
                    else if (!string.IsNullOrEmpty(selected.Address.State))
                        Destination = selected.Address.State;
                    else if (!string.IsNullOrEmpty(selected.Address.CountryName))
                        Destination = selected.Address.CountryName;
                    else
                        Destination = selected.Label;
                });
        }

        internal void ExecuteSetRandomColorCommand()
        {
            ColorA = GetRandomColor();
            ColorB = GetRandomColor();
        }

        private string GetRandomColor()
        {
            var random = new Random();
            var index = random.Next(_colors.Count);
            return _colors[index];
        }
    }
}