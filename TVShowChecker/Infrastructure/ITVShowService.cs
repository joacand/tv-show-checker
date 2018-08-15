using System.Collections.Generic;
using System.Threading.Tasks;
using TVShowChecker.Entities;

namespace TVShowChecker.Infrastructure
{
    interface ITVShowService
    {
        Task<IEnumerable<TVShow>> GetTvShows(List<string> subscribedTvShows);
    }
}
