using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistPageDataService : IPlaylistPageDataService
    {
        private readonly ChinookContext dbContext;
        public PlaylistPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        public async Task<PlaylistClientModel> GetPlaylistByUserIdAndPlaylistIdAsync(string userId, long playlistId)
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

            //Playlist = DbContext.Playlists
            //    .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
            //    .Where(p => p.PlaylistId == PlaylistId)
            //    .Select(p => new ClientModels.PlaylistClientModel()
            //        {
            //            Name = p.Name,
            //            Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrackClientModel()
            //            {
            //                AlbumTitle = t.Album.Title,
            //                ArtistName = t.Album.Artist.Name,
            //                TrackId = t.TrackId,
            //                TrackName = t.Name,
            //                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == "Favorites")).Any()
            //            }).ToList()
            //        })
            //    .FirstOrDefault();

        }
    }
}
