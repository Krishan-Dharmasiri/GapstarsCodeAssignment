using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    /// <summary>
    /// Playlist Page component related DB interactions
    /// </summary>
    public class PlaylistPageDataService : IPlaylistPageDataService
    {
        private readonly ChinookContext dbContext;
        public PlaylistPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        /// <summary>
        /// Featches the Playlist for a given user and given playlist id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        public async Task<PlaylistClientModel> GetPlaylistByUserIdAndPlaylistIdAsync(string userId, long playlistId)
        {
            try
            {
                var res = dbContext.Playlists
                        .Include(a => a.PlaylistTracks)
                        .ThenInclude(a => a.Track)
                        .ThenInclude(a => a.Album)
                        .ThenInclude(a => a.Artist)
                        .Where(w => w.PlaylistId == playlistId)
                        .FirstOrDefault();

                foreach (var item in res.PlaylistTracks)
                {
                    res.Tracks.Add(item.Track);
                }
                var playlist = new PlaylistClientModel
                {
                    Name = res.Name,
                    Tracks = res.Tracks.Select(t => new PlaylistTrackClientModel()
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites")).Any()
                    }).ToList()
                };

                return playlist;
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fething the playlist, Error : {ex.Message}");
            }

        }
    }
}
