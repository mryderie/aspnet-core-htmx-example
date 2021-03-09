using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.Entities;

namespace MusicManager.Domain.DataAccess
{
    public class MusicManagerContext : DbContext
    {
        public MusicManagerContext(DbContextOptions<MusicManagerContext> options)
            : base(options)
        {
        }

        internal DbSet<Artist> Artists { get; set; }
        internal DbSet<Album> Albums { get; set; }
        internal DbSet<Track> Tracks { get; set; }
        internal DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumGenre>().HasKey(ag => new { ag.AlbumId, ag.GenreId });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
