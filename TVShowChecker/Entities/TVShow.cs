using System;

namespace TVShowChecker.Entities
{
    class TVShow
    {
        public string Name { get; set; }
        public string CurrentEpisodeNumber { get; set; }
        public string NextEpisode { get; set; }
        public string PreviousEpisode { get; set; }
        
        public TVShow(string name, string episodeNumber, string nextEpisode, string previousEpisode)
        {
            Name = name;
            CurrentEpisodeNumber = episodeNumber;
            NextEpisode = nextEpisode;
            PreviousEpisode = previousEpisode;
        }

        public string GetPreviousEpisodeTime()
        {
            string previousEpisodeTime = "";
            if (!string.IsNullOrWhiteSpace(PreviousEpisode))
            {
                TimeSpan timeLeftSpan = DateTime.Today.Subtract(DateTime.Parse(PreviousEpisode));
                if (timeLeftSpan.Days == 1)
                    previousEpisodeTime = $"{timeLeftSpan.Days} day ago";
                else if (timeLeftSpan.Days == 0)
                    previousEpisodeTime = "Today";
                else
                    previousEpisodeTime = $"{timeLeftSpan.Days} days ago";
            }
            return previousEpisodeTime;
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
