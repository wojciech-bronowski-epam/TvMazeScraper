namespace TvMazeScraper.ScraperServices.TvMazeClient.Configuration
{
    public class TvMazeApiConfiguration
    {
        public string BaseUrl => "http://api.tvmaze.com";
        public string Shows => "shows";
        public string Cast => "cast";
        public string Page => "page";
        public int MaxShowsInPage => 250;
        public int DelayTimeSeconds => 1;
    }
}