using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotMorten.Xamarin.Forms;
using Fernweh.Models;
using Fernweh.Services;
using Xamarin.Essentials;

namespace Fernweh.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        private readonly List<string> _colors = new List<string>(new List<string>
            {"#45EC9C", "#7E57FF", "#FE5D7A", "#FFB422"});

        private string _destination = "Destination";
        private DateTime _endDate = DateTime.Now;
        private DateTime _startDate = DateTime.Now;

        public NewTripViewModel()
        {
            NewTrip = new Trip
            {
                Destination = "Destination",
                ColorA = GetRandomColor(),
                ColorB = GetRandomColor(),
                StartDate = _startDate,
                EndDate = _endDate
            };
        }

        public Trip NewTrip { get; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                NewTrip.StartDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                NewTrip.EndDate = value;
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                SetProperty(ref _destination, value);
                NewTrip.Destination = value;
            }
        }

        public List<Country> CountriesList { get; set; } = new List<Country>();


        private string GetRandomColor()
        {
            var random = new Random();
            var index = random.Next(_colors.Count);
            return _colors[index];
        }

        public async Task UpdateCountrySuggestionsAsync()
        {
            CountriesList = await CountryInfoProvider.GetCountriesAsync();
        }


        public async void ExecuteTextChangedCommand(AutoSuggestBox autoSuggestBox,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput || CountriesList.Count == 0) return;

            var suggestions = new List<string>();
            Placemark placemark;

            try
            {
                var locations = await Geocoding.GetLocationsAsync(autoSuggestBox.Text);

                if (locations != null)
                {
                    var location = locations.FirstOrDefault();
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    placemark = placemarks.FirstOrDefault();

                    if (placemark != null) suggestions.Add($"{placemark.Locality}, {placemark.CountryName}");
                }
            }
            catch (Exception ex)
            {
                // Feature not supported on device
            }

            autoSuggestBox.ItemsSource = suggestions;
            Destination = autoSuggestBox.Text;
        }

        public void ExecuteSuggestionChosenCommand(AutoSuggestBox autoSuggestBox,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Destination = autoSuggestBox.Text;
        }
    }
}