using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Common.TvMazeApiData;

namespace TvMazeScraper.ScraperServices.TvMazeClient
{
    public interface ITvMazeWebClient
    {
        Task<IEnumerable<TvMazeShow>> GetShowsPagedAsync(int pageNumber);
        Task<IEnumerable<TvMazeActor>> GetCastForAsync(int showId);
    }
}