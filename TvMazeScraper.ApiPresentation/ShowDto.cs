using System.Collections.Generic;

namespace TvMazeScraper.ApiPresentation
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ActorDto> Cast { get; set; }
    }
}