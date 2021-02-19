namespace Fernweh.Services.HereMaps
{
    public class Suggestion
    {
        public string Icon
        {
            get
            {
                switch (MatchLevel)
                {
                    case SuggestionKind.City:
                        return "\uf64f";
                    case SuggestionKind.Country:
                        return "\uf024";
                    case SuggestionKind.State:
                        return "\uf041";
                    default:
                        return string.Empty;
                }
            }
        }

        public string Label { get; set; }
        public string CountryCode { get; set; }
        public Address Address { get; set; } = new Address();
        public SuggestionKind MatchLevel { get; set; }
    }
}