using System;

namespace TVShowChecker
{
    class TVShow
    {
        public string Name { get; set; }
        public string CurrEpisodeNumber { get; set; }
        public string NextEpisode { get; set; }
        public string LatestEpisode { get; set; }

        public TVShow()
        {
        }

        public TVShow(string name, string episodeNumber, string nextEpisode, string previousEpisode)
        {
            Name = name;
            CurrEpisodeNumber = episodeNumber;
            NextEpisode = nextEpisode;
            LatestEpisode = previousEpisode;
        }

        public string GetLatestEpisodeTime()
        {
            string latestEpisodeTime = "";
            if (!string.IsNullOrWhiteSpace(LatestEpisode))
            {
                TimeSpan timeLeftSpan = DateTime.Today.Subtract(DateTime.Parse(LatestEpisode));
                if (timeLeftSpan.Days == 1)
                    latestEpisodeTime = $"{timeLeftSpan.Days} day ago";
                else if (timeLeftSpan.Days == 0)
                    latestEpisodeTime = "Today";
                else
                    latestEpisodeTime = $"{timeLeftSpan.Days} days ago";
            }
            return latestEpisodeTime;
        }

        public string GetTimeLeftForNextEpisode()
        {
            string timeLeft = "";
            if (!string.IsNullOrWhiteSpace(NextEpisode))
            {
                TimeSpan timeLeftSpan = DateTime.Parse(NextEpisode).Subtract(DateTime.Today);
                timeLeft = timeLeftSpan.Days + " day";
                if (timeLeftSpan.Days != 1)
                    timeLeft += "s";
                if (timeLeftSpan.Days == 0)
                    timeLeft = "Today";
            }
            return timeLeft;
        }
    }
}
