namespace TVShowChecker
{
    class RawTVShow
    {
        public string Name { get; }
        public string AirDate { get; }
        public string Episode { get; }

        public RawTVShow(string name, string airDate, string episode = null)
        {
            Name = name;
            AirDate = airDate;
            Episode = episode;
        }
    }
}
