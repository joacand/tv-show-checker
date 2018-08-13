namespace TVShowChecker
{
    class EpisodeInfo
    {
        public string EpisodeName { get; }
        public RawTVShow NextEp { get; }
        public RawTVShow PrevEp { get; }

        public EpisodeInfo(string episodeName, RawTVShow nextEp, RawTVShow prevEp)
        {
            EpisodeName = episodeName;
            NextEp = nextEp;
            PrevEp = prevEp;
        }
    }
}
