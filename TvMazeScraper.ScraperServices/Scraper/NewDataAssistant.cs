using TvMazeScraper.ScraperServices.TvMazeClient.Configuration;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public class NewDataAssistant : ActionTypeAssistant
    {
        private readonly TvMazeApiConfiguration _tvMazeApiConfiguration;

        public NewDataAssistant(TvMazeApiConfiguration tvMazeApiConfiguration)
        {
            _tvMazeApiConfiguration = tvMazeApiConfiguration;
        }

        public override int GetFirstPageToCall(int lastShowId)
        {
            return lastShowId / _tvMazeApiConfiguration.MaxShowsInPage;
        }

        public override bool ShouldOmitShow(int lastShowId, int showId)
        {
            return showId <= lastShowId;
        }
    }
}