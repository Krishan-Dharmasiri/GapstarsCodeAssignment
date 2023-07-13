using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    /// <summary>
    /// Responisble for dealing with ArtistPage component relatyed DB interactions
    /// </summary>
    public class IndexPageDataService : IIndexPageDataService
    {
        private readonly ChinookContext dbContext;
        public IndexPageDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            dbContext = DbFactory.CreateDbContext();
        }

        /// <summary>
        /// Fetchs the artists from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Artist>> GetArtistsAsync()
        {
            try
            {
                var users = dbContext.Users.Include(a => a.UserPlaylists).ToList();
                return dbContext.Artists.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching Artists, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Fetches all the Albums belong to a perticular Artist
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public async Task<List<Album>> GetAlbumsForArtistAsync(int artistId)
        {
            try
            {
                return dbContext.Albums.Where(a => a.ArtistId == artistId).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching Albums, Error : {ex.Message}");
            }
            
        }
    }
}
