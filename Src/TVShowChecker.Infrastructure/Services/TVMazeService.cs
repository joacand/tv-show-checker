using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TVShowChecker.Core.Interfaces;
using TVShowChecker.Core.Models;

namespace TVShowChecker.Infrastructure.Services
{
    public class TVMazeService : ITVShowService
    {
        private static readonly string TVMAZE_API_URL = @"http://api.tvmaze.com/search/shows?q=";
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<IEnumerable<TVShow>> GetTvShows(IEnumerable<string> subscribedTVShows)
        {
            var apiRequests = GetApiRequests(subscribedTVShows);
            var showInfoJson = (await GetMultipleAPIJson(apiRequests)).Where(x => !string.IsNullOrWhiteSpace(x));

            var taskList = showInfoJson.Select(x => CreateEpisode(x));
            var episodes = (await Task.WhenAll(taskList.ToArray())).Where(x => x != null);

            var tvShows = episodes.Select(episode => new TVShow(episode.TvShowName, episode?.PrevEp?.EpisodeNumber, episode?.NextEp?.AirDate, episode?.PrevEp?.AirDate));
            return tvShows;
        }

        private IEnumerable<string> GetApiRequests(IEnumerable<string> subscribedTVShows) =>
            subscribedTVShows.Select(x => $"{TVMAZE_API_URL}{x}");

        private async Task<IEnumerable<string>> GetMultipleAPIJson(IEnumerable<string> apiRequests) =>
            await Task.WhenAll(apiRequests.Select(x => GetAPIJson(x)));

        private async Task<TVShowContext> CreateEpisode(string showInfoJson)
        {
            var fullObject = JsonConvert.DeserializeObject<List<ShowResponse>>(showInfoJson);
            if (!fullObject.Any())
            {
                return null;
            }

            var showName = fullObject.First().Show.Name;
            var nextEpHref = fullObject.First().Show._Links?.NextEpisode?.Href;
            var prevEpHref = fullObject.First().Show._Links?.PreviousEpisode?.Href;

            var nextEp = await GetEpisodeInfoJson(nextEpHref);
            var prevEp = await GetEpisodeInfoJson(prevEpHref);

            var nextEpRaw = GenEpisodeInfo(showName, nextEp);
            var prevEpRaw = GenEpisodeInfo(showName, prevEp);
            return new TVShowContext(showName, nextEpRaw, prevEpRaw);
        }

        private async Task<string> GetEpisodeInfoJson(string apiRequest) => await GetAPIJson(apiRequest);

        private Episode GenEpisodeInfo(string name, string prevEpisodeJson)
        {
            if (string.IsNullOrWhiteSpace(prevEpisodeJson))
            {
                return null;
            }

            var fullObject = JsonConvert.DeserializeObject<EpisodeResponse>(prevEpisodeJson);

            var airDate = fullObject.Airdate;
            var season = fullObject.Season;
            season = season?.PadLeft(2, '0');
            var number = fullObject.Number;
            number = number?.PadLeft(2, '0');

            return new Episode(name, airDate, $"S{season}E{number}");
        }

        private async Task<string> GetAPIJson(string apiRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiRequest))
                {
                    return null;
                }

                var response = await httpClient.GetAsync(apiRequest);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }

        private class ShowResponse
        {
            public Show Show { get; set; }
        }

        private class Show
        {
            public string Name { get; set; }
            public ShowLinks _Links { get; set; }
        }

        private class ShowLinks
        {
            public EpisodeHref NextEpisode { get; set; }
            public EpisodeHref PreviousEpisode { get; set; }
        }

        private class EpisodeHref
        {
            public string Href { get; set; }
        }

        private class EpisodeResponse
        {
            public string Airdate { get; set; }
            public string Season { get; set; }
            public string Number { get; set; }
        }
    }
}
