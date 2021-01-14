using System;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        public NewTripViewModel()
        {
            NewTrip = new Trip
            {
                Destination = "Destination",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
        }

        public Trip NewTrip { get; set; }
    }
}