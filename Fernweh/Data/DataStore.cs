using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Models;
using Fernweh.Services;
using Microsoft.EntityFrameworkCore;

namespace Fernweh.Data
{
    public static class DataStore
    {
        private static readonly SerialQueue _serialQueue = new SerialQueue();

        public static async Task AddTripAsync(Trip trip)
        {
            await using var tripContext = new TravelContext();
            tripContext.Add(trip);
            await tripContext.SaveChangesAsync();
        }

        public static async Task<IEnumerable<Trip>> GetTripsAsync()
        {
            var travelContext = new TravelContext();
            var trips = travelContext.Trips
                .Include(t => t.Categories)
                .ThenInclude(c => c.Items)
                .ToList();
            return await Task.FromResult(trips);
        }

        public static async Task UpdateTripAsync(Trip trip)
        {
            var travelContext = new TravelContext();
            travelContext.Update(trip);
            await travelContext.SaveChangesAsync();

            UpdateRemoteByTripId(trip.Id);
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

            UpdateRemoteByTripId(trip.Id);
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

            UpdateRemoteByTripId(checklist.ParentId);
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

            UpdateRemoteByCategoryId(category.Id);
        }

        public static async Task UpdateItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.Update(item);
            await travelContext.SaveChangesAsync();

            UpdateRemoteByItemId(item.Id);
        }

        public static async Task DeleteItemAsync(Item item)
        {
            var travelContext = new TravelContext();
            travelContext.ChecklistItems.Remove(item);
            await travelContext.SaveChangesAsync();

            UpdateRemoteByCategoryId(item.ParentId);
        }

        private static void UpdateRemoteByTripId(string tripId)
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
                    _ = _serialQueue.Enqueue(async () => { await restService.SaveTripAsync(trip); });
                }
            }
        }

        private static void UpdateRemoteByCategoryId(string categoryId)
        {
            using (var travelContext = new TravelContext())
            {
                var category = travelContext.Checklists
                    .Where(c => c.Id == categoryId)
                    .Include(c => c.Parent)
                    .SingleOrDefault();

                UpdateRemoteByTripId(category.ParentId);
            }
        }

        private static void UpdateRemoteByItemId(string itemId)
        {
            using (var travelContext = new TravelContext())
            {
                var item = travelContext.ChecklistItems
                    .Where(i => i.Id == itemId)
                    .Include(c => c.Parent)
                    .SingleOrDefault();

                UpdateRemoteByCategoryId(item.ParentId);
            }
        }
    }
}