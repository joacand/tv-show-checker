namespace TVShowChecker.Entities
{
    class TVShowContext
    {
        public string TvShowName { get; }
        public Episode NextEp { get; }
        public Episode PrevEp { get; }

        public TVShowContext(string tvShowName, Episode nextEp, Episode prevEp)
        {
            TvShowName = tvShowName;
            NextEp = nextEp;
            PrevEp = prevEp;
        }
    }
}
