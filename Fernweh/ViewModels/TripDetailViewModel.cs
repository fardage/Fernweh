using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Fernweh.Data;
using Fernweh.Models;
using Fernweh.Services;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        private double _averageTemperature;
        private string _nativeName;
        private int _population;

        public TripDetailViewModel(Trip trip = null)
        {
            Title = trip?.Destination;
            Trip = trip;
            ChecklistGroups = new ObservableCollection<GroupedList>();
            LoadChecklistsCommand = new Command(async () => await ExecuteLoadChecklistsCommand());

            // TODO: Clean this up
            _ = ExecuteLoadChecklistsCommand();
            _ = ExecuteLoadInfoCommand();
        }

        public Command LoadChecklistsCommand { get; set; }
        public Trip Trip { get; set; }

        public string NativeName
        {
            get => _nativeName;
            set => SetProperty(ref _nativeName, value);
        }

        public int Population
        {
            get => _population;
            set => SetProperty(ref _population, value);
        }

        public double AverageTemperature
        {
            get => _averageTemperature;
            set => SetProperty(ref _averageTemperature, value);
        }

        public ObservableCollection<GroupedList> ChecklistGroups { get; set; }

        public async Task ExecuteLoadChecklistsCommand()
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

        public async Task ExecuteLoadInfoCommand()
        {
            try
            {
                var country = await CountryInfoProvider.GetCountryAsync(Trip.Destination);
                Population = country.Population;
                NativeName = country.NativeName;

                var weather = await CountryInfoProvider.GetAverageWeatherAsync(country.Alpha3Code);
                var startTemp = weather.MonthVals[Trip.StartDate.Month - 1];
                var endTemp = weather.MonthVals[Trip.EndDate.Month - 1];
                AverageTemperature = Math.Round((startTemp + endTemp) / 2, 2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}