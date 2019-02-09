namespace TVShowChecker.Core.Models
{
    public sealed class Episode
    {
        public string Name { get; }
        public string AirDate { get; }
        public string EpisodeNumber { get; }

        public Episode(string name, string airDate, string episodeNumber = null)
        {
            Name = name;
            AirDate = airDate;
            EpisodeNumber = episodeNumber;
        }
    }
}
