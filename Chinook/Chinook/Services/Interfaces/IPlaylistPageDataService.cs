using Chinook.ClientModels;

namespace Chinook.Services.Interfaces
{
    public interface IPlaylistPageDataService
    {
        Task<PlaylistClientModel> GetPlaylistByUserIdAndPlaylistIdAsync(string userId, long playlistId);
    }
}
