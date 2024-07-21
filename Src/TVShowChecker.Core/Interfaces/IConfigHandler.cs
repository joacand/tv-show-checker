using System.Collections.Generic;

namespace TVShowChecker.Core.Interfaces;

public interface IConfigHandler
{
    List<string> ReadSubscribedTvShowsFromConfig();
    void SaveTvShowsToConfig(List<string> tvShows);
}
