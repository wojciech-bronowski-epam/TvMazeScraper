using System.Threading.Tasks;
using RestSharp;

namespace TvMazeScraper.ScraperServices.TvMazeClient
{
    public interface ITvMazeHttpClient
    {
        Task<IRestResponse<T>> GetAsync<T>(string url, string queryName = null, string queryValue = null);
    }
}