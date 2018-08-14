using TvMazeScraper.Common.TvMazeApiData;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public class ExistingDataAssistant : ActionTypeAssistant
    {
        public override bool ShouldContinue(int lastExistingDbShowId, int pageId)
        {
            return lastExistingDbShowId != pageId;
        }
    }
}