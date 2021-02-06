﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public TripDetailViewModel(Trip trip = null)
        {
            Title = trip?.Destination;
            Trip = trip;
            ChecklistGroups = new ObservableCollection<GroupedList>();

            LoadChecklistsCommand = new Command(async () => await ExecuteLoadChecklistsCommand());
            DeleteChecklistItemCommand = new Command<Models.Item>(async (item) => await ExecuteDeleteChecklistItemCommand(item));
            AddItemCommand = new Command<GroupedList>(async (groupedList) => await ExecuteAddItemCommand(groupedList));

            _ = ExecuteLoadChecklistsCommand();
            _ = ExecuteLoadInfoCommand();
        }

        public Command LoadChecklistsCommand { get; set; }
        public Command<Models.Item> DeleteChecklistItemCommand { get; set; }
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

        private async Task ExecuteLoadChecklistsCommand()
        {
            IsBusy = true;
            ChecklistGroups.Clear();
            try
            {
                var checklists = await DataStore.GetItemCategoriesAsync(Trip.Id);

                foreach (var category in checklists)
                {
                    if (category.Items.Count != 0)
                    {
                        var listGroup = new GroupedList
                        {
                            Id = category.Id,
                            GroupName = category.Name,
                            Icon = category.Icon 
                            
                        };
                        foreach (var item in category.Items)
                        {
                            listGroup.Add(item);
                        }
                        ChecklistGroups.Add(listGroup);
                    }
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

        private async Task ExecuteDeleteChecklistItemCommand(Models.Item item)
        {
            await DataStore.DeleteItemAsync(item);
            for (int i = ChecklistGroups.Count - 1; i >= 0; i--)
            {
                ChecklistGroups[i].Remove(item);
                if (ChecklistGroups[i].Count == 0)
                {
                    ChecklistGroups.Remove(ChecklistGroups[i]);
                }
            }
        }

        public async Task ExecuteAddItemCommand(GroupedList groupedList)
        {
            var itemName = await Application.Current
                .MainPage.DisplayPromptAsync("Add Item", "Enter name of item:");
            var newItem = new Item() {Name = itemName};
            groupedList.Add(newItem);
            await DataStore.AddItemAsync(groupedList.Id, newItem);
        }
    }
}