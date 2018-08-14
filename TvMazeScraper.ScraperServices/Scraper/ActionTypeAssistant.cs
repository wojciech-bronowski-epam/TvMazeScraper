using TvMazeScraper.Common.TvMazeApiData;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public abstract class ActionTypeAssistant : IActionTypeAssistant
    {
        public virtual int GetFirstPageToCall(int lastShowId)
        {
            return 0;
        }

        public virtual bool ShouldOmitShow(int lastShowId, int showId)
        {
            return false;
        }

        public virtual bool ShouldContinue(int lastExistingDbShowId, int pageId)
        {
            return true;
        }
    }
}