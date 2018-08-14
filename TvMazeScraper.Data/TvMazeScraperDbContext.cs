using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Data.Model;

namespace TvMazeScraper.Data
{
    public class TvMazeScraperDbContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ShowActor> ShowActors { get; set; }

        public TvMazeScraperDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureShowEntity(modelBuilder);
            ConfigureActorEntity(modelBuilder);
            ConfigureShowActorRelation(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureShowEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>()
                .HasKey(e => e.ShowId);
        }

        private static void ConfigureActorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .HasKey(e => e.ActorId);

            modelBuilder.Entity<Actor>()
                .Property(a => a.BirthDay)
                .HasColumnType("date");
        }

        private static void ConfigureShowActorRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShowActor>()
                .HasKey(s => new {s.ShowId, s.ActorId});
        }
    }
}