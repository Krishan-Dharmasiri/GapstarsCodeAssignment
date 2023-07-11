using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Chinook.Services
{
    public class NavMenuDataService : INavMenuDataService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;
        public NavMenuDataService(IDbContextFactory<ChinookContext> DbFactory)
        {
            _dbFactory = DbFactory;
        }

        public async Task<List<Playlist>> GetPlaylistsByUserIdAsync(string userId)
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

        public async Task AddDefaultPlaylistAsync(string userId)
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
    }
}
