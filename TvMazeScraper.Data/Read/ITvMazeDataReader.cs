using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Data.Model;

namespace TvMazeScraper.Data.Read
{
    public interface ITvMazeDataReader
    {
        Task<int> GetLastShowIdAsync();
        Task<IEnumerable<Show>> GetShowsWithActorsAsync(int pageId, int pageSize);
    }
}