using System.Collections.ObjectModel;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class GroupedList : ObservableCollection<Item>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string GroupName { get; set; }
        public new ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    }
}