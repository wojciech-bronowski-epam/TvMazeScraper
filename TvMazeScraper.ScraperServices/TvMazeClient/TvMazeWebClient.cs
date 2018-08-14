using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TvMazeScraper.Common.TvMazeApiData;
using TvMazeScraper.ScraperServices.TvMazeClient.Configuration;

namespace TvMazeScraper.ScraperServices.TvMazeClient
{
    public class TvMazeWebClient : ITvMazeWebClient
    {
        private readonly ITvMazeHttpClient _httpClient;
        private readonly TvMazeApiConfiguration _tvMazeApiConfiguration;

        public TvMazeWebClient(ITvMazeHttpClient httpClient, TvMazeApiConfiguration tvMazeApiConfiguration)
        {
            _httpClient = httpClient;
            _tvMazeApiConfiguration = tvMazeApiConfiguration;
        }

        public async Task<IEnumerable<TvMazeShow>> GetShowsPagedAsync(int pageNumber)
        {
            return await CallTvMazeApiAsync<List<TvMazeShow>>(_tvMazeApiConfiguration.Shows, _tvMazeApiConfiguration.Page, pageNumber.ToString());
        }

        public async Task<IEnumerable<TvMazeActor>> GetCastForAsync(int showId)
        {
            return await CallTvMazeApiAsync<List<TvMazeActor>>($"{_tvMazeApiConfiguration.Shows}/{showId}/{_tvMazeApiConfiguration.Cast}");
        }

        private async Task<TReturn> CallTvMazeApiAsync<TReturn>(string url, string queryName = null, string queryValue = null) 
            where TReturn : new()
        {
            while (true)
            {
                var response = await _httpClient.GetAsync<TReturn>(url, queryName, queryValue);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return response.Data;
                    case HttpStatusCode.NotFound:
                        return new TReturn();
                    case HttpStatusCode.TooManyRequests:
                        await Task.Delay(TimeSpan.FromSeconds(_tvMazeApiConfiguration.DelayTimeSeconds));
                        break;
                    default:
                        throw new Exception($"TvMaze api responded with status code {response.StatusCode}");
                }
            }
        }
    }
}