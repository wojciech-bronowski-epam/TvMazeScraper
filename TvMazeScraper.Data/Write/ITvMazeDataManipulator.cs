using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Common.TvMazeApiData;

namespace TvMazeScraper.Data.Write
{
    public interface ITvMazeDataManipulator
    {
        Task RunAsync(TvMazeShow tvMazeShow, IEnumerable<TvMazePerson> tvMazePersons);
    }
}