using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fernweh.Data.RestService;
using Xamarin.Forms;

namespace Fernweh.ViewModels
{
    public class AddSharedTripViewModel : BaseViewModel
    {
        private readonly RestService restService = new RestService();

        private readonly string _guid_pattern =
            @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";

        private bool _isValidCode;
        private readonly INavigation _navigation;
        private string _tripCode = "52129b5e-7a79-4e81-873c-0e3017afee4f";

        public AddSharedTripViewModel(INavigation navigation)
        {
            _navigation = navigation;
            AddTripCommand = new Command(async () => await ExecuteAddTripCommand());
        }

        public bool IsValidCode
        {
            get => _isValidCode;
            set => SetProperty(ref _isValidCode, value);
        }

        public string TripCode
        {
            get => _tripCode;
            set
            {
                _tripCode = value;
                IsValidCode = Regex.IsMatch(value, _guid_pattern);
            }
        }

        public Command AddTripCommand { get; set; }

        public async Task ExecuteAddTripCommand()
        {
            if (IsValidCode)
            {
                var trip = await restService.GetTripAsync(TripCode);

                if (trip != null)
                {
                    trip.IsShared = true;
                    MessagingCenter.Send(this, "AddSharedTrip", trip);
                    await _navigation.PopModalAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Not Found",
                        "No trip has been fond for the entered code.", "Ok");
                }
            }
        }
    }
}