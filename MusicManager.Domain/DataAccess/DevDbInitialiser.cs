using MusicManager.Domain.Entities;
using System;
using System.Linq;

namespace MusicManager.Domain.DataAccess
{
    public static class DevDbInitialiser
    {
        public static void Initialise(MusicManagerContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Artists.Any())
            {
                return;   // DB has been seeded
            }

            var now = DateTime.UtcNow;
            var popGenre = new Genre { Name = "Pop", Created = now };
            var rockGenre = new Genre { Name = "Rock", Created = now };
            var genres = new[] { popGenre, rockGenre };
            
            var artists = new[]
            {
                new Artist
                {
                    Name = "Led Zeppelin",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Houses of the Holy",
                            ReleaseYear = 1973,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = rockGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "The Song Remains the Same",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "The Rain Song",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "ABBA",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Arrival",
                            ReleaseYear = 1976,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = popGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "When I Kissed the Teacher",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Dancing Queen",
                                    Created = now
                                }
                            }
                        }
                    }
                },

            };

            dbContext.Genres.AddRange(genres);
            dbContext.Artists.AddRange(artists);
            dbContext.SaveChanges();
        }
    }
}
