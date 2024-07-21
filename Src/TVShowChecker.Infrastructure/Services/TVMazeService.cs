using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVShowChecker.Core.Interfaces;
using TVShowChecker.Core.Models;

namespace TVShowChecker.Infrastructure.Services;

public sealed partial class TVMazeService : ITVShowService
{
    private const string TVMAZE_API_URL = @"http://api.tvmaze.com/search/shows?q=";

    public async Task<IEnumerable<TVShow>> GetTvShows(IEnumerable<string> subscribedTVShows)
    {
        var apiRequests = GetApiRequests(subscribedTVShows);
        var showInfoJson = (await GetMultipleAPIJson(apiRequests)).Where(x => !string.IsNullOrWhiteSpace(x));

        var taskList = showInfoJson.Select(x => CreateEpisode(x));
        var episodes = (await Task.WhenAll(taskList.ToArray())).Where(x => x != null);

        return episodes.Select(episode => new TVShow(episode.TvShowName, episode?.PrevEp?.EpisodeNumber, episode?.NextEp?.AirDate, episode?.PrevEp?.AirDate));
    }

    private static IEnumerable<string> GetApiRequests(IEnumerable<string> subscribedTVShows) =>
        subscribedTVShows.Select(x => $"{TVMAZE_API_URL}{x}");

    private static async Task<IEnumerable<string>> GetMultipleAPIJson(IEnumerable<string> apiRequests) =>
        await Task.WhenAll(apiRequests.Select(ApiClient.Get));

    private static async Task<TVShowContext> CreateEpisode(string showInfoJson)
    {
        var fullObject = JsonConvert.DeserializeObject<List<ShowResponse>>(showInfoJson);
        if (fullObject.Count == 0) { return null; }

        var showName = fullObject.First().Show.Name;
        var nextEpHrefUri = fullObject.First().Show._Links?.NextEpisode?.Href;
        var prevEpHrefUri = fullObject.First().Show._Links?.PreviousEpisode?.Href;

        var nextEp = await ApiClient.Get(nextEpHrefUri);
        var prevEp = await ApiClient.Get(prevEpHrefUri);

        var nextEpRaw = GenEpisodeInfo(showName, nextEp);
        var prevEpRaw = GenEpisodeInfo(showName, prevEp);
        return new TVShowContext(showName, nextEpRaw, prevEpRaw);
    }

    private static Episode GenEpisodeInfo(string name, string prevEpisodeJson)
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
}
