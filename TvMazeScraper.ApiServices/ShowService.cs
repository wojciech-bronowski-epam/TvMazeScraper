using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvMazeScraper.ApiPresentation;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Data.Model;
using TvMazeScraper.Data.Read;

namespace TvMazeScraper.ApiServices
{
    public class ShowService : IShowService
    {
        private readonly ITvMazeDataReader _tvMazeDataReader;
        private readonly IDataParse _dataParse;

        public ShowService(ITvMazeDataReader tvMazeDataReader, IDataParse dataParse)
        {
            _tvMazeDataReader = tvMazeDataReader;
            _dataParse = dataParse;
        }

        public async Task<IEnumerable<ShowDto>> GetShowsWithActorsAsync(int pageId, int pageSize)
        {
            var showsWithActors = await _tvMazeDataReader.GetShowsWithActorsAsync(pageId, pageSize);
            return GetOrderedResult(showsWithActors);
        }

        private IEnumerable<ShowDto> GetOrderedResult(IEnumerable<Show> showsWithActors)
        {
            return showsWithActors
                .Select(s => new ShowDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cast = s.ShowActors
                            .Select(sa => new ActorDto
                            {
                                Id = sa.Actor.Id,
                                Name = sa.Actor.Name,
                                Birthday = _dataParse.DateToString(sa.Actor.BirthDay)
                            })
                            .OrderByDescending(a => a.Birthday)
                            .ToList()
                });
        }
    }
}