using System.Collections.Generic;
using System.Threading.Tasks;
using TVShowChecker.Core.Models;

namespace TVShowChecker.Core.Interfaces
{
    public interface ITVShowService
    {
        Task<IEnumerable<TVShow>> GetTvShows(IEnumerable<string> subscribedTvShows);
    }
}
