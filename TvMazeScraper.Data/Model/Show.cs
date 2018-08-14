using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Data.Model
{
    [Table("Shows")]
    public class Show
    {
        public int ShowId { get; set; }
        public int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public ICollection<ShowActor> ShowActors { get; set; }
    }
}