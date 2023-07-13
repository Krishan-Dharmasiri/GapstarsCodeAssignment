using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Chinook.Services
{
    
    /// <summary>
    /// Responsible for interacting with the database for all the read and write operations using EF
    /// </summary>
    public class ArtistPageDataService : IArtistPageDataService
    {
        private readonly ChinookContext dbContext;
        public ArtistPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        /// <summary>
        /// Returns the playlistId of the "My favorite tracks" playlist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<long> GetDefaultPlaylistIdAsync(string userId)
        {
            try
            {
                return dbContext.UserPlaylists.Include(a => a.Playlist)
                                    .Where(p => p.Playlist.Name == "My favorite tracks")
                                    .Where(u => u.UserId == userId).ToList()
                                    .FirstOrDefault()
                                    .Playlist.PlaylistId;
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching the playlist Id of the My favorite tracks playlist. Error : {ex.Message} ");
            }           
            
        }

        /// <summary>
        /// returns the latest playlist Id from the playlist table
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetLastPlaylistIdAsync()
        {
            try
            {
                return await dbContext.Playlists.CountAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching playlist count, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Adds a new playlist into the Database
        /// </summary>
        /// <param name="playListName"></param>
        /// <param name="playListId"></param>
        /// <returns></returns>
        public async Task AddPlaylistAsync(string playListName, long playListId)
        {
            try
            {
                await dbContext.Playlists.AddAsync(new Playlist
                {
                    Name = playListName,
                    PlaylistId = playListId
                });
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while adding a new playlist Error: {ex.Message}");
            }            
        }

        /// <summary>
        /// Adds a userplaylist entity into the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        public async Task AddUserPlaylistAsync(string userId, long playlistId)
        {
            try
            {
                await dbContext.UserPlaylists.AddAsync(new UserPlaylist
                {
                    UserId = userId,
                    PlaylistId = playlistId
                });
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while adding a new userplaylist entity Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Adds a playlisttrack entity into the database
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public async Task AddPlaylistTrackAsync(long playlistId, long trackId)
        {
            try
            {
                await dbContext.PlaylistTracks.AddAsync(new PlaylistTrack
                {
                    PlaylistId = playlistId,
                    TrackId = trackId
                });
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while adding a playlisttrack entity, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Fetched the Artist by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Artist> GetArtistByIdAsync(long id)
        {
            try
            {
                return dbContext.Artists.SingleOrDefault(a => a.ArtistId == id);
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching the Artist, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Fetches the playlists belongs to a perticular user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Playlist>> GetPlaylistsByUserAsync(string userId)
        {
            try
            {
                return dbContext.UserPlaylists.Where(u => u.UserId == userId)
                                .Include(p => p.Playlist)
                                .Select(p => p.Playlist).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching the playlists, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Gets all the Tracks that are in a given users playlists
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public async Task<List<PlaylistTrackClientModel>> GetTracksByUserAndArtistAsync(string userId, long artistId)
        {
            try
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
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching the Tracks. Error : {ex.Message}");
            }

            
        }
    }
}
