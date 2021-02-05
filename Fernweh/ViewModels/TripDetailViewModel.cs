using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Services.HereMaps;
using Fernweh.Services.RestCountries;
using Fernweh.Services.WorldBank;
using Xamarin.Forms;

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

            _ = ExecuteLoadChecklistsCommand();
            _ = ExecuteLoadInfoCommand();
        }

        public Command LoadChecklistsCommand { get; set; }
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

            try
            {
                var checklists = await DataStore.GetItemCategoriesAsync(Trip.Id);

                foreach (var category in checklists)
                {
                    var listGroup = new GroupedList {GroupName = category.Name, Icon = category.Icon};
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

        private async Task ExecuteLoadInfoCommand()
        {
            var address = await _hereMapsProvider.GetGeocode(Trip.Destination);
            if (!string.IsNullOrEmpty(address.CountryCode))
                _ = Task.WhenAll(LoadWeather(address.CountryCode), LoadFacts(address.CountryCode));
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
    }
}