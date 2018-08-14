using System;
using System.Threading.Tasks;
using RestSharp;
using TvMazeScraper.ScraperServices.TvMazeClient.Configuration;

namespace TvMazeScraper.ScraperServices.TvMazeClient
{
    public class TvMazeHttpClient : ITvMazeHttpClient
    {
        private readonly IRestClient _restClient;

        public TvMazeHttpClient(IRestClient restClient, TvMazeApiConfiguration tvMazeApiConfiguration)
        {
            _restClient = restClient;
            _restClient.BaseUrl = new Uri(tvMazeApiConfiguration.BaseUrl);
        }

        public async Task<IRestResponse<T>> GetAsync<T>(string url, string queryName = null, string queryValue = null)
        {
            var request = new RestRequest(url, Method.GET);
            if (!string.IsNullOrEmpty(queryName) && !string.IsNullOrEmpty(queryValue))
            {
                request.AddQueryParameter(queryName, queryValue);
            }
            return await _restClient.ExecuteTaskAsync<T>(request);
        }
    }
}