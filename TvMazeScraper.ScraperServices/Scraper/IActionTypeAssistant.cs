using TvMazeScraper.Common.TvMazeApiData;

namespace TvMazeScraper.ScraperServices.Scraper
{
    public interface IActionTypeAssistant
    {
        int GetFirstPageToCall(int lastShowId);
        bool ShouldOmitShow(int lastShowId, int showId);
        bool ShouldContinue(int lastExistingDbShowId, int pageId);
    }
}