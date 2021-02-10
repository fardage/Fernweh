using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            DeleteTripCommand = new Command<Trip>(async trip => await ExecuteDeleteTripCommand(trip));

            SubscribeToMessagingCenter();
            _ = ExecuteLoadTripsCommand();
        }

        public ObservableCollection<Trip> Trips { get; set; }
        public Command LoadTripsCommand { get; set; }
        public Command<Trip> DeleteTripCommand { get; set; }

        private void SubscribeToMessagingCenter()
        {
            MessagingCenter.Subscribe<SetupTripViewModel, Trip>(this, "SetupTrip", async (obj, trip) =>
            {
                if (!Trips.ToList().Any(t => t.Id.Equals(trip.Id)))
                {
                    Analytics.TrackEvent($"Trip added: {trip.Destination}");
                    Trips.Add(trip);
                    await DataStore.AddTripAsync(trip);
                }
                else
                {
                    await DataStore.UpdateTripChecklistsAsync(trip);
                }
            });

            MessagingCenter.Subscribe<TripDetailPage, Trip>(this, "DeleteTrip",
                async (obj, trip) =>
                {
                    Trips.Remove(trip);
                    await DataStore.DeleteTripAsync(trip.Id);
                });

            MessagingCenter.Subscribe<TripDetailPage, Trip>(this, "RenameTrip",
                async (obj, trip) =>
                {
                    await DataStore.UpdateTripAsync(trip);
                    await ExecuteLoadTripsCommand();
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
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteDeleteTripCommand(Trip trip)
        {
            Trips.Remove(trip);
            await DataStore.DeleteTripAsync(trip.Id);
        }
    }
}