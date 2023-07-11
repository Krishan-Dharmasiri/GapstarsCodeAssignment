using Chinook.Services.Interfaces;

namespace Chinook.Services
{
    public class CommonDataService : ICommonDataService
    {
        public event Action RefreshNavMenu;

        public void InvokeRefreshNavMenu()
        {
            RefreshNavMenu?.Invoke();
        }
    }
}
