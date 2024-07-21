namespace TVShowChecker.Core.Models
{
    public sealed class TVShowContext(string tvShowName, Episode nextEp, Episode prevEp)
    {
        public string TvShowName { get; } = tvShowName;
        public Episode NextEp { get; } = nextEp;
        public Episode PrevEp { get; } = prevEp;
    }
}
