using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Chinook.Services
{
    /// <summary>
    /// NavMenu component related functionalities
    /// </summary>
    public class NavMenuDataService : INavMenuDataService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;
        public NavMenuDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            _dbFactory = DbFactory;
        }

        /// <summary>
        /// Fetches the playlists that belong to a perticular user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Playlist>> GetPlaylistsByUserIdAsync(string userId)
        {
            try
            {
                var _dbContext = await _dbFactory.CreateDbContextAsync();
                // Get the currently avaialable playlists for the current user
                var _mainUserList = _dbContext.UserPlaylists
                                            .Include(a => a.Playlist)
                                            .Where(u => u.UserId == userId)
                                            .ToList();

                List<Playlist> list = new List<Playlist>();

                foreach (var up in _mainUserList)
                {
                    list.Add(up.Playlist);
                }
                return list;
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while fetching Playlists, Error : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Adds the default play list named "My Favorite tracks" automatically
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddDefaultPlaylistAsync(string userId)
        {
            try
            {
                var defaultPlayListName = "My favorite tracks";
                var CurrentPlaylistFromDb = await GetPlaylistsByUserIdAsync(userId);

                var isDefaultPlaylistExists = CurrentPlaylistFromDb.Any(p => p.Name == defaultPlayListName);
                if (!isDefaultPlaylistExists)
                {
                    var _dbContext = await _dbFactory.CreateDbContextAsync();
                    // Add the playlist into the DB
                    long newId = _dbContext.Playlists.Count() + 1;
                    var defaultPlaylist = new Playlist
                    {
                        Name = defaultPlayListName,
                        PlaylistId = newId
                    };

                    await _dbContext.Playlists.AddAsync(defaultPlaylist);


                    await _dbContext.UserPlaylists.AddAsync(new UserPlaylist
                    {
                        UserId = userId,
                        PlaylistId = newId
                    });
                    var result = await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex) 
            {
                throw new Exception($"An error occured while adding the default playlist, Error : {ex.Message}");
            }
            
        }
    }
}
