using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IIndexPageDataService
    {
        Task<List<Artist>> GetArtistsAsync();
        Task<List<Album>> GetAlbumsForArtistAsync(int artistId);
    }
}
