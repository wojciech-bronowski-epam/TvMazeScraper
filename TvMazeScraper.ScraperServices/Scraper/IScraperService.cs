using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public interface IScraperService
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}