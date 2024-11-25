using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VideoGamesCatalog.Core.Data.Models;

namespace VideoGamesCatalog.Core.Data.Context
{
    public class VideoGameContext : DbContext
    {
        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<AgeRating> AgeRating { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<VideoGameImage> VideoGameImages { get; set; }

        public VideoGameContext(DbContextOptions<VideoGameContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoGame>(entity => entity.HasIndex(e => e.Title).IsClustered(false));
        }

    }
}
