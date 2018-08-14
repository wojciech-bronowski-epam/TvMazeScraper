using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Common.TvMazeApiData;
using TvMazeScraper.Data.Read;
using TvMazeScraper.Data.Write;
using TvMazeScraper.ScraperServices.TvMazeClient;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public class ScraperService : IScraperService
    {        
        private readonly ITvMazeWebClient _tvMazeWebClient;
        private ITvMazeDataManipulator _dataInjector;
        private ITvMazeDataManipulator _dataUpdater;
        private readonly ITvMazeDataReader _dataReader;
        private IActionTypeAssistant _newDataAssistant;
        private IActionTypeAssistant _existingDataAssistant;

        public ScraperService(
            ITvMazeWebClient tvMazeWebClient,
            ITvMazeDataReader dataReader,
            IEnumerable<ITvMazeDataManipulator> dataManipulators,
            IEnumerable<IActionTypeAssistant> actionTypeAssistants
        )
        {
            _tvMazeWebClient = tvMazeWebClient;
            _dataReader = dataReader;
            InitializeActionTypeAssistants(actionTypeAssistants);
            InitializeDataManipulators(dataManipulators);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var lastExistingDbShowId = await _dataReader.GetLastShowIdAsync();
            await AddNewData(lastExistingDbShowId, cancellationToken);
            await UpdateExistingData(lastExistingDbShowId, cancellationToken);
        }

        private async Task AddNewData(int lastExistingDbShowId, CancellationToken cancellationToken)
        {
            await RunScraper(_newDataAssistant, _dataInjector, lastExistingDbShowId, cancellationToken);
        }

        private async Task UpdateExistingData(int lastExistingDbShowId, CancellationToken cancellationToken)
        {
            await RunScraper(_existingDataAssistant, _dataUpdater, lastExistingDbShowId, cancellationToken);
        }

        private async Task RunScraper(
            IActionTypeAssistant actionTypeAssistant, 
            ITvMazeDataManipulator dataManipulator, 
            int lastShowId,
            CancellationToken cancellationToken)
        {
            var pageId = actionTypeAssistant.GetFirstPageToCall(lastShowId);
            IEnumerable<TvMazeShow> tvMazeShows;
            do
            {
                tvMazeShows = await GetTvMazeShowsPagedAsync(pageId++);
                foreach (var show in tvMazeShows)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    if (actionTypeAssistant.ShouldOmitShow(lastShowId, show.Id)) continue;
                    var actors = await GetTvMazeCastAsync(show);
                    await dataManipulator.RunAsync(show, actors.Select(actor => actor.Person));
                }
            } while (actionTypeAssistant.ShouldContinue(lastShowId, pageId) && tvMazeShows.Any() && !cancellationToken.IsCancellationRequested);
        }

        private async Task<IEnumerable<TvMazeShow>> GetTvMazeShowsPagedAsync(int pageId)
        {
            return await _tvMazeWebClient.GetShowsPagedAsync(pageId);
        }

        private async Task<IEnumerable<TvMazeActor>> GetTvMazeCastAsync(TvMazeShow show)
        {
            return await _tvMazeWebClient.GetCastForAsync(show.Id);
        }

        private void InitializeActionTypeAssistants(IEnumerable<IActionTypeAssistant> actionTypeAssistants)
        {
            foreach (var actionTypeAssistant in actionTypeAssistants)
            {
                if (actionTypeAssistant.GetType() == typeof(NewDataAssistant))
                {
                    _newDataAssistant = actionTypeAssistant;
                }
                else if (actionTypeAssistant.GetType() == typeof(ExistingDataAssistant))
                {
                    _existingDataAssistant = actionTypeAssistant;
                }
            }
        }

        private void InitializeDataManipulators(IEnumerable<ITvMazeDataManipulator> dataManipulators)
        {
            foreach (var dataManipulator in dataManipulators)
            {
                if (dataManipulator.GetType() == typeof(TvMazeDataInjector))
                {
                    _dataInjector = dataManipulator;
                }
                else if (dataManipulator.GetType() == typeof(TvMazeDataUpdater))
                {
                    _dataUpdater = dataManipulator;
                }
            }
        }

    }
}
