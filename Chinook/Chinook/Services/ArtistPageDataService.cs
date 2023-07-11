using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Chinook.Services
{
    

    public class ArtistPageDataService : IArtistPageDataService
    {
        private readonly ChinookContext dbContext;
        public ArtistPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        public async Task<long> GetLastPlaylistIdAsync()
        {
            return await dbContext.Playlists.CountAsync();
        }

        public async Task AddPlaylistAsync(string playListName, long playListId)
        {
            await dbContext.Playlists.AddAsync(new Playlist
            {
                Name = playListName,
                PlaylistId = playListId
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddUserPlaylistAsync(string userId, long playlistId)
        {
            await dbContext.UserPlaylists.AddAsync(new UserPlaylist
            {
                UserId = userId,
                PlaylistId = playlistId
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddPlaylistTrackAsync(long playlistId, long trackId)
        {
            await dbContext.PlaylistTracks.AddAsync(new PlaylistTrack { 
                PlaylistId = playlistId,
                TrackId = trackId
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task<Artist> GetArtistByIdAsync(long id)
        {
            return dbContext.Artists.SingleOrDefault(a => a.ArtistId == id);
        }

        public async Task<List<Playlist>> GetPlaylistsByUserAsync(string userId)
        {
            return dbContext.UserPlaylists.Where(u => u.UserId == userId)
                                .Include(p => p.Playlist)
                                .Select(p => p.Playlist).ToList();
        }

        public async Task<List<PlaylistTrackClientModel>> GetTracksByUserAndArtistAsync(string userId, long artistId)
        {
            return dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
            .Include(a => a.Album)
            .Select(t => new PlaylistTrackClientModel()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites")).Any()
            })
            .ToList();
        }
    }
}
