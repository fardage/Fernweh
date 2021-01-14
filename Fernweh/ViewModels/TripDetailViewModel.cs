using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        public TripDetailViewModel(Trip trip = null)
        {
            Title = trip?.Destination;
            Trip = trip;
            ChecklistGroups = new ObservableCollection<GroupedList>();
            LoadChecklistsAsync().Wait();
        }
        
        public Trip Trip { get; set; }
        public ObservableCollection<GroupedList> ChecklistGroups { get; set; }

        private async Task LoadChecklistsAsync()
        {
            IsBusy = true;

            try
            {
                ChecklistGroups.Clear();
                var checklists = await DataStore.GetItemCategoriesAsync(Trip.Id);
                foreach (var category in checklists)
                {
                    var listGroup = new GroupedList {GroupName = category.Name};
                    listGroup.AddRange(category.Items);
                    ChecklistGroups.Add(listGroup);
                }
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