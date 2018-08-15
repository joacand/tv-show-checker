using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TVShowChecker.Entities;

namespace TVShowChecker.Infrastructure
{
    class TVMazeService : ITVShowService
    {
        private static readonly string TVMAZE_API_URL = @"http://api.tvmaze.com/search/shows?q=";

        public async Task<IEnumerable<TVShow>> GetTvShows(List<string> subscribedTVShows)
        {
            string[] apiRequests = GetApiRequests(subscribedTVShows);
            var showInfoJson = new List<string>(await GetMultipleAPIJson(apiRequests));

            var taskList = new List<Task<TVShowContext>>();
            for (int i = 0; i < subscribedTVShows.Count; i++)
            {
                taskList.Add(CreateEpisode(showInfoJson[i]));
            }

            var episodes = await Task.WhenAll(taskList.ToArray());
            var episodeInfos = new List<TVShowContext>(episodes);

            var tvShows = episodes.Select(episode => new TVShow(episode.TvShowName, episode?.PrevEp?.EpisodeNumber, episode?.NextEp?.AirDate, episode?.PrevEp?.AirDate));
            return tvShows;
        }

        private string[] GetApiRequests(List<string> subscribedTVShows)
        {
            string[] apiRequests = new string[subscribedTVShows.Count];
            for (int i = 0; i < subscribedTVShows.Count; i++)
            {
                apiRequests[i] = TVMAZE_API_URL + subscribedTVShows[i];
            }
            return apiRequests;
        }

        private async Task<string[]> GetMultipleAPIJson(string[] apiRequests)
        {
            List<string> res = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (string req in apiRequests)
            {
                tasks.Add(GetAPIJson(req));
            }
            foreach (Task<string> task in tasks)
            {
                res.Add(await task);
            }
            return res.ToArray();
        }

        private async Task<string> GetAPIJson(string apiRequest)
        {
            if (apiRequest == null)
            {
                return null;
            }

            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(apiRequest);
            }
        }

        private async Task<TVShowContext> CreateEpisode(string showInfoJson)
        {
            var fullObject = JsonConvert.DeserializeObject<dynamic>(showInfoJson);
            if (fullObject.First == null)
            {
                return null;
            }
            string showName = fullObject.First.show.name;
            string nextEpHref = fullObject.First.show._links?.nextepisode?.href;
            string prevEpHref = fullObject.First.show._links?.previousepisode?.href;

            var nextEp = await GetEpisodeInfoJson(nextEpHref);
            var prevEp = await GetEpisodeInfoJson(prevEpHref);

            var nextEpRaw = GenEpisodeInfo(showName, nextEp);
            var prevEpRaw = GenEpisodeInfo(showName, prevEp);
            return new TVShowContext(showName, nextEpRaw, prevEpRaw);
        }

        private async Task<string> GetEpisodeInfoJson(string apiRequest)
        {
            return await GetAPIJson(apiRequest);
        }

        private Episode GenEpisodeInfo(string name, string prevEpisodeJson)
        {
            if (string.IsNullOrWhiteSpace(prevEpisodeJson))
            {
                return null;
            }

            var fullObject = JsonConvert.DeserializeObject<dynamic>(prevEpisodeJson);

            string airDate = fullObject.airdate;
            string season = fullObject.season;
            season = season.PadLeft(2, '0');
            string number = fullObject.number;
            number = number.PadLeft(2, '0');

            return new Episode(name, airDate, $"S{season}E{number}");
        }
    }
}
