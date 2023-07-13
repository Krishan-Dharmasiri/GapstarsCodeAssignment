using Chinook.Services.Interfaces;

namespace Chinook.Services
{
    /// <summary>
    /// Responsible for maintianing all the functionality that is applicable across the application
    /// </summary>
    public class CommonDataService : ICommonDataService
    {
        public event Action RefreshNavMenu;

        /// <summary>
        /// Raises the event when a playlist is added so that the NavMenu component can be
        /// partially refreshed
        /// </summary>
        public void InvokeRefreshNavMenu()
        {
            try
            {
                RefreshNavMenu?.Invoke();
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while raising the NavMenu event, Error : {ex.Message} ");
            }
            
        }
    }
}
