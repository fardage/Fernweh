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
                if (!IsBusy)
                {
                    IsBusy = true;
                    _ = GetSuggestionsAsync();
                    IsBusy = false;
                }
            }
        }

        public ObservableCollection<Suggestion> SearchSuggestions { get; set; } =
            new ObservableCollection<Suggestion>();

        private async Task GetSuggestionsAsync()
        {
            SearchSuggestions.Clear();
            SearchSuggestions.Add(new Suggestion {Label = SearchText});

            var suggestions = await HereMapsProvider.GetAutocomplete(_searchText);

            foreach (var suggestion in suggestions)
                switch (suggestion.MatchLevel)
                {
                    case SuggestionKind.City:
                        suggestion.Icon = "\uf64f";
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.Country:
                        suggestion.Icon = "\uf024";
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.District:
                        suggestion.Icon = "\uf1ad";
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.Intersection:
                        suggestion.Icon = "\uf1ad";
                        SearchSuggestions.Add(suggestion);
                        break;
                    case SuggestionKind.State:
                        suggestion.Icon = "\uf041";
                        SearchSuggestions.Add(suggestion);
                        break;
                }
        }
    }
}