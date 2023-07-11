using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class IndexPageDataService : IIndexPageDataService
    {
        private readonly ChinookContext dbContext;
        public IndexPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        public async Task<List<Artist>> GetArtistsAsync()
        {
            var users = dbContext.Users.Include(a => a.UserPlaylists).ToList();
            return dbContext.Artists.ToList();
        }

        public async Task<List<Album>> GetAlbumsForArtistAsync(int artistId)
        {
            return dbContext.Albums.Where(a => a.ArtistId == artistId).ToList();
        }
    }
}
