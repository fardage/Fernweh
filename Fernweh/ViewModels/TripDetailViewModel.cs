using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Services.HereMaps;
using Fernweh.Services.RestCountries;
using Fernweh.Services.WorldBank;
using Xamarin.Forms;
using Item = Fernweh.Models.Item;

namespace Fernweh.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        private readonly HereMapsProvider _hereMapsProvider = new HereMapsProvider();
        private readonly RestCountriesProvider _restCountriesProvider = new RestCountriesProvider();
        private readonly WorldBankProvider _worldBankProvider = new WorldBankProvider();
        private double _averageTemperature;
        private CountryFacts _facts;
        private string _tripName;

        public TripDetailViewModel(Trip trip)
        {
            Title = trip.Destination;
            TripName = trip.Destination;
            Trip = trip;
            ChecklistGroups = new ObservableCollection<GroupedList>();

            LoadChecklistsCommand = new Command(async () => await ExecuteLoadChecklistsCommand());
            DeleteChecklistItemCommand = new Command<Item>(async item => await ExecuteDeleteChecklistItemCommand(item));
            AddItemCommand = new Command<GroupedList>(async groupedList => await ExecuteAddItemCommand(groupedList));

            MessagingCenter.Subscribe<SetupTripViewModel, Trip>(this, "SetupTrip",
                async (obj, trip) => { await ExecuteLoadChecklistsCommand(); });

            _ = ExecuteLoadChecklistsCommand(true);
            _ = ExecuteLoadInfoCommand();
        }

        public Command LoadChecklistsCommand { get; set; }
        public Command<Item> DeleteChecklistItemCommand { get; set; }
        public Command<GroupedList> AddItemCommand { get; set; }
        public Trip Trip { get; set; }

        public CountryFacts Facts
        {
            get => _facts;
            set => SetProperty(ref _facts, value);
        }

        public double AverageTemperature
        {
            get => _averageTemperature;
            set => SetProperty(ref _averageTemperature, value);
        }

        public ObservableCollection<GroupedList> ChecklistGroups { get; set; }

        public string TripName
        {
            get => _tripName;
            set => SetProperty(ref _tripName, value);
        }

        private async Task ExecuteLoadChecklistsCommand(bool forceRefresh = false)
        {
            IsBusy = true;

            var categoryies = forceRefresh ? await DataStore.GetItemCategoriesAsync(Trip.Id) : Trip.Categories;

            ChecklistGroups.Clear();

            foreach (var category in categoryies)
            {
                var listGroup = new GroupedList
                {
                    Id = category.Id,
                    GroupName = category.Name,
                    Icon = category.Icon
                };
                foreach (var item in category.Items) listGroup.Add(item);
                ChecklistGroups.Add(listGroup);
            }

            IsBusy = false;
        }

        private async Task ExecuteLoadInfoCommand()
        {
            var address = await _hereMapsProvider.GetGeocode(Trip.Destination);
            if (!string.IsNullOrEmpty(address.CountryCode))
            {
                _ = LoadWeather(address.CountryCode);
                _ = LoadFacts(address.CountryCode);
            }
        }

        private async Task LoadWeather(string countryCode)
        {
            var weather = await _worldBankProvider.GetAverageWeatherAsync(countryCode);
            var startTemp = weather.MonthVals[Trip.StartDate.Month - 1];
            var endTemp = weather.MonthVals[Trip.EndDate.Month - 1];
            AverageTemperature = Math.Round((startTemp + endTemp) / 2, 1);
        }

        private async Task LoadFacts(string countryCode)
        {
            Facts = await _restCountriesProvider.GetCountryFactsAsync(countryCode);
        }

        private async Task ExecuteDeleteChecklistItemCommand(Item item)
        {
            await DataStore.DeleteItemAsync(item);
            for (var i = ChecklistGroups.Count - 1; i >= 0; i--)
            {
                ChecklistGroups[i].Remove(item);
                if (ChecklistGroups[i].Count == 0) ChecklistGroups.Remove(ChecklistGroups[i]);
            }
        }

        public async Task ExecuteAddItemCommand(GroupedList groupedList)
        {
            var itemName = await Application.Current
                .MainPage.DisplayPromptAsync("Add Item", "Enter name of item:");
            if (!string.IsNullOrEmpty(itemName))
            {
                var newItem = new Item {Name = itemName};
                groupedList.Insert(0, newItem);
                await DataStore.AddItemAsync(groupedList.Id, newItem);
            }
        }

        internal void AddEmptyCategoryAsync(string name)
        {
            var category = new ItemCategory(name) { Icon = "\uf4ff" };

            Trip.Categories.Add(category);

            var listGroup = new GroupedList
            {
                Id = category.Id,
                GroupName = category.Name,
                Icon = category.Icon
            };
            ChecklistGroups.Add(listGroup);

            _ = DataStore.UpdateTripChecklistsAsync(Trip);
        }
    }
}