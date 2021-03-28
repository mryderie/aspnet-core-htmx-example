using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Dtos.Genre;
using MusicManager.Domain.Dtos.Track;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public class DataReadService : IDataReadService
    {
        private readonly MusicManagerContext _dbContext;

        public DataReadService(MusicManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(int genreCount, int artistCount, int albumCount, int trackCount)> GetEntityCounts()
        {
            var genreCount = await _dbContext.Genres.CountAsync();
            var artistCount = await _dbContext.Artists.CountAsync();
            var albumCount = await _dbContext.Albums.CountAsync();
            var trackCount = await _dbContext.Tracks.CountAsync();

            return (genreCount, artistCount, albumCount, trackCount);
        }

        // Artists
        public async Task<ArtistViewDto> GetArtistView(int id)
        {
            return await _dbContext.Artists
                                    .Select(a => new ArtistViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ArtistEditDto> GetArtistEdit(int id)
        {
            return await _dbContext.Artists
                                    .Select(a => new ArtistEditDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<ArtistViewDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField,
                                                                                    bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Artists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "albumcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.Albums.Count);
                    else
                        query = query.OrderBy(a => a.Albums.Count)
                                        .ThenBy(a => a.Name);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Name);
                    else
                        query = query.OrderBy(a => a.Name);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new ArtistViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<IList<(int artistId, string artistName)>> GetArtistNames()
        {
            var result = await _dbContext.Artists.OrderBy(a => a.Name)
                                                .Select(a => new { a.Id, a.Name })
                                                .ToListAsync();

            return result.Select(a => (a.Id, a.Name)).ToList();
        }


        // Albums
        public async Task<AlbumViewDto> GetAlbumView(int id)
        {
            return await _dbContext.Albums
                                    .Select(a => new AlbumViewDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        TrackCount = a.Tracks.Count,
                                        Genres = a.AlbumGenres.Select(g => g.Genre.Name).ToList(),
                                        ArtistId = a.ArtistId,
                                        ArtistName = a.Artist.Name,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AlbumEditDto> GetAlbumEdit(int id)
        {
            return await _dbContext.Albums
                                    .Select(a => new AlbumEditDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        ArtistId = a.ArtistId,
                                        GenreIds = a.AlbumGenres.Select(g => g.GenreId).ToList()
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<AlbumViewDto> pageItems, int totalCount)> GetAlbumsPage(int? artistId, string search, string sortField,
                                                                                        bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Albums.AsQueryable();

            if (artistId.HasValue)
                query = query.Where(a => a.ArtistId == artistId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Title, $"%{search}%")
                                        || EF.Functions.Like(a.Artist.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "trackcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.Tracks.Count)
                                        .OrderBy(a => a.Artist.Name)
                                        .ThenBy(a => a.Title);
                    else
                        query = query.OrderBy(a => a.Tracks.Count)
                                        .OrderBy(a => a.Artist.Name)
                                        .ThenBy(a => a.Title);
                    break;
                case "releaseyear":
                    if (descending)
                        query = query.OrderByDescending(a => a.ReleaseYear)
                                        .OrderBy(a => a.Artist.Name)
                                        .ThenBy(a => a.Title);
                    else
                        query = query.OrderBy(a => a.ReleaseYear)
                                        .OrderBy(a => a.Artist.Name)
                                        .ThenBy(a => a.Title);
                    break;
                case "artistname":
                    if (descending)
                        query = query.OrderByDescending(a => a.Artist.Name)
                                        .ThenBy(a => a.ReleaseYear)
                                        .ThenBy(a => a.Title);
                    else
                        query = query.OrderBy(a => a.Artist.Name)
                                        .ThenBy(a => a.ReleaseYear)
                                        .ThenBy(a => a.Title);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Title);
                    else
                        query = query.OrderBy(a => a.Title);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new AlbumViewDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        TrackCount = a.Tracks.Count,
                                        Genres = a.AlbumGenres.Select(g => g.Genre.Name).ToList(),
                                        ArtistId = a.ArtistId,
                                        ArtistName = a.Artist.Name,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<IList<(int albumId, string albumTitle)>> GetAlbumTitles()
        {
            var result = await _dbContext.Albums.OrderBy(a => a.Title)
                                                .Select(a => new { a.Id, a.Title })
                                                .ToListAsync();

            return result.Select(a => (a.Id, a.Title)).ToList();
        }


        // Genres

        public async Task<IList<(int genreId, string genreName)>> GetGenreNames()
        {
            var result = await _dbContext.Genres.OrderBy(g => g.Name)
                                                .Select(g => new { g.Id, g.Name })
                                                .ToListAsync();

            return result.Select(g => (g.Id, g.Name)).ToList();
        }
        
        public async Task<(IList<GenreViewDto> pageItems, int totalCount)> GetGenresPage(string search, string sortField, bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Genres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => EF.Functions.Like(g.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "albumcount":
                    if (descending)
                        query = query.OrderByDescending(g => g.AlbumGenres.Count);
                    else
                        query = query.OrderBy(g => g.AlbumGenres.Count);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(g => g.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(g => g.Updated);
                    else
                        query = query.OrderBy(g => g.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(g => g.Name);
                    else
                        query = query.OrderBy(g => g.Name);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(g => new GenreViewDto
                                    {
                                        Id = g.Id,
                                        Name = g.Name,
                                        AlbumCount = g.AlbumGenres.Count,
                                        Created = g.Created,
                                        Updated = g.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<GenreViewDto> GetGenreView(int id)
        {
            return await _dbContext.Genres
                                    .Select(g => new GenreViewDto
                                    {
                                        Id = g.Id,
                                        Name = g.Name,
                                        AlbumCount = g.AlbumGenres.Count,
                                        Created = g.Created,
                                        Updated = g.Updated
                                    })
                                    .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<GenreEditDto> GetGenreEdit(int id)
        {
            return await _dbContext.Genres
                                    .Select(g => new GenreEditDto
                                    {
                                        Id = g.Id,
                                        Name = g.Name
                                    })
                                    .FirstOrDefaultAsync(g => g.Id == id);
        }


        // Tracks

        public async Task<TrackViewDto> GetTrackView(int id)
        {
            return await _dbContext.Tracks
                                    .Select(t => new TrackViewDto
                                    {
                                        Id = t.Id,
                                        Title = t.Title,
                                        TrackNumber = t.TrackNumber,
                                        AlbumTitle = t.Album.Title,
                                        AlbumId = t.AlbumId,
                                        ArtistId = t.Album.ArtistId,
                                        ArtistName = t.Album.Artist.Name,
                                        Created = t.Created,
                                        Updated = t.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<TrackViewDto> pageItems, int totalCount)> GetTracksPage(int? albumId, string search, string sortField, bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Tracks.AsQueryable();

            if (albumId.HasValue)
                query = query.Where(t => t.AlbumId == albumId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => EF.Functions.Like(t.Title, $"%{search}%")
                                        || EF.Functions.Like(t.Album.Title, $"%{search}%")
                                        || EF.Functions.Like(t.Album.Artist.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "artistname":
                    if (descending)
                        query = query.OrderByDescending(t => t.Album.Artist.Name)
                                    .ThenBy(t => t.Album.ReleaseYear)
                                    .ThenBy(t => t.Album.Title)
                                    .ThenBy(t => t.TrackNumber);
                    else
                        query = query.OrderBy(t => t.Album.Artist.Name)
                                    .ThenBy(t => t.Album.ReleaseYear)
                                    .ThenBy(t => t.Album.Title)
                                    .ThenBy(t => t.TrackNumber);
                    break;
                case "albumtitle":
                    if (descending)
                        query = query.OrderByDescending(t => t.Album.Title)
                                    .ThenBy(t => t.TrackNumber);
                    else
                        query = query.OrderBy(t => t.Album.Title)
                                    .ThenBy(t => t.TrackNumber);
                    break;
                case "tracknumber":
                    if (descending)
                        query = query.OrderByDescending(t => t.TrackNumber)
                                    .ThenBy(t => t.Album.Artist.Name)
                                    .ThenBy(t => t.Album.ReleaseYear)
                                    .ThenBy(t => t.Album.Title);
                    else
                        query = query.OrderBy(t => t.TrackNumber)
                                    .ThenBy(t => t.Album.Artist.Name)
                                    .ThenBy(t => t.Album.ReleaseYear)
                                    .ThenBy(t => t.Album.Title);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(t => t.Created);
                    else
                        query = query.OrderBy(t => t.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(t => t.Updated);
                    else
                        query = query.OrderBy(t => t.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(t => t.Title);
                    else
                        query = query.OrderBy(t => t.Title);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(t => new TrackViewDto
                                    {
                                        Id = t.Id,
                                        ArtistId = t.Album.ArtistId,
                                        ArtistName = t.Album.Artist.Name,
                                        AlbumId = t.AlbumId,
                                        AlbumTitle = t.Album.Title,
                                        Title = t.Title,
                                        TrackNumber = t.TrackNumber,
                                        Created = t.Created,
                                        Updated = t.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<TrackEditDto> GetTrackEdit(int id)
        {
            return await _dbContext.Tracks
                                    .Select(t => new TrackEditDto
                                    {
                                        Id = t.Id,
                                        Title = t.Title,
                                        TrackNumber = t.TrackNumber,
                                        AlbumId = t.AlbumId,
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
