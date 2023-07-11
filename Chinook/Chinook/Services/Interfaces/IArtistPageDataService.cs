using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IArtistPageDataService
    {
        Task<Artist> GetArtistByIdAsync(long id);
        Task<List<PlaylistTrackClientModel>> GetTracksByUserAndArtistAsync(string userId, long artistId);
        Task<List<Playlist>> GetPlaylistsByUserAsync(string userId);
        Task AddPlaylistTrackAsync(long playlistId, long trackId);
        Task<long> GetLastPlaylistIdAsync();
        Task AddPlaylistAsync(string playListName, long playListId);
        Task AddUserPlaylistAsync(string userId, long playlistId);
    }
}
