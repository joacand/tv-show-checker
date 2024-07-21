namespace TVShowChecker.Core.Models
{
    public sealed class Episode(string name, string airDate, string episodeNumber = null)
    {
        public string Name { get; } = name;
        public string AirDate { get; } = airDate;
        public string EpisodeNumber { get; } = episodeNumber;
    }
}
