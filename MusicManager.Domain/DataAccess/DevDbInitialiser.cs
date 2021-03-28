using MusicManager.Domain.Entities;
using System;
using System.Linq;

namespace MusicManager.Domain.DataAccess
{
    /// <summary>
    /// Helper to seed database with placeholder data during development.
    /// </summary>
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
            var catstepGenre = new Genre { Name = "Catstep", Created = now };
            var screamoGenre = new Genre { Name = "Screamo", Created = now };
            var lowercaseGenre = new Genre { Name = "Lowercase", Created = now };
            var vaporwaveGenre = new Genre { Name = "Vaporwave", Created = now };
            var complextroGenre = new Genre { Name = "Complextro", Created = now };
            var folktronicaGenre = new Genre { Name = "Folktronica", Created = now };
            var jazzGenre = new Genre { Name = "Jazz", Created = now };
            var genres = new[] { catstepGenre, screamoGenre, lowercaseGenre, vaporwaveGenre, complextroGenre, folktronicaGenre };

            // Artist, Album & Track names generated with https://www.name-generator.org.uk/band-name/
            var artists = new[]
            {
                new Artist
                {
                    Name = "Bring Me the Arms",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Crazy Spring",
                            ReleaseYear = 2017,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = catstepGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Teddy Metal",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Wet Can For the Builders",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Cape Town Revival",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Tortoisesica",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Wet Brown Day",
                                    Created = now
                                }
                            },
                        },
                        new Album
                        {
                            Title = "Bob Bob",
                            ReleaseYear = 2019,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = catstepGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "For the Jockies",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Lonely Pink Day",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Lord of the Lonely Frogs",
                                    Created = now
                                }
                            },
                        },
                        new Album
                        {
                            Title = "C.H.O.R.D.",
                            ReleaseYear = 2020,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = catstepGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Grubby Grubby",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Twenty Monkeys",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Wales Thunder Eyebrows",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "Deaf Frogs",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Head A Dozen",
                            ReleaseYear = 2007,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = screamoGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Lonely Soap Brigade",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Les Drums",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Prague",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Au Revior Hips",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Chord Popping English People",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "McKnight",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Skipping at the Disco",
                            ReleaseYear = 2013,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = folktronicaGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Shouting Dolls",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Undercover Rock",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "My Useful Romance",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Five Times Stunning",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Stunningplay",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "Lynette Wonder",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Black Grey White",
                            ReleaseYear = 2011,
                            Created = now,
                            AlbumGenres = new [] {
                                new AlbumGenre { Genre = vaporwaveGenre, Created = now },
                                new AlbumGenre { Genre = catstepGenre, Created = now }
                            },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Pixie Tribute",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Super Moist Lizards",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "My Space Rocket",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Joey Eats the Book",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Puddle of Book",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "Perryatron",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "One Thousand Smiling Iced Lizards",
                            ReleaseYear = 2015,
                            Created = now,
                            AlbumGenres = new [] {
                                new AlbumGenre { Genre = complextroGenre, Created = now },
                                new AlbumGenre { Genre = lowercaseGenre, Created = now }
                            },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Four Lions",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Beyond The North Pole",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Sweaty Thursday",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Bumpy Perry",
                                    Created = now
                                }
                            }
                        },
                        new Album
                        {
                            Title = "Matilidaatron",
                            ReleaseYear = 2019,
                            Created = now,
                            AlbumGenres = new [] {
                                new AlbumGenre { Genre = complextroGenre, Created = now },
                                new AlbumGenre { Genre = lowercaseGenre, Created = now }
                            },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Singing Twins",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Heart Two",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Allo Chaps",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Mild Spacemen's Club",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Four Odd Kittens",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "Chewy Kid Edie",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "Purely Brown",
                            ReleaseYear = 2016,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = jazzGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Cool Goblins",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "No Rest For the Painters",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Fairy Metal",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Compass Tribute",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Why Goats, Why?",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 6,
                                    Title = "The Love of Shoes",
                                    Created = now
                                }
                            }
                        }
                    }
                },
                new Artist
                {
                    Name = "Super Smelly Butterflies",
                    Created = now,
                    Albums = new []
                    {
                        new Album
                        {
                            Title = "The Dancing Pants",
                            ReleaseYear = 2018,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = complextroGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "King Blonde",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Rage Against the Rock",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "No Fishing Rod",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Wild Wild Wild",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 5,
                                    Title = "Working in Japan",
                                    Created = now
                                }
                            }
                        },
                        new Album
                        {
                            Title = "Badgersica",
                            ReleaseYear = 2020,
                            Created = now,
                            AlbumGenres = new [] { new AlbumGenre { Genre = complextroGenre, Created = now } },
                            Tracks = new []
                            {
                                new Track
                                {
                                    TrackNumber = 1,
                                    Title = "Head Failure",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 2,
                                    Title = "Of Men and Blue Bottles",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 3,
                                    Title = "Charlie Eats the Torch",
                                    Created = now
                                },
                                new Track
                                {
                                    TrackNumber = 4,
                                    Title = "Yummyknot",
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
