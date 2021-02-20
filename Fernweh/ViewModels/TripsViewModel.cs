using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Views;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        public TripsViewModel()
        {
            Title = "Trips";
            LoadTripsCommand = new Command(async () => await ExecuteLoadTripsCommand());
            DeleteTripCommand = new Command<Trip>(async trip => await ExecuteDeleteTripCommand(trip));

            TripsHolder = new TripsHolder();
            _ = TripsHolder.LoadTrips();
        }

        public Command LoadTripsCommand { get; set; }

        public Command<Trip> DeleteTripCommand { get; set; }

        public TripsHolder TripsHolder { get; set; }

        public async Task ExecuteLoadTripsCommand()
        {
            IsBusy = true;
            await TripsHolder.LoadTrips();
            IsBusy = false;
        }

        private async Task ExecuteDeleteTripCommand(Trip trip)
        {
            await TripsHolder.RemoveTrip(trip);
        }
    }
}