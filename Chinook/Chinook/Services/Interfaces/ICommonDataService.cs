namespace Chinook.Services.Interfaces
{
    
    public interface ICommonDataService
    {
        public event Action RefreshNavMenu;
        void InvokeRefreshNavMenu();
    }
}
