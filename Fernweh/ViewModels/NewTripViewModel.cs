using System;
using System.Collections.Generic;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        private readonly List<string> colors = new List<string>(new List<string>
            {"#45EC9C", "#7E57FF", "#fE5D7A", "#FFB422"});

        private string _destination = "Destination";
        private DateTime _endDate;
        private DateTime _startDate;

        public NewTripViewModel()
        {
            NewTrip = new Trip
            {
                Destination = "Destination",
                ColorA = GetRandomColor(),
                ColorB = GetRandomColor()
            };

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
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

        private string GetRandomColor()
        {
            var random = new Random();
            var index = random.Next(colors.Count);
            return colors[index];
        }
    }
}