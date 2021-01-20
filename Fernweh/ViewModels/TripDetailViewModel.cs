using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        public TripDetailViewModel(Trip trip = null)
        {
            Title = trip?.Destination;
            Trip = trip;
            ChecklistGroups = new ObservableCollection<GroupedList>();
            _ = LoadChecklists();
        }

        public Trip Trip { get; set; }
        public ObservableCollection<GroupedList> ChecklistGroups { get; set; }

        private async Task LoadChecklists()
        {
            var checklists = await DataStore.GetItemCategoriesAsync(Trip.Id);
            foreach (var category in checklists)
            {
                var listGroup = new GroupedList {GroupName = category.Name};
                listGroup.AddRange(category.Items);
                ChecklistGroups.Add(listGroup);
            }
        }
    }
}