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
                .Include(t => t.Categories)
                .ThenInclude(c => c.Items)
                .ToList();
            return await Task.FromResult(storedTrips);
        }

        public static async Task UpdateTripAsync(Trip trip)
        {
            var travelContext = new TravelContext();
            travelContext.Update(trip);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByTripId(trip.Id);
        }

        public static async Task UpdateTripChecklistsAsync(Trip trip)
        {
            var travelContext = new TravelContext();
            var targetTrip = await travelContext.Trips.FindAsync(trip.Id);
            var targetCategories = await GetItemCategoriesAsync(trip.Id);

            foreach (var category in trip.Categories)
                if (!targetCategories.Any(x => x.Name.Equals(category.Name)))
                    targetTrip.Categories.Add(category);
            travelContext.Update(targetTrip);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByTripId(trip.Id);
        }

        public static async Task DeleteChecklistAsync(string id)
        {
            var travelContext = new TravelContext();
            var checklist = travelContext.Checklists
                .Where(c => c.Id.Equals(id))
                .Include(i => i.Items)
                .SingleOrDefault();
            travelContext.Checklists.Remove(checklist);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByTripId(checklist.ParentId);
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
                .Where(x => x.Id.Equals(id))
                .Include(y => y.Categories)
                .ThenInclude(z => z.Items)
                .Select(c => c.Categories).SingleOrDefault();
            return await Task.FromResult(storedChecklists);
        }

        public static async Task AddItemAsync(string categoryId, Item item)
        {
            var travelContext = new TravelContext();
            var category = await travelContext.Checklists.FindAsync(categoryId);
            category.Items.Add(item);
            travelContext.Update(category);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByCategoryId(category.Id);
        }

        public static async Task UpdateItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.Update(item);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByItemId(item.Id);
        }

        public static async Task DeleteItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.ChecklistItems.Remove(item);
            await travelContext.SaveChangesAsync();

            await UpdateRemoteByCategoryId(item.ParentId);
        }

        private static async Task UpdateRemoteByTripId(string tripId)
        {
            using (var travelContext = new TravelContext())
            {
                var trip = travelContext.Trips
                    .Where(t => t.Id == tripId)
                    .Include(t => t.Categories)
                    .ThenInclude(c => c.Items)
                    .SingleOrDefault();

                if (trip.IsShared)
                {
                    var restService = new RestService.RestService();
                    await restService.SaveTripAsync(trip);
                }
            }
        }

        private static async Task UpdateRemoteByCategoryId(string categoryId)
        {
            using (var travelContext = new TravelContext())
            {
                var category = travelContext.Checklists
                    .Where(c => c.Id == categoryId)
                    .Include(c => c.Parent)
                    .SingleOrDefault();

                await UpdateRemoteByTripId(category.ParentId);
            }
        }

        private static async Task UpdateRemoteByItemId(string itemId)
        {
            using (var travelContext = new TravelContext())
            {
                var item = travelContext.ChecklistItems
                    .Where(i => i.Id == itemId)
                    .Include(c => c.Parent)
                    .SingleOrDefault();

                await UpdateRemoteByCategoryId(item.ParentId);
            }
        }
    }
}