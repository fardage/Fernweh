using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Models;
using Microsoft.EntityFrameworkCore;

namespace Fernweh.Data
{
    public static class DataStore
    {
        public static async Task AddTripAsync(Trip trip)
        {
            await using var tripContext = new TravelContext();
            tripContext.Add(trip);
            await tripContext.SaveChangesAsync();
        }

        public static async Task<IEnumerable<Trip>> GetTripsAsync()
        {
            var travelContext = new TravelContext();
            var storedTrips = travelContext.Trips
                .AsNoTracking()
                .ToList();
            return await Task.FromResult(storedTrips);
        }

        public static async Task DeleteTripAsync(string id)
        {
            var travelContext = new TravelContext();
            var toDelete = travelContext.Trips
                .Where(x => x.Id.Equals(id))
                .Include(y => y.Categories)
                .ThenInclude(z => z.Items)
                .SingleOrDefault();
            travelContext.Trips.RemoveRange(toDelete);
            await travelContext.SaveChangesAsync();
        }

        public static async Task<IEnumerable<ItemCategory>> GetItemCategoriesAsync(string id)
        {
            var travelContext = new TravelContext();
            var storedChecklists = travelContext.Trips
                .AsNoTracking()
                .Where(x => x.Id.Equals(id))
                .Include(y => y.Categories)
                .ThenInclude(z => z.Items)
                .Select(c => c.Categories).SingleOrDefault();
            return await Task.FromResult(storedChecklists);
        }

        public static async Task UpdateItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.Update(item);
            await travelContext.SaveChangesAsync();
        }

        public static async Task DeleteItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.ChecklistItems.Remove(item);
            await travelContext.SaveChangesAsync();
        }
    }
}