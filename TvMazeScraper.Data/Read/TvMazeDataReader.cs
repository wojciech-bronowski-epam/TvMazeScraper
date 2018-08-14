using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Data.Model;

namespace TvMazeScraper.Data.Read
{
    public class TvMazeDataReader : ITvMazeDataReader
    {
        private const int FirstPageId = 0;
        private readonly TvMazeScraperDbContext _dbContext;

        public TvMazeDataReader(TvMazeScraperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetLastShowIdAsync()
        {
            var lastShow = await _dbContext.Shows.AsNoTracking().LastOrDefaultAsync();
            return lastShow?.Id ?? FirstPageId;
        }

        public async Task<IEnumerable<Show>> GetShowsWithActorsAsync(int pageId, int pageSize)
        {
            var showsWithActors = await _dbContext
                .Shows.AsNoTracking()
                .Include(s => s.ShowActors)
                .ThenInclude(s => s.Actor)
                .Skip((pageId - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return showsWithActors;
        }
    }
}