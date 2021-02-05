namespace Fernweh.Services.HereMaps
{
    public class Suggestion
    {
        public string Icon { get; set; }
        public string Label { get; set; }
        public string CountryCode { get; set; }
        public SuggestionKind MatchLevel { get; set; }
    }
}