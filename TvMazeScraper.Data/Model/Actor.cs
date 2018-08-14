using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Data.Model
{
    [Table("Actors")]
    public class Actor
    {        
        public int ActorId { get; set; }
        public int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}