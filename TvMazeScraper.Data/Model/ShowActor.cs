namespace TvMazeScraper.Data.Model
{
    public class ShowActor
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
    }
}