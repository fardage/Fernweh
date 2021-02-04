namespace Fernweh.Services
{
    public class Suggestion
    {
        public string Icon { get; set; }
        public string Label { get; set; }
        public string CountryCode { get; set; }
        public SuggestionKind MatchLevel { get; set; }
    }

    public enum SuggestionKind
    {
        Intersection,
        Street,
        PostalCode,
        District,
        City,
        County,
        State,
        Country
    }
}