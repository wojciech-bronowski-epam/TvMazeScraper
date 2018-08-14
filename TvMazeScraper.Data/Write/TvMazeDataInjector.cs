using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Common.TvMazeApiData;
using TvMazeScraper.Data.Model;

namespace TvMazeScraper.Data.Write
{
    public class TvMazeDataInjector : ITvMazeDataManipulator
    {
        private readonly TvMazeScraperDbContext _dbContext;
        private readonly IDataParse _dataParse;

        public TvMazeDataInjector(TvMazeScraperDbContext dbContext, IDataParse dataParse)
        {
            _dbContext = dbContext;
            _dataParse = dataParse;
        }

        public async Task RunAsync(TvMazeShow tvMazeShow, IEnumerable<TvMazePerson> tvMazePersons)
        {
            var allActors = await GetDbActorsForShowAsync(tvMazePersons);
            var dbShow = new Show { Id = tvMazeShow.Id, Name = tvMazeShow.Name };
            dbShow.ShowActors = GetDbRelations(allActors, dbShow);

            await _dbContext.Shows.AddAsync(dbShow);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<IEnumerable<Actor>> GetDbActorsForShowAsync(IEnumerable<TvMazePerson> tvMazePersons)
        {
            var existingDbActors = await GetExistingDbActorsAsync(tvMazePersons);
            var newDbActors = GetNewDbActors(tvMazePersons, existingDbActors);
            var allActors = new List<Actor>(existingDbActors).Concat(newDbActors);
            return allActors;
        }

        private async Task<IEnumerable<Actor>> GetExistingDbActorsAsync(IEnumerable<TvMazePerson> tvMazePersons)
        {
            return await _dbContext
                .Actors       
                .Where(actor => tvMazePersons
                    .Select(p => p.Id)
                    .Contains(actor.Id))
                .ToListAsync();
        }

        private IEnumerable<Actor> GetNewDbActors(IEnumerable<TvMazePerson> tvMazePersons, IEnumerable<Actor> existingDbActors)
        {
            var newActors = tvMazePersons.Where(person => !existingDbActors.Select(actor => actor.Id).Contains(person.Id));
            return newActors.Select(actor => new Actor()
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthDay = _dataParse.StringToDate(actor.BirthDay)
            });
        }

        private static ICollection<ShowActor> GetDbRelations(IEnumerable<Actor> actors, Show dbShow)
        {
            return actors.Select(dbActor => new ShowActor
            {
                Actor = dbActor,
                Show = dbShow
            }).ToList();
        }
    }
}