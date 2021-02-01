using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Views;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        public TripsViewModel()
        {
            Title = "Trips";
            Trips = new ObservableCollection<Trip>();
            LoadTripsCommand = new Command(async () => await ExecuteLoadTripsCommand());
            SubscribeToMessagingCenter();
            _ = ExecuteLoadTripsCommand();
        }

        public ObservableCollection<Trip> Trips { get; set; }
        public Command LoadTripsCommand { get; set; }

        private void SubscribeToMessagingCenter()
        {
            MessagingCenter.Subscribe<SetupTripViewModel, Trip>(this, "AddTrip", async (obj, trip) =>
            {
                Trips.Add(trip);
                await DataStore.AddTripAsync(trip);
            });

            MessagingCenter.Subscribe<TripDetailPage, Trip>(this, "DeleteTrip",
                async (obj, trip) =>
                {
                    Trips.Remove(trip);
                    await DataStore.DeleteTripAsync(trip.Id);
                });
        }

        public async Task ExecuteLoadTripsCommand()
        {
            IsBusy = true;

            try
            {
                Trips.Clear();
                var trips = await DataStore.GetTripsAsync();
                foreach (var trip in trips) Trips.Add(trip);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}