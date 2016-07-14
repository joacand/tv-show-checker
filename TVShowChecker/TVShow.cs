using System;

namespace TVShowChecker {
    class TVShow {
        public string name { get; set; }
        public string currEpisodeNumber { get; set; }
        public string nextEpisode { get; set; }
        public string latestEpisode { get; set; }

        public TVShow() {
        }

        public TVShow(string name, string episodenr, string nxt, string prv) {
            this.name = name;
            currEpisodeNumber = episodenr;
            nextEpisode = nxt;
            latestEpisode = prv;
        }

        public string getLatestEpisodeTime() {
            string latestEpisodeTime = "";
            if (latestEpisode != "") {
                TimeSpan timeLeftSpan = DateTime.Today.Subtract(DateTime.Parse(latestEpisode));
                if (timeLeftSpan.Days == 1)
                    latestEpisodeTime = timeLeftSpan.Days.ToString() + " day ago";
                else if (timeLeftSpan.Days == 0)
                    latestEpisodeTime = "Today";
                else
                    latestEpisodeTime = timeLeftSpan.Days.ToString() + " days ago";
            }
            return latestEpisodeTime;
        }

        public string getTimeLeftForNextEpisode() {
            string timeLeft = "";
            if (nextEpisode != "") {
                TimeSpan timeLeftSpan = DateTime.Parse(nextEpisode).Subtract(DateTime.Today);
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
