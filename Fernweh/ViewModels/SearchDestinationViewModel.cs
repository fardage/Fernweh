using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fernweh.Services;

namespace Fernweh.ViewModels
{
    public class SearchDestinationViewModel : BaseViewModel
    {
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _ = GetSuggestionsAsync();
            }
        }

        public ObservableCollection<string> SearchSuggestions { get; set; } = new ObservableCollection<string>();

        private async Task GetSuggestionsAsync()
        {
            var suggestions = await HereMapsProvider.GetAutocomplete(_searchText);
            SearchSuggestions.Clear();
            SearchSuggestions.Add(SearchText);
            foreach (var suggestion in suggestions) SearchSuggestions.Add(suggestion);
        }
    }
}