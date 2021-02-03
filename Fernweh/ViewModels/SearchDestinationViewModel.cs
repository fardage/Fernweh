using System.Collections.ObjectModel;

namespace Fernweh.ViewModels
{
    public class SearchDestinationViewModel : BaseViewModel
    {
        private string _searchText;

        public SearchDestinationViewModel(string searchText)
        {
            SearchText = searchText;
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public ObservableCollection<string> SearchSuggestions { get; set; } = new ObservableCollection<string>();
    }
}