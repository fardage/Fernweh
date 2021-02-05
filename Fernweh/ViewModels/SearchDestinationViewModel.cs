using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fernweh.Services;
using Fernweh.Services.HereMaps;

namespace Fernweh.ViewModels
{
    public class SearchDestinationViewModel : BaseViewModel
    {
        private readonly HereMapsProvider _hereMapsProvider = new HereMapsProvider();
        private readonly SerialQueue _serialQueue = new SerialQueue();
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _ = _serialQueue.Enqueue(GetSuggestionsAsync);
            }
        }

        public ObservableCollection<Suggestion> SearchSuggestions { get; set; } =
            new ObservableCollection<Suggestion>();

        private async Task GetSuggestionsAsync()
        {
            SearchSuggestions.Clear();
            SearchSuggestions.Add(new Suggestion {Label = _searchText});

            var suggestions = await _hereMapsProvider.GetAutocomplete(_searchText);

            foreach (var suggestion in suggestions)
                switch (suggestion.MatchLevel)
                {
                    case SuggestionKind.City:
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.Country:
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.State:
                        SearchSuggestions.Add(suggestion);
                        break;
                }
        }
    }
}