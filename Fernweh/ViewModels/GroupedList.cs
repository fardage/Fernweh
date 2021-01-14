using System.Collections.Generic;
using Fernweh.Models;

namespace Fernweh.ViewModels
{
    public class GroupedList : List<Item>
    {
        public string GroupName { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}