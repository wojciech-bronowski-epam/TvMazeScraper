using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Common.TvMazeApiData;
using TvMazeScraper.Data.Model;

namespace TvMazeScraper.Data.Write
{
    public class TvMazeDataUpdater : ITvMazeDataManipulator
    {
        private readonly TvMazeScraperDbContext _dbContext;
        private readonly IDataParse _dataParse;

        public TvMazeDataUpdater(TvMazeScraperDbContext dbContext, IDataParse dataParse)
        {
            _dbContext = dbContext;
            _dataParse = dataParse;
        }
        public async Task RunAsync(TvMazeShow tvMazeShow, IEnumerable<TvMazePerson> tvMazePersons)
        {
            // To be implemented...
        }

    }
}