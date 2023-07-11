using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface INavMenuDataService
    {
        Task AddDefaultPlaylistAsync(string userId);
        Task<List<Playlist>> GetPlaylistsByUserIdAsync(string userId);
    }
}