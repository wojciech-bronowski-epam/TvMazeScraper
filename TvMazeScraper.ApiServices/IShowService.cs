using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.ApiPresentation;

namespace TvMazeScraper.ApiServices
{
    public interface IShowService
    {
        Task<IEnumerable<ShowDto>> GetShowsWithActorsAsync(int pageId, int pageSize);
    }
}