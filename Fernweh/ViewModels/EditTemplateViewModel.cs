using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Models;
using Fernweh.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class EditTemplateViewModel : BaseViewModel
    {
        public EditTemplateViewModel()
        {
            AddItemCommand = new Command<GroupedList>(async groupedList => await ExecuteAddItemCommand(groupedList));
            DeleteItemCommand = new Command<Item>(ExecuteDeleteItemCommand);
            AddCategoryCommand = new Command(async () => await ExecuteAddCategoryCommand());
            ExecuteLoadChecklistsCommand();
        }

        public ObservableCollection<GroupedList> ChecklistGroups { get; set; } =
            new ObservableCollection<GroupedList>();

        public Command<GroupedList> AddItemCommand { get; set; }
        public Command<Item> DeleteItemCommand { get; set; }

        public Command AddCategoryCommand { get; set; }

        private async Task ExecuteAddCategoryCommand()
        {
            var categoryName = await Application.Current
                .MainPage.DisplayPromptAsync("Add Category", "Enter name of category:");

            if (!string.IsNullOrEmpty(categoryName))
            {
                ChecklistGroups.Insert(0, new GroupedList
                {
                    GroupName = categoryName,
                    Icon = "\uf4ff",
                    Items = new ObservableCollection<Item>()
                });
                SaveTemplate();
            }
        }

        private void ExecuteLoadChecklistsCommand()
        {
            IsBusy = true;
            ChecklistGroups.Clear();
            try
            {
                var checklists = TemplateProvider.GetChecklist();

                foreach (var category in checklists)
                {
                    var listGroup = new GroupedList
                    {
                        GroupName = category.Name,
                        Icon = category.Icon
                    };
                    foreach (var item in category.Items) listGroup.Add(item);
                    ChecklistGroups.Add(listGroup);
                }
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

        public void ExecuteDeleteItemCommand(Item item)
        {
            for (var i = ChecklistGroups.Count - 1; i >= 0; i--)
            {
                ChecklistGroups[i].Remove(item);
                if (ChecklistGroups[i].Count == 0) ChecklistGroups.Remove(ChecklistGroups[i]);
            }

            SaveTemplate();
        }

        public async Task ExecuteAddItemCommand(GroupedList groupedList)
        {
            var itemName = await Application.Current
                .MainPage.DisplayPromptAsync("Add Item", "Enter name of item:");
            if (!string.IsNullOrEmpty(itemName))
            {
                var newItem = new Item {Name = itemName};
                groupedList.Insert(0, newItem);
                SaveTemplate();
            }
        }

        private void SaveTemplate()
        {
            var newTemplate = new List<ItemCategory>();
            foreach (var group in ChecklistGroups)
                newTemplate.Add(new ItemCategory(@group.GroupName)
                {
                    Icon = @group.Icon,
                    Items = @group.ToList()
                });

            TemplateProvider.SetChecklist(newTemplate);
        }
    }
}