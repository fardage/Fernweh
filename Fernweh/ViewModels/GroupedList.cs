using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class GroupedList : ObservableCollection<Item>
    {
        public string Icon { get; set; }
        public string GroupName { get; set; }
        public new ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    }
}