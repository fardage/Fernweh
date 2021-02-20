using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Models;
using Fernweh.ViewModels;
using Fernweh.Views;
using Xamarin.Forms;

namespace Fernweh.Data
{
    public class TripsHolder
    {
        public TripsHolder()
        {
            Trips = new ObservableCollection<Trip>();
        }

        public ObservableCollection<Trip> Trips { get; set; }

        public async Task LoadTrips()
        {
            Trips.Clear();
            var trips = await DataStore.GetTripsAsync();
            foreach (var trip in trips) Trips.Add(trip);
        }

        public async Task AddTrip(Trip trip)
        {
            if (!Trips.ToList().Any(t => t.Id.Equals(trip.Id)))
            {
                Trips.Add(trip);
                await DataStore.AddTripAsync(trip);
            }
            else
            {
                await DataStore.UpdateTripChecklistsAsync(trip);
            }
        }

        public async Task RemoveTrip(Trip trip)
        {
            Trips.Remove(trip);
            await DataStore.DeleteTripAsync(trip.Id);
        }

        public async Task UpdateTrip(Trip trip)
        {
            await DataStore.UpdateTripAsync(trip);
            await LoadTrips();
        }
    }
}
